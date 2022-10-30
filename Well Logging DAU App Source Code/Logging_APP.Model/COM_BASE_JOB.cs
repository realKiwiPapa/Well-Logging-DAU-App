using System;
using System.Text;
using System.Collections.Generic;

namespace Logging_App.Model
{
    //针对同一口井，由于井名、井型、井别等发生变化，
    [Serializable]
    public class COM_BASE_JOB : ModelBase
    {

        /// <summary>
        /// 井ID
        /// </summary>		
        private string _well_id;
        public string WELL_ID
        {
            get { return _well_id; }
            set { _well_id = value; }
        }
        /// <summary>
        /// 作业项目ID
        /// </summary>
        [Required]
        private string _job_id;
        public string JOB_ID
        {
            get { return _job_id; }
            set { _job_id = value; }
        }
        /// <summary>
        /// 作业井名
        /// </summary>		
        private string _well_job_name;
        public string WELL_JOB_NAME
        {
            get { return _well_job_name; }
            set { _well_job_name = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>		
        private string _project_name;
        public string PROJECT_NAME
        {
            get { return _project_name; }
            set { _project_name = value; }
        }
        /// <summary>
        /// 业主单位
        /// </summary>		
        private string _owner_org_id;
        public string OWNER_ORG_ID
        {
            get { return _owner_org_id; }
            set { _owner_org_id = value; }
        }
        /// <summary>
        /// 井所属单位
        /// </summary>		
        private string _well_the_market;
        public string WELL_THE_MARKET
        {
            get { return _well_the_market; }
            set { _well_the_market = value; }
        }
        /// <summary>
        /// 井型
        /// </summary>		
        private string _well_type;
        public string WELL_TYPE
        {
            get { return _well_type; }
            set { _well_type = value; }
        }
        /// <summary>
        /// 井别
        /// </summary>		
        private string _well_sort;
        public string WELL_SORT
        {
            get { return _well_sort; }
            set { _well_sort = value; }
        }
        /// <summary>
        /// 设计完钻层位
        /// </summary>		
        private string _design_finish_formation;
        public string DESIGN_FINISH_FORMATION
        {
            get { return _design_finish_formation; }
            set { _design_finish_formation = value; }
        }
        /// <summary>
        /// 设计完井方法
        /// </summary>		
        private string _design_complete_method;
        public string DESIGN_COMPLETE_METHOD
        {
            get { return _design_complete_method; }
            set { _design_complete_method = value; }
        }
        /// <summary>
        /// 作业目的
        /// </summary>		
        private string _job_purpose;
        public string JOB_PURPOSE
        {
            get { return _job_purpose; }
            set { _job_purpose = value; }
        }
        /// <summary>
        /// 行状态
        /// </summary>		
        private string _row_state;
        public string ROW_STATE
        {
            get { return _row_state; }
            set { _row_state = value; }
        }
        /// <summary>
        /// 创建者所在单位
        /// </summary>		
        private string _create_org;
        public string CREATE_ORG
        {
            get { return _create_org; }
            set { _create_org = value; }
        }
        /// <summary>
        /// 创建者
        /// </summary>		
        private string _create_user;
        public string CREATE_USER
        {
            get { return _create_user; }
            set { _create_user = value; }
        }
        /// <summary>
        /// 修改者所在单位
        /// </summary>		
        private string _update_org;
        public string UPDATE_ORG
        {
            get { return _update_org; }
            set { _update_org = value; }
        }
        /// <summary>
        /// 修改者
        /// </summary>		
        private string _update_user;
        public string UPDATE_USER
        {
            get { return _update_user; }
            set { _update_user = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        private DateTime _create_date;
        public DateTime CREATE_DATE
        {
            get { return _create_date; }
            set { _create_date = value; }
        }
        /// <summary>
        /// 修改时间
        /// </summary>		
        private DateTime _update_date;
        public DateTime UPDATE_DATE
        {
            get { return _update_date; }
            set { _update_date = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>		
        private string _remark;
        public string REMARK
        {
            get { return _remark; }
            set { _remark = value; }
        }

    }
}

