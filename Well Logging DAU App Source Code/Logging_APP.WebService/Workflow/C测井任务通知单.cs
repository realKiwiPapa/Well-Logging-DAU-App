using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 

namespace Logging_App.WebService.Workflow
{
    public class C测井任务通知单 : Controller
    {
        public C测井任务通知单()
        {
        }

        public C测井任务通知单(string obj_id1, string obj_id2)
            : base(obj_id1, obj_id2)
        {
        }

        private bool? _canSave = null;

        protected override bool canSave()
        {
            if (_canSave == null)
                _canSave = (ActiveUserRoles.Contains(ServiceEnums.UserRole.调度管理员) 
                    && (DataList == null 
                    ||(DataList.Count==1&&DataList[0].SOURCE_LOGINNAME == ActiveUserLoginName)
                    || (DataList[0].SOURCE_LOGINNAME != DataList[0].TARGET_LOGINNAME && DataList[0].TARGET_LOGINNAME == ActiveUserLoginName)));
            return _canSave.Value;
        }

        protected override bool canAppointData()
        {
            return !string.IsNullOrWhiteSpace(Obj_id1);
            //return !string.IsNullOrWhiteSpace(Obj_id1) && canSave() ;//&& (DataList == null || DataList[0].TARGET_LOGINNAME != DataList[0].SOURCE_LOGINNAME);
        }

        protected override bool canSubmitReview()
        {
            return false;
        }

        protected override bool canReview()
        {
            return false;
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
            return false;
        }

        protected override bool canCancelReview()
        {
            return false;
        }
    }
}