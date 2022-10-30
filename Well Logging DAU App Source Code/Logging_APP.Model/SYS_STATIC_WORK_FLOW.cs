using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App.Model
{
    [Serializable]
    public class SYS_STATIC_WORK_FLOW:ModelBase
    {
        /// <summary>
        /// 流程ID
        /// </summary>
        private decimal _flow_id;
        [Required]
        public decimal FLOW_ID
        {
            get { return _flow_id; }
            set { _flow_id = value; }
        }
        /// <summary>
        /// 流程名称
        /// </summary>
        private string _flow_name;

        public string FLOW_NAME
        {
            get { return _flow_name; }
            set { _flow_name = value; }
        }
        /// <summary>
        /// 流程节点数
        /// </summary>
        private decimal? _flow_node_num;

        public decimal? FLOW_NODE_NUM
        {
            get { return _flow_node_num; }
            set { _flow_node_num = value; }
        }
        /// <summary>
        /// 目标用户名
        /// </summary>
        private string _target_name;

        public string TARGET_NAME
        {
            get { return _target_name; }
            set { _target_name = value; }
        }
        /// <summary>
        /// 录入人
        /// </summary>
        private string _inputer;

        public string INPUTER
        {
            get { return _inputer; }
            set { _inputer = value; }
        }
        /// <summary>
        /// 录入ID
        /// </summary>
        private string _inputer_id;

        public string INPUTER_ID
        {
            get { return _inputer_id; }
            set { _inputer_id = value; }
        }
        /// <summary>
        /// 流程类型
        /// </summary>
        private string _flow_type;

        public string FLOW_TYPE
        {
            get { return _flow_type; }
            set { _flow_type = value; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        private string _create_name;

        public string CREATE_NAME
        {
            get { return _create_name; }
            set { _create_name = value; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        private DateTime? _create_date;

        public DateTime? CREATE_DATE
        {
            get { return _create_date; }
            set { _create_date = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        private string _note;

        public string NOTE
        {
            get { return _note; }
            set { _note = value; }
        }
    }
}
