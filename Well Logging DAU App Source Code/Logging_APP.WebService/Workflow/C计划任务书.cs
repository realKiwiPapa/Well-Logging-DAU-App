using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging_App.WebService.Workflow
{
    public class C计划任务书 : Controller
    {
        public C计划任务书()
        {
        }

        public C计划任务书(string obj_id1, string obj_id2)
            : base(obj_id1, obj_id2)
        {
        }

        private bool? _canSave = null;

        protected override bool canSave()
        {
            if (_canSave == null)
            {

                if (string.IsNullOrWhiteSpace(Obj_id1))
                {
                    _canSave = string.IsNullOrWhiteSpace(Obj_id2);
                    if (_canSave.Value) return _canSave.Value;
                    var data = queryData(Obj_id2, ServiceEnums.WorkflowType.测井任务通知单);
                    _canSave = data != null && data.Exists(x => x.TARGET_LOGINNAME == ActiveUserLoginName)
                    //_canSave = data != null && data[0].TARGET_LOGINNAME == ActiveUserLoginName;
                        && (data[0].SOURCE_LOGINNAME == data[0].TARGET_LOGINNAME || !ActiveUserRoles.Contains(ServiceEnums.UserRole.调度管理员));
                }
                else
                    _canSave = DataList != null 
                        && DataList[DataList.Count - 1].SOURCE_LOGINNAME == ActiveUserLoginName 
                        && CanSaveState.Contains(DataList[0].FLOW_STATE);
                //canSave = (string.IsNullOrWhiteSpace(Obj_id1) || (data != null && data[0].TARGET_LOGINNAME == ActiveUserLoginName && (data[0].SOURCE_LOGINNAME == data[0].TARGET_LOGINNAME || !ActiveUserRoles.Contains(ServiceEnums.UserRole.调度管理员)) && (DataList == null || CanSaveState.Contains(DataList[0].FLOW_STATE))));
            }
            return _canSave.Value;
        }

        protected override bool canAppointData()
        {
            return false;
        }

        protected override bool canSubmitReview()
        {
            if (string.IsNullOrWhiteSpace(Obj_id1)) return false;
            //if (!canSave())
            //{
            //    return DataLDataList[0].SOURCE_LOGINNAME != DataList[0].TARGET_LOGINNAME && canReview();
            //}
            return canSave();
        }

        private bool? _canReview = null;
        protected override bool canReview()
        {
            if (_canReview == null)
                _canReview = (ActiveUserRoles.Contains(ServiceEnums.UserRole.调度管理员) && DataList != null && DataList[0].TARGET_LOGINNAME == ActiveUserLoginName && DataList[0].FLOW_STATE == ServiceEnums.WorkflowState.提交审核);
            return _canReview.Value;
        }

        protected override bool canDelete()
        {
            return false;
        }

        protected override bool submitReview(string target_loginname)
        {
            return false;
        }

        protected override bool review(ServiceEnums.WorkflowState state)
        {
            return false;
        }

        protected override bool appointData(string target_loginname)
        {
            return false;
        }

        protected override bool canCancelSubmitReview()
        {
            return DataList != null && DataList[0].SOURCE_LOGINNAME == ActiveUserLoginName && DataList[0].FLOW_STATE == ServiceEnums.WorkflowState.提交审核;
        }

        protected override bool canCancelReview()
        {
            return DataList != null && DataList[0].SOURCE_LOGINNAME == ActiveUserLoginName && DataList[0].FLOW_STATE == ServiceEnums.WorkflowState.审核通过;
        }
    }
}