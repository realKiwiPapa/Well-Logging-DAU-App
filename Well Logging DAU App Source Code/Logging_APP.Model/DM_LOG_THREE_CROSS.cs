using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    /// <summary>
    /// 小队出发前，领导的交代记录
    /// </summary>
    [Serializable]
    public class DM_LOG_THREE_CROSS : ModelBase
    {
        #region Model
        private string _three_cross_id;
        private string _job_plan_cd;
        private DateTime? _meeting_date;
        private decimal? _meeting_time;
        private string _host;
        private string _writer;
        private string _participants;
        private string _summary_item1;
        private string _summary_item2;
        private string _summary_item3;
        private string _summary_item6;
        private string _summary_item4;
        private string _summary_item5;
        private string _note;
        private string _attach_info;
        private byte[] _attach_file;

        
        /// <summary>
        /// 三交会ID
        /// </summary>
        [Required]
        public string THREE_CROSS_ID
        {
            set { _three_cross_id = value; }
            get { return _three_cross_id.TrimCharEnd(); }
        }
        /// <summary>
        /// 作业计划书编号
        /// </summary>
        public string JOB_PLAN_CD
        {
            set { _job_plan_cd = value; }
            get { return _job_plan_cd; }
        }
        /// <summary>
        /// 会议日期
        /// </summary>
        public DateTime? MEETING_DATE
        {
            set { _meeting_date = value; }
            get { return _meeting_date; }
        }
        /// <summary>
        /// 会议时间
        /// </summary>
        public decimal? MEETING_TIME
        {
            set { _meeting_time = value; }
            get { return _meeting_time; }
        }
        /// <summary>
        /// 测井公司领导
        /// </summary>
        public string HOST
        {
            set { _host = value; }
            get { return _host.TrimCharEnd(); }
        }
        /// <summary>
        /// 记录人
        /// </summary>
        public string WRITER
        {
            set { _writer = value; }
            get { return _writer.TrimCharEnd(); }
        }
        /// <summary>
        /// 参加会议人员
        /// </summary>
        public string PARTICIPANTS
        {
            set { _participants = value; }
            get { return _participants.TrimCharEnd(); }
        }
        /// <summary>
        /// 作业内容及特殊要求
        /// </summary>
        public string SUMMARY_ITEM1
        {
            set { _summary_item1 = value; }
            get { return _summary_item1.TrimCharEnd(); }
        }
        /// <summary>
        /// 行车路线、路况及安全要求
        /// </summary>
        public string SUMMARY_ITEM2
        {
            set { _summary_item2 = value; }
            get { return _summary_item2.TrimCharEnd(); }
        }
        /// <summary>
        /// 作业主要风险及控制措施
        /// </summary>
        public string SUMMARY_ITEM3
        {
            set { _summary_item3 = value; }
            get { return _summary_item3.TrimCharEnd(); }
        }
        /// <summary>
        /// 会议纪要
        /// </summary>
        public string SUMMARY_ITEM6
        {
            set { _summary_item6 = value; }
            get { return _summary_item6.TrimCharEnd(); }
        }
        /// <summary>
        /// 会议纪要
        /// </summary>
        public string SUMMARY_ITEM4
        {
            set { _summary_item4 = value; }
            get { return _summary_item4.TrimCharEnd(); }
        }
        /// <summary>
        /// 会议纪要
        /// </summary>
        public string SUMMARY_ITEM5
        {
            set { _summary_item5 = value; }
            get { return _summary_item5.TrimCharEnd(); }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string NOTE
        {
            set { _note = value; }
            get { return _note.TrimCharEnd(); }
        }

        /// <summary>
        /// 附件信息
        /// </summary>
        public string ATTACH_INFO
        {
            get { return _attach_info; }
            set { _attach_info = value; }
        }
        /// <summary>
        /// 附件
        /// </summary>
        public byte[] ATTACH_FILE
        {
            get { return _attach_file; }
            set { _attach_file = value; }
        }
        #endregion Model
    }
}