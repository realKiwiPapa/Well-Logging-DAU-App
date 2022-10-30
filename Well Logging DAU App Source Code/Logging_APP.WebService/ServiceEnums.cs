using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging_App.WebService
{
    public class ServiceEnums
    {
        ///// <summary>
        ///// 用户类型类型
        ///// </summary>
        //public enum UserType1 : int
        //{
        //    分公司用户 = 0,
        //    小队用户 = 1,
        //    解释处理用户 = 2
        //}

        /// <summary>
        /// 流程状态
        /// </summary>
        ///  

        public enum WorkflowState : int
        {
            [Workflow.Property("CanAppointData")]
            数据指派,

            [Workflow.Property("CanSubmitReview")]
            提交审核,

            [Workflow.Property("CanReview")]
            审核通过,

            [Workflow.Property("CanReview")]
            审核未通过,

            新建,

            [Workflow.Property("CanCancelSubmitReview")]
            取消提交审核,

            [Workflow.Property("CanCancelReview")]
            退回审核
        }

        /// <summary>
        /// 业务流程类型
        /// </summary>
        public enum WorkflowType : int
        {
            [Workflow.Class(typeof(Workflow.C测井任务通知单))]
            测井任务通知单 = 0,

            [Workflow.Class(typeof(Workflow.C计划任务书))]
            计划任务书 = 1,

            [Workflow.Class(typeof(Workflow.C测井作业收集信息))]
            测井作业收集信息 = 2,

            小队施工信息 = 3,
            [Workflow.Class(typeof(Workflow.C测井现场提交信息))]
            测井现场提交信息 = 4,

            [Workflow.Class(typeof(Workflow.C解释处理作业))]
            解释处理作业 = 5,
            归档入库 = 6
        }

        /// <summary>
        /// 用户角色
        /// </summary>
        public enum UserRole : int
        {
            调度管理员 = 0,
            解释管理员 = 1,
            文件下载审核 = 2,
            批量下载员 = 3,
            刻盘管理员 = 4,
            系统管理员 = 999
        }
    }
}
