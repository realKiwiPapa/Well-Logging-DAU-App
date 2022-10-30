using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logging_App.WebService.Workflow
{
    public class C测井作业收集信息 : Controller
    {
        public C测井作业收集信息()
        {
        }

        public C测井作业收集信息(string obj_id1, string obj_id2)
            : base(obj_id1, obj_id2)
        {
        }
        protected override bool canSave()
        {
            return ActiveUserRoles.Contains(ServiceEnums.UserRole.调度管理员);
        }

        protected override bool canAppointData()
        {
            return false;
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
            return false;
        }

        protected override bool canCancelReview()
        {
            return false;
        }
    }
}