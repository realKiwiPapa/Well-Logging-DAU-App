using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging_App.WebService.Workflow
{
    public class C解释处理作业 : Controller
    {
        public C解释处理作业()
        {
        }

        public C解释处理作业(string obj_id1, string obj_id2)
            : base(obj_id1, obj_id2)
        {
        }

        private bool? _canSave = null;
        protected override bool canSave()
        {
            if (_canSave == null)
                _canSave = DataList == null 
                    || (DataList[DataList.Count - 1].SOURCE_LOGINNAME == ActiveUserLoginName &&  CanSaveState.Contains(DataList[0].FLOW_STATE));
            return _canSave.Value;
        }

        protected override bool canAppointData()
        {
            return false;
        }

        protected override bool canSubmitReview()
        {
            return canSave() && (DataList != null && DataList[DataList.Count - 1].SOURCE_LOGINNAME == ActiveUserLoginName);
        }

        protected override bool canReview()
        {
            return (DataList != null && DataList[0].TARGET_LOGINNAME == ActiveUserLoginName && DataList[0].FLOW_STATE == ServiceEnums.WorkflowState.提交审核);
        }

        protected override bool canDelete()
        {
            return ActiveUserLoginName == "admin";
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