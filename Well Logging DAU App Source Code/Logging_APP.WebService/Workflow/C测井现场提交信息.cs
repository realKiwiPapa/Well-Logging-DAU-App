using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging_App.WebService.Workflow
{
    public class C测井现场提交信息 : Controller
    {
        public C测井现场提交信息()
        {
        }

        public C测井现场提交信息(string obj_id1, string obj_id2)
            : base(obj_id1, obj_id2)
        {
        }
        private bool? _canContinue = null;
        private bool canContinue()
        {
            if (_canContinue == null)
            {
                var data = queryData(Obj_id1, ServiceEnums.WorkflowType.计划任务书);
                _canContinue = (data != null && data[0].FLOW_STATE == ServiceEnums.WorkflowState.审核通过);
            }
            return _canContinue.Value;
        }

        private bool? _canSave = null;
        protected override bool canSave()
        {
            if (_canSave == null)
            {
                _canSave = canContinue();
                if (!_canSave.Value) return false;
                var data = queryData(Obj_id1, ServiceEnums.WorkflowType.计划任务书);
                _canSave = (data[0].TARGET_LOGINNAME == ActiveUserLoginName && (DataList == null || CanSaveState.Contains(DataList[0].FLOW_STATE)));
            }
            return _canSave.Value;
        }

        protected override bool canAppointData()
        {
            return (DataList != null && !DataList.Exists(x => x.FLOW_STATE == ServiceEnums.WorkflowState.数据指派) && DataList[0].FLOW_STATE == ServiceEnums.WorkflowState.审核通过 && DataList[0].SOURCE_LOGINNAME == ActiveUserLoginName);
        }

        protected override bool canSubmitReview()
        {
            return canSave();
        }

        protected override bool canReview()
        {
            return (DataList != null && DataList[0].FLOW_STATE == ServiceEnums.WorkflowState.提交审核 && DataList[0].TARGET_LOGINNAME == ActiveUserLoginName);
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
            return DataList != null && DataList[0].SOURCE_LOGINNAME == ActiveUserLoginName && (DataList[0].FLOW_STATE == ServiceEnums.WorkflowState.审核通过 || DataList[0].FLOW_STATE == ServiceEnums.WorkflowState.数据指派);
        }
    }
}