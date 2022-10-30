using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    //在井场，给小队人员交代安排和人员具体分工
    [Serializable]
    public class DM_LOG_CLASS_MEET : ModelBase
    {

        /// <summary>
        /// 井场班前会ID
        /// </summary>		
        private string _class_meet_id;
        [Required]
        public string CLASS_MEET_ID
        {
            get { return _class_meet_id.TrimCharEnd(); }
            set { _class_meet_id = value; }
        }
        /// <summary>
        /// 作业计划书编号
        /// </summary>		
        private string _job_plan_cd;
        public string JOB_PLAN_CD
        {
            get { return _job_plan_cd; }
            set { _job_plan_cd = value; }
        }
        /// <summary>
        /// 测井公司领导
        /// </summary>		
        private string _host;
        public string HOST
        {
            get { return _host.TrimCharEnd(); }
            set { _host = value; }
        }
        /// <summary>
        /// 记录人
        /// </summary>		
        private string _writer;
        public string WRITER
        {
            get { return _writer.TrimCharEnd(); }
            set { _writer = value; }
        }
        /// <summary>
        /// 会议日期
        /// </summary>		
        private DateTime _meeting_date;
        public DateTime MEETING_DATE
        {
            get { return _meeting_date; }
            set { _meeting_date = value; }
        }
        /// <summary>
        /// 会议时间
        /// </summary>		
        private decimal _meeting_time;
        public decimal MEETING_TIME
        {
            get { return _meeting_time; }
            set { _meeting_time = value; }
        }
        /// <summary>
        /// 参加会议人员
        /// </summary>		
        private string _participants;
        public string PARTICIPANTS
        {
            get { return _participants.TrimCharEnd(); }
            set { _participants = value; }
        }
        /// <summary>
        /// 现场属地安全及应急疏散提示
        /// </summary>		
        private string _summary_item1;
        public string SUMMARY_ITEM1
        {
            get { return _summary_item1.TrimCharEnd(); }
            set { _summary_item1 = value; }
        }
        /// <summary>
        /// 作业属地划分及人员分工
        /// </summary>		
        private string _summary_item2;
        public string SUMMARY_ITEM2
        {
            get { return _summary_item2.TrimCharEnd(); }
            set { _summary_item2 = value; }
        }
        /// <summary>
        /// 作业风险及技术安全措施
        /// </summary>		
        private string _summary_item3;
        public string SUMMARY_ITEM3
        {
            get { return _summary_item3.TrimCharEnd(); }
            set { _summary_item3 = value; }
        }
        /// <summary>
        /// 备用
        /// </summary>		
        private string _summary_item6;
        public string SUMMARY_ITEM6
        {
            get { return _summary_item6.TrimCharEnd(); }
            set { _summary_item6 = value; }
        }
        /// <summary>
        /// 备用
        /// </summary>		
        private string _summary_item4;
        public string SUMMARY_ITEM4
        {
            get { return _summary_item4.TrimCharEnd(); }
            set { _summary_item4 = value; }
        }
        /// <summary>
        /// 备用
        /// </summary>		
        private string _summary_item5;
        public string SUMMARY_ITEM5
        {
            get { return _summary_item5.TrimCharEnd(); }
            set { _summary_item5 = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>		
        private string _note;
        public string NOTE
        {
            get { return _note.TrimCharEnd(); }
            set { _note = value; }
        }
        /// <summary>
        /// 附件信息
        /// </summary>
        private string _attach_info;
        public string ATTACH_INFO
        {
            get { return _attach_info; }
            set { _attach_info = value; }
        }
        /// <summary>
        /// 附件
        /// </summary>
        private byte[] _attach_file;
        public byte[] ATTACH_FILE
        {
            get { return _attach_file; }
            set { _attach_file = value; }
        }
    }
}

