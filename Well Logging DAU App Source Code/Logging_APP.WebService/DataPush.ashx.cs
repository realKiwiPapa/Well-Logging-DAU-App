using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Text.RegularExpressions;
using Logging_App.WebService.DAL;

namespace Logging_App.WebService
{
    /// <summary>
    /// DataPush 的摘要说明
    /// </summary>
    public class DataPush : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        private static bool isRunning = false;
        private HttpContext _context;
        private StringBuilder logBuilder = null;
        private void WriteLine(string msg)
        {
            _context.Response.Write(msg);
            _context.Response.Flush();
            if (logBuilder != null)
            {
                logBuilder.Append(msg + "\r\n");
            }

        }

        private void WriteLog(string msg)
        {
            WriteLine("正在推送：" + msg + "。。。");
        }


        private static Regex curveRegex = new Regex(@"[^\s]+", RegexOptions.Compiled);

        private decimal? FieldConvert(string str)
        {
            decimal result;
            if (decimal.TryParse(str, out result))
                return result;
            return null;
        }

        public void ProcessRequest(HttpContext context)
        {
            _context = context;
            if (isRunning)
            {
                WriteLine("一个推送任务正在进行！");
                return;
            }
            isRunning = true;
            DbTransaction tran = null;
            try
            {
                //context.Response.ContentType = "text/plain";
                string cd = context.Request.Form["cd"];
                string well_name = context.Request.Form["well_name"];
                if (!new UserService().GetActiveUserRoles().Contains(ServiceEnums.UserRole.调度管理员))
                {
                    WriteLine("没有权限！");
                    return;
                }

                logBuilder = new StringBuilder();
                logBuilder.Append("-".PadLeft(20, '-') + "\r\n");
                logBuilder.Append(ServiceUtils.GetUserInfo().COL_LOGINNAME + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "\r\n");

                WriteLine("开始推送：" + cd);
                using (EntitiesA12 a12 = new EntitiesA12())
                {
                    using (EntitiesLogging cj = new EntitiesLogging())
                    {
                        var well = a12.COM_BASE_WELL.Where(x => x.WELL_NAME == well_name).FirstOrDefault();
                        if (well == null)
                        {
                            WriteLine("A12数据库不存在井：" + well_name + " ！");
                            return;
                        }
                        if (a12.LOG_OPS_JOB_ACTIVITY.Any(x => x.REQUISITION_ID == cd))
                        {
                            WriteLine("A12数据库已存在通知单：" + cd + " ！");
                            return;
                        }
                        var task = cj.DM_LOG_TASK.Where(x => x.REQUISITION_CD == cd).FirstOrDefault();
                        if (task == null)
                        {
                            WriteLine("测井数据库不存在通知单：" + cd + " ！");
                            return;
                        }
                        a12.Connection.Open();
                        tran = a12.Connection.BeginTransaction();
                        string job_id = new Sequence().NextValue;
                        WriteLog("测井作业活动");

                        var aning = cj.DM_LOG_WORK_ANING.Where(x => x.REQUISITION_CD == cd).FirstOrDefault();
                        if (aning != null)
                        {
                            a12.AddToLOG_OPS_JOB_ACTIVITY(
                                new LOG_OPS_JOB_ACTIVITY
                                {
                                    WELL_ID = well.WELL_ID,
                                    JOB_ID = job_id,
                                    REQUISITION_ID = cd,
                                    ARRIVE_TIME = aning.ARRIVE_TIME,
                                    HANDINTIME = aning.RECEIVING_TIME,
                                    HANDOVERTIME = aning.HAND_TIME,
                                    LEAVE_WELL_TIME = aning.LEAVE_TIME,
                                    WORK_SECTION = task.PREDICTED_LOGGING_INTERVAL
                                });
                            a12.SaveChanges();
                        }

                        WriteLog("施工设计");
                        var plan = cj.DM_LOG_OPS_PLAN.Where(x => x.REQUISITION_CD == cd).ToArray();
                        if (aning == null)
                            aning = new DM_LOG_WORK_ANING
                            {
                                REQUISITION_CD = plan[0].REQUISITION_CD,
                                JOB_PLAN_CD = plan[0].JOB_PLAN_CD
                            };
                        var seq = new Sequence();
                        for (int i = 0; i < plan.Length; i++)
                        {
                            decimal? top = null, bottom = null;
                            if (!string.IsNullOrEmpty(plan[i].PRELOGGING_INTERVAL))
                            {
                                var groups = new Regex(@"(\d+\.?\d*)-(\d+\.?\d*)").Match(plan[i].PRELOGGING_INTERVAL).Groups;
                                if (!string.IsNullOrEmpty(groups[1].Value))
                                    top = Convert.ToDecimal(groups[1].Value);
                                if (!string.IsNullOrEmpty(groups[2].Value))
                                    bottom = Convert.ToDecimal(groups[2].Value);
                            }
                            a12.AddToLOG_DESIGN_WORKPLAN(
                                new LOG_DESIGN_WORKPLAN
                                {
                                    WELL_ID = well.WELL_ID,
                                    DESIGN_ID = seq.NextValue,
                                    JOB_TYPE = plan[i].LOG_TYPE,
                                    DESIGN_PERSON = plan[i].PREPARE_PERSON,
                                    AUDITOR = plan[i].VERIFIER,
                                    TOP = top,
                                    BOTTOM = bottom
                                });
                        }
                        a12.SaveChanges();

                        WriteLog("测井曲线对象");
                        var detail = cj.DM_LOG_WORK_DETAILS.Where(x => x.JOB_PLAN_CD == aning.JOB_PLAN_CD).FirstOrDefault();
                        if (detail != null)
                        {
                            a12.AddToLOG_ACH_CARVE_OBJECT(
                                new LOG_ACH_CARVE_OBJECT
                                {
                                    WELL_ID = well.WELL_ID,
                                    CURVE_CODE_ORI = new Sequence().NextValue,
                                    JOB_ID = job_id,
                                    LOG_DATE = aning.RECEIVING_TIME,
                                    TOP_DEPTH = detail.MEASURE_WELL_TO,
                                    BOTTOM_DEPTH = detail.MEASURE_WELL_FROM
                                    //CURVE_NAME_ORI
                                });
                            a12.SaveChanges();
                        }
                        //WriteLog("原始数据文件描述");
                        //a12.AddToLOG_OPS_ACQUISITION_DATA(
                        //    new LOG_OPS_ACQUISITION_DATA 
                        //    { 
                        //       ACQUISITION_DATA_ID=DateTime.Now.Ticks.ToString(),
                        //    });
                        WriteLog("测井施工基础信息");
                        var wellbore_basic = (from a in cj.DM_LOG_OPS_PLAN
                                              from b in cj.COM_WELLBORE_BASIC
                                              where a.REQUISITION_CD == cd && a.DM_LOG_TASK.COM_JOB_INFO.WELL_ID == b.WELL_ID
                                              select b).FirstOrDefault();
                        var well_basic = (from a in cj.DM_LOG_OPS_PLAN
                                          from b in cj.COM_WELL_BASIC
                                          where a.REQUISITION_CD == cd && a.DM_LOG_TASK.COM_JOB_INFO.WELL_ID == b.WELL_ID
                                          select b).FirstOrDefault();
                        var produce = (from a in cj.PRO_LOG_PRODUCE
                                       where a.REQUISITION_CD == cd
                                       select a).FirstOrDefault();
                        var log_base = (from a in cj.DM_LOG_BASE
                                        where a.DM_LOG_OPS_PLAN.REQUISITION_CD == cd
                                        select a).FirstOrDefault();
                        var job_info = (from a in cj.DM_LOG_TASK
                                        where a.REQUISITION_CD == cd
                                        select a.COM_JOB_INFO).FirstOrDefault();
                        var work_personnel = (from a in cj.DM_LOG_WORK_PERSONNEL
                                              where a.DM_LOG_OPS_PLAN.REQUISITION_CD == cd
                                              select a).FirstOrDefault();
                        {
                            DateTime? _BEGIN_DATE = null, _REQUEST_TIME_ARR = null;
                            if (plan.Length > 0)
                            {
                                _BEGIN_DATE = plan[0].RECEIVED_INFORM_TIME;
                                _REQUEST_TIME_ARR = plan[0].REQUIREMENTS_TIME;
                            }
                            if (wellbore_basic == null)
                                wellbore_basic = new COM_WELLBORE_BASIC();
                            if (produce == null)
                                produce = new PRO_LOG_PRODUCE();
                            if (detail == null)
                                detail = new DM_LOG_WORK_DETAILS();
                            a12.AddToLOG_OPS_BASEINFO_RECORD(
                                new LOG_OPS_BASEINFO_RECORD
                                {
                                    WELL_ID = well.WELL_ID,
                                    OPS_BASEINFO_ID = new Sequence().NextValue,
                                    JOB_ID = job_id,
                                    INCLNATION = wellbore_basic.MAX_WELL_DEVIATION,
                                    INCLNATION_DEPTH = wellbore_basic.MAX_WELL_DEVIATION_MD,
                                    WELLBOTTOM_PRESS = produce.WELLBOTTOM_PRESS,
                                    SUL_HYD = produce.SUL_HYD,
                                    WELLBOTTOM_TMP = log_base.BOTTOM_TEMPERATURE,
                                    WELLHEAD_PRESS = produce.WELLHEAD_PRESS,
                                    BEGIN_DATE = _BEGIN_DATE,
                                    WELL_NAME = well_basic.WELL_NAME,
                                    WELL_SORT = job_info.WELL_SORT,
                                    WELL_TYPE = job_info.WELL_TYPE,
                                    DEEP_DESIGN = wellbore_basic.DESIGN_MD,
                                    MEASURE_TOP = detail.MEASURE_WELL_TO,
                                    MEASURE_BOTTOM = detail.MEASURE_WELL_FROM,
                                    REQUEST_TIME_ARR = _REQUEST_TIME_ARR,
                                    LOG_TEAM_MASTER = work_personnel.LOG_TEAM_LEADER,
                                    DEPART_TIME = aning.RECEIVED_INFORM_TIME,
                                    LOG_BEGIN_TIME = aning.LOG_START_TIME,
                                    ARRIVE_TIME = aning.REQUIREMENTS_TIME,
                                    LOG_FINISH_TIME = aning.LOG_END_TIME
                                });
                        }
                        a12.SaveChanges();

                        WriteLog("采集过程、井场提交资料清单、井下仪器运行记录、实际作业项目明细");
                        var details = cj.DM_LOG_WORK_DETAILS.Where(x => x.JOB_PLAN_CD == aning.JOB_PLAN_CD).ToArray();
                        seq = new Sequence();
                        var seq1 = new Sequence();
                        var seq2 = new Sequence();
                        var seq3 = new Sequence();
                        for (int i = 0; i < details.Length; i++)
                        {
                            string _COLLECT_PROCESS_ID = seq3.NextValue;
                            a12.AddToLOG_OPS_COLLECT_PROCESS(
                                new LOG_OPS_COLLECT_PROCESS
                                {
                                    WELL_ID = well.WELL_ID,
                                    JOB_ID = job_id,
                                    COLLECT_PROCESS_ID = _COLLECT_PROCESS_ID,
                                    DOWN_WELL_SEQUENCE = details[i].DOWN_WELL_SEQUENCE,
                                    START_TIME = details[i].START_TIME,
                                    END_TIME = details[i].END_TIME,
                                    TOP = details[i].MEASURE_WELL_TO,
                                    BOTTOM = details[i].MEASURE_WELL_FROM,
                                    WELL_SECTION = details[i].WELL_SECTION,
                                    IS_SUCCESS = details[i].IF_SUCCESS,
                                    IS_SUPPLEMENT = details[i].IS_ADD
                                });
                            string filename = details[i].FILENAME;
                            if (filename.Length > 255)
                                filename = filename.Substring(0, 255);

                            a12.AddToLOG_OPS_DATA_SUBMIT(
                                new LOG_OPS_DATA_SUBMIT
                                {
                                    WELL_ID = well.WELL_ID,
                                    DATA_SUMBIT_ID = seq.NextValue,
                                    JOB_ID = job_id,
                                    COLLECT_PROCESS_ID = _COLLECT_PROCESS_ID,
                                    COLLECT_ID = details[i].DOWN_WELL_SEQUENCE.ToString(),
                                    FILE_NAME = filename
                                });
                            a12.AddToLOG_OPS_INS_RUNNINGRECORD(
                                new LOG_OPS_INS_RUNNINGRECORD
                                {
                                    WELL_ID = well.WELL_ID,
                                    INSTRUMENT_ID = seq1.NextValue,
                                    JOB_ID = job_id,
                                    COLLECT_PROCESS_ID = _COLLECT_PROCESS_ID,
                                    RUN_TIME = details[i].WORK_HOURS
                                });
                            a12.AddToLOG_OPS_TASK_ITEMS(
                                new LOG_OPS_TASK_ITEMS
                                {
                                    WELL_ID = well.WELL_ID,
                                    TASK_OPS_ID = seq2.NextValue,
                                    COLLECT_PROCESS_ID = _COLLECT_PROCESS_ID,
                                    IS_ADDITIONAL = details[i].IS_ADD,
                                    IS_SUCESS = details[i].IF_SUCCESS
                                });
                        }
                        a12.SaveChanges();

                        WriteLog("故障记录、遇阻遇卡");
                        var holdup_details = cj.DM_LOG_WORK_HOLDUP_DETAILS.Where(x => x.DM_LOG_OPS_PLAN.REQUISITION_CD == cd).ToArray();
                        seq = new Sequence();
                        seq1 = new Sequence();
                        for (int i = 0; i < holdup_details.Length; i++)
                        {
                            a12.AddToLOG_OPS_FAILURE_RECORD(
                                new LOG_OPS_FAILURE_RECORD
                                {
                                    WELL_ID = well.WELL_ID,
                                    JOB_ID = job_id,
                                    FAILURE_ID = seq.NextValue,
                                    FAILURE_TIME = holdup_details[i].HOLDUP_DATE,
                                    //DEEP_FAILURE_REC=holdup_details[i].HOLDUP_MD,
                                    FAILURE_DESC = holdup_details[i].OBSTRUCTION_DESC
                                });
                            a12.AddToLOG_OPS_TIGHT_HOLE_WARNING(
                                new LOG_OPS_TIGHT_HOLE_WARNING
                                {
                                    WELL_ID = well.WELL_ID,
                                    JOB_ID = job_id,
                                    TIGHT_HOLE_ID = seq1.NextValue,
                                    COLLECT_PROCESS_ID = holdup_details[i].DOWN_WELL_SEQUENCE.ToString(),
                                    DANGEROUS_EVENT_SOURCE = holdup_details[i].HOLDUP_TYPE,
                                    DANGEROUS_EVENT_DATE_TIME = holdup_details[i].HOLDUP_DATE,
                                    //DANGEROUS_EVENT_DEEP=holdup_details[i].HOLDUP_MD,
                                    DANGEROUS_EVNET_DESC = holdup_details[i].OBSTRUCTION_DESC
                                });
                        }
                        a12.SaveChanges();

                        WriteLog("测井监督记录");
                        var punish = cj.PRO_LOG_PUNISH.Where(x => x.DM_LOG_OPS_PLAN.REQUISITION_CD == cd).ToArray();
                        seq = new Sequence();
                        for (int i = 0; i < punish.Length; i++)
                        {
                            a12.AddToLOG_OPS_OPERATION_MONITOR(
                                new LOG_OPS_OPERATION_MONITOR
                                {
                                    WELL_ID = well.WELL_ID,
                                    OPERATION_MONITOR_ID = seq.NextValue,
                                    //WELL_LOGGING_PROJECT=punish[i].PUNISH_DATE,
                                    REASON = punish[i].REASON,
                                    PUNISH_SUGGESTION = punish[i].PUNISH_SUGGESTION,
                                    RECTIFY_DESC = punish[i].IS_RECTIFY
                                });
                        }
                        a12.SaveChanges();

                        WriteLog("放射源施工记录");
                        var radiation_status = cj.DM_LOG_RADIATION_STATUS.Where(x => x.DM_LOG_OPS_PLAN.REQUISITION_CD == cd).ToArray();
                        seq = new Sequence();
                        for (int i = 0; i < radiation_status.Length; i++)
                        {
                            a12.AddToLOG_OPS_RADIATION_RECORD(
                                new LOG_OPS_RADIATION_RECORD
                                {
                                    WELL_ID = well.WELL_ID,
                                    JOB_ID = job_id,
                                    RADIATION_RECORD_ID = seq.NextValue,
                                    DOWN_WELL_TIME = radiation_status[i].DOWN_WELL_SEQUENCE,
                                    RADIATION_NO = radiation_status[i].RADIATION_NO,
                                    LOAD_PERSON = radiation_status[i].LOAD_PERSON,
                                    UNLOAD_PERSON = radiation_status[i].UNLOAD_PERSON,
                                    UNDER_WELL_TIME = radiation_status[i].UNDER_WELL_TIME
                                });
                        }

                        string cj_process_id = cj.DM_LOG_SOURCE_DATA.Where(x => x.DM_LOG_OPS_PLAN.REQUISITION_CD == cd).FirstOrDefault().LOG_DATA_ID;
                        if (!string.IsNullOrEmpty(cj_process_id))
                        {
                            string a12_process_id = new Sequence().NextValue;
                            WriteLog("解释平台");
                            var data_publish = cj.PRO_LOG_DATA_PUBLISH.Where(x => x.PROCESS_ID == cj_process_id).FirstOrDefault();
                            a12.AddToLOG_OPS_INTERPRETATIONPLATFORM(new LOG_OPS_INTERPRETATIONPLATFORM
                            {
                                WELL_ID = well.WELL_ID,
                                PLATFORM_ID = new Sequence().NextValue,
                                PROCESS_ID = a12_process_id,
                                PLATFORM_NAME = data_publish.P_PROCESS_SOFTWARE
                            });
                            a12.SaveChanges();

                            WriteLog("解释过程");
                            var processing_item = cj.PRO_LOG_PROCESSING_ITEM.Where(x => x.PROCESS_ID == cj_process_id).FirstOrDefault();
                            //for (int i = 0; i < processing_item.Length; i++)
                            //{
                            if (processing_item != null)
                                a12.AddToLOG_OPS_INTERPRET_PROCESS(
                                    new LOG_OPS_INTERPRET_PROCESS
                                    {
                                        WELL_ID = well.WELL_ID,
                                        JOB_ID = job_id,
                                        PROCESS_ID = a12_process_id,
                                        INTERPRETOR = processing_item.PROCESSOR,
                                        CHECKER = processing_item.INTERPRETER,
                                        AUDITPERSOON = processing_item.P_SUPERVISOR,
                                        PROCESS_START_TIME = processing_item.P_START_DATE
                                    });
                            // }
                            a12.SaveChanges();

                            WriteLog("声幅资料、固井质量解释结论");
                            var cement_evaluation_inf = cj.COM_LOG_CEMENT_EVALUATION_INF.Where(x => x.PROCESS_ID == cj_process_id).ToArray();
                            seq = new Sequence();
                            seq1 = new Sequence();
                            for (int i = 0; i < cement_evaluation_inf.Length; i++)
                            {
                                a12.AddToLOG_ACH_AMPLITUDE_DATA(
                                    new LOG_ACH_AMPLITUDE_DATA
                                    {
                                        WELL_ID = well.WELL_ID,
                                        AMPLITUDE_ID = seq.NextValue,
                                        JOB_ID = job_id,
                                        TOP_DEPTH = cement_evaluation_inf[i].ST_DEP,
                                        BOTTOM_DEPTH = cement_evaluation_inf[i].EN_DEP,
                                        VALUE = cement_evaluation_inf[i].MEA_CBL
                                        //QUALITY=cement_evaluation_inf[i].RESULT
                                    });
                                a12.AddToLOG_ACH_CEMENT_QUALITY_REPORT(
                                    new LOG_ACH_CEMENT_QUALITY_REPORT
                                    {
                                        WELL_ID = well.WELL_ID,
                                        VDL_ID = seq1.NextValue,
                                        PROCESS_ID = a12_process_id,
                                        LAY_ID = i + 1,
                                        JOB_ID = job_id,
                                        START_DEPTH = cement_evaluation_inf[i].ST_DEP,
                                        END_DEPTH = cement_evaluation_inf[i].EN_DEP,
                                        CEMENTING_QUALITY = cement_evaluation_inf[i].RESULT
                                    });
                            }
                            string requisition_type = null;
                            if (task.REQUISITION_TYPE != null)
                            {
                                string id = task.REQUISITION_TYPE.Value.ToString();
                                requisition_type = cj.PKL_LOG_REQUISITION_TYPE.Where(x => x.REQUISITION_TYPE_ID == id).FirstOrDefault().REQUISITION_TYPE_NAME;
                            }
                            a12.SaveChanges();

                            if ("工程测井,裸眼测井,生产测井".Split(',').Contains(requisition_type))
                                requisition_type = requisition_type + "解释结论、";
                            else
                                requisition_type = string.Empty;
                            WriteLog(requisition_type + "解释符合率");
                            var result = cj.COM_LOG_RESULT.Where(x => x.PROCESS_ID == cj_process_id).ToArray();
                            seq = new Sequence();
                            for (int i = 0; i < result.Length; i++)
                            {
                                if (requisition_type == "工程测井解释结论、")
                                    a12.AddToLOG_ACH_ENGINEERING_LOG_REPORT(
                                        new LOG_ACH_ENGINEERING_LOG_REPORT
                                        {
                                            WELL_ID = well.WELL_ID,
                                            JOB_ID = job_id,
                                            LAY_ID = result[i].RESULTID.ToString(),
                                            PROCESS_ID = a12_process_id,
                                            FORMATION = result[i].FORMATION_NAME,
                                            START_DEPTH = result[i].START_DEPTH,
                                            END_DEPTH = result[i].END_DEPTH,
                                            VALID_THICKNESS = result[i].VALID_THICKNESS
                                        });

                                if (requisition_type == "裸眼测井解释结论、")
                                {
                                    string EXPLAIN_CONCLUSION = result[i].EXPLAIN_CONCLUSION;
                                    if (EXPLAIN_CONCLUSION.Length > 6)
                                        EXPLAIN_CONCLUSION = EXPLAIN_CONCLUSION.Substring(0, 6);
                                    a12.AddToWL_ACH_BOREHOLE_INTER(
                                        new WL_ACH_BOREHOLE_INTER
                                        {
                                            WELL_ID = well.WELL_ID,
                                            INTERP_BOREHOLE_ID = seq.NextValue,
                                            PROCESS_ID = a12_process_id,
                                            ZONE_ID = result[i].FORMATION_NAME,
                                            ZONE_NO = result[i].LAY_ID,
                                            //FORMATION = result[i].FORMATION_NAME,
                                            START_DEPTH = result[i].START_DEPTH,
                                            END_DEPTH = result[i].END_DEPTH,
                                            THICKNESS = result[i].VALID_THICKNESS,
                                            //MAX_SP_GAMMA_RADIANT_INTENSITY=result[i].GR_MAX_VALUE,
                                            //MIN_SP_GAMMA_RADIANT_INTENSITY=result[i].GR_MIN_VALUE,
                                            MAX_SONIC_DIFFERENTIAL_TIME = result[i].SOUNDWAVE_MAX_VALUE,
                                            MIN_SONIC_DIFFERENTIAL_TIME = result[i].SOUNDWAVE_MIN_VALUE,
                                            MAX_NEUTRON_POROSITY = result[i].NEUTRON_MAX_VALUE,
                                            MIN_NEUTRON_POROSITY = result[i].NEUTRON_MIN_VALUE,
                                            MAX_DENSITY_POROSITY = result[i].DENSITY_MAX_VALUE,
                                            MIN_DENSITY_POROSITY = result[i].DENSITY_MIN_VALUE,
                                            MAX_SONIC_POROSITY = result[i].GR_MAX_VALUE,
                                            MIN_SONIC_POROSITY = result[i].GR_MIN_VALUE,
                                            INTERPRETATION_RESULT = EXPLAIN_CONCLUSION
                                            //VALID_THICKNESS = result[i].VALID_THICKNESS,
                                            //PENETRATE_RATE = result[i].PENETRATE_RATE,
                                            //INTERPRET_RESULT = result[i].EXPLAIN_CONCLUSION
                                        });
                                }

                                a12.AddToLOG_ACH_INTERPRET_VALID_RATE(
                                    new LOG_ACH_INTERPRET_VALID_RATE
                                    {
                                        WELL_ID = well.WELL_ID,
                                        LAY_ID = result[i].RESULTID.ToString(),
                                        PROCESS_ID = a12_process_id,
                                        FORMATION = result[i].FORMATION_NAME,
                                        START_DEPTH = result[i].START_DEPTH,
                                        END_DEPTH = result[i].END_DEPTH,
                                        VALID_THICKNESS = result[i].VALID_THICKNESS,
                                        INTERPRET_RESULT = result[i].EXPLAIN_CONCLUSION
                                    });

                                if (requisition_type == "生产测井解释结论、")
                                    a12.AddToLOG_ACH_PRODUCTION_LOG_REPORT(
                                        new LOG_ACH_PRODUCTION_LOG_REPORT
                                        {
                                            WELL_ID = well.WELL_ID,
                                            JOB_ID = job_id,
                                            LAY_ID = result[i].RESULTID.ToString(),
                                            PROCESS_ID = a12_process_id,
                                            FORMATION = result[i].FORMATION_NAME,
                                            START_DEPTH = result[i].START_DEPTH,
                                            END_DEPTH = result[i].END_DEPTH,
                                            VALID_THICKNESS = result[i].VALID_THICKNESS,
                                            INTERPRET_RESULT = result[i].EXPLAIN_CONCLUSION
                                        });
                            }
                            a12.SaveChanges();

                            WriteLog("井径资料");
                            var cache = cj.COM_LOG_COM_CURVEDATA_CACHE.Where(x => x.PROCESS_ID == cj_process_id).Select(x => x).FirstOrDefault();
                            if (cache != null)
                            {
                                var curveDataComboBoxsSetting = Logging_App.Utility.ModelHelper.DeserializeObject(cache.COMBOBOXS_SETTING) as Dictionary<string, int>;
                                if (curveDataComboBoxsSetting != null && curveDataComboBoxsSetting.ContainsKey("StartLine"))
                                {
                                    var depIndex = curveDataComboBoxsSetting.First(x => x.Key.StartsWith("DEP|")).Value;
                                    var calIndex = curveDataComboBoxsSetting.First(x => x.Key.StartsWith("CAL|")).Value;
                                    var startLine = curveDataComboBoxsSetting["StartLine"];
                                    if (depIndex > 0 && startLine > 0)
                                    {
                                        var uploadInfo = cj.SYS_UPLOAD.Where(x => x.UPLOADID == cache.UPLOADID).Select(x => x).FirstOrDefault();
                                        if (uploadInfo != null)
                                        {
                                            var path = FileUpload.PathList.FirstOrDefault(x => x.id == uploadInfo.PATHID).name;
                                            path = path.Remove(path.LastIndexOf('\\') + 1);
                                            //path += uploadInfo.PATHMAIN;

                                            string fullname = string.Format("{0}\\{1}-{2}-{3}",
                                                path,
                                                uploadInfo.SHA1,
                                                uploadInfo.MD5,
                                                uploadInfo.LENGTH);
                                            if (File.Exists(fullname))
                                            {
                                                using (var reader = new StreamReader(fullname, Encoding.Default, true))
                                                {
                                                    string linetxt;
                                                    MatchCollection matches;
                                                    int line = 1;
                                                    decimal? depValue;
                                                    decimal? calValue;
                                                    seq = new Sequence();
                                                    while ((linetxt = reader.ReadLine()) != null)
                                                    {
                                                        if (line >= startLine)
                                                        {
                                                            matches = curveRegex.Matches(linetxt);
                                                            if (depIndex <= matches.Count)
                                                            {
                                                                depValue = FieldConvert(matches[depIndex - 1].Value);
                                                                calValue = null;
                                                                if (calIndex > 0 && calIndex <= matches.Count)
                                                                {
                                                                    calValue = FieldConvert(matches[calIndex - 1].Value);
                                                                }
                                                                a12.AddToLOG_ACH_HOLE_DIAMETER_DATA(
                                                                    new LOG_ACH_HOLE_DIAMETER_DATA
                                                                    {
                                                                        WELL_ID = well.WELL_ID,
                                                                        DATA_ID = seq.NextValue,
                                                                        JOB_ID = job_id,
                                                                        DEPTH = depValue,
                                                                        WELL_DIAMETER = calValue
                                                                    });
                                                            }
                                                        }
                                                        if (line % 1000 == 0)
                                                        {
                                                            context.Response.Write('1');
                                                            context.Response.Flush();
                                                            a12.SaveChanges();
                                                        }
                                                        line += 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //var curvedata = cj.COM_LOG_COM_CURVEDATA.OrderBy(x => x.CURVEDATAID).Take(100).ToArray();
                            //seq = new Sequence();
                            //for (int i = 0; i < curvedata.Length; i++)
                            //{
                            //    a12.AddToLOG_ACH_HOLE_DIAMETER_DATA(
                            //        new LOG_ACH_HOLE_DIAMETER_DATA
                            //        {
                            //            WELL_ID = well.WELL_ID,
                            //            DATA_ID = seq.NextValue,
                            //            JOB_ID = job_id,
                            //            DEPTH = curvedata[i].DEP,
                            //            WELL_DIAMETER = curvedata[i].CAL
                            //        });
                            //}
                            a12.SaveChanges();

                            WriteLog("井斜资料");
                            var trajectory = cj.COM_LOG_WELL_TRAJECTORY.Where(x => x.PROCESS_ID == cj_process_id).ToArray();
                            seq = new Sequence();
                            for (int i = 0; i < trajectory.Length; i++)
                            {
                                a12.AddToLOG_ACH_WELL_DEVIATION_DATA(
                                    new LOG_ACH_WELL_DEVIATION_DATA
                                    {
                                        WELL_ID = well.WELL_ID,
                                        DEV_DATA_ID = seq.NextValue,
                                        JOB_ID = job_id,
                                        MUDLOGGING_DEPTH = trajectory[i].MD.Value,
                                        UPRIGHTNESS_DEPTH = trajectory[i].TVD.Value,
                                        INCLNATION = trajectory[i].INCLNATION,
                                        AZIMUTH = trajectory[i].AZIMUTH,
                                        E_ELEMENT = trajectory[i].E_ELEMENT,
                                        N_ELEMENT = trajectory[i].N_ELEMENT,
                                        DOG_LEG = trajectory[i].DOG_LEG
                                    });
                            }
                            a12.SaveChanges();

                            WriteLog("曲线质量评定");
                            var curveRating = (from a in cj.PRO_LOG_PROCESSING_CURVERATING
                                               where a.PROCESS_ID == cj_process_id
                                               select new
                                               {
                                                   CURVE_NAME = a.PKL_LOG_OPS_CURVE.CURVE_NAME,
                                                   a.START_DEP,
                                                   a.END_DEP,
                                                   a.SCENE_RATING,
                                                   a.INDOOR_RATING
                                               }).ToArray();
                            seq = new Sequence();
                            for (int i = 0; i < curveRating.Length; i++)
                            {
                                a12.AddToLOG_OPS_CARVE_QUALITY(
                                    new LOG_OPS_CARVE_QUALITY
                                    {
                                        WELL_ID = well.WELL_ID,
                                        CURVE_QUALITY_ID = seq.NextValue,
                                        JOB_ID = job_id,
                                        PROCESS_ID = cj_process_id,
                                        //CURVE_NO = cj_process_id + i,
                                        CURVE_NAME = curveRating[i].CURVE_NAME,
                                        TOP_DEPTH = curveRating[i].START_DEP,
                                        BOTTOM_DEPTH = curveRating[i].END_DEP,
                                        CARVE_QUALITY_ONE = curveRating[i].SCENE_RATING,
                                        CARVE_QUALITY_TWO = curveRating[i].INDOOR_RATING,
                                    });
                            }
                        }
                        a12.SaveChanges();
                        tran.Commit();
                        //tran.Rollback();
                        tran.Dispose();
                    }
                }
                WriteLine("数据推送完成！");
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    try
                    {
                        tran.Rollback();
                    }
                    catch (Exception e) { }
                }
                WriteLine("推送数据出错：" + ex.Message);
                var inEx = ex.InnerException;
                while (inEx != null)
                {
                    logBuilder.Append(ex.InnerException.Message + "\r\n");
                    inEx = inEx.InnerException;
                }
            }
            finally
            {
                try
                {
                    File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "LogFile//DataPushLog//" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", logBuilder.ToString());
                }
                catch (Exception e) { }
                isRunning = false;
            }
        }


        private class Sequence
        {
            private long counter = 0;
            private long baseValue = DateTime.Now.Ticks / 100000 * 100000;
            public string NextValue
            {
                get
                {
                    counter += 1;
                    return (baseValue + counter).ToString();
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}