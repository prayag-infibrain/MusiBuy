using MusiBuy.Common.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MusiBuy.Admin.Helper
{
    public class ValidatePageAccess : ActionFilterAttribute
    {
        GlobalCode.Actions act;

        public ValidatePageAccess(GlobalCode.Actions Action)
        {
            act = Action;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isAuthorize = true;
            if (act == GlobalCode.Actions.Index && !CurrentAdminSession.HasViewPermission)
            {
                isAuthorize = false;
            }

            if (act == GlobalCode.Actions.Create && !CurrentAdminSession.HasAddPermission)
            {
                isAuthorize = false;
            }

            if (act == GlobalCode.Actions.Edit && !CurrentAdminSession.HasEditPermission)
            {
                isAuthorize = false;
            }

            if (act == GlobalCode.Actions.Delete && !CurrentAdminSession.HasDeletePermission)
            {
                isAuthorize = false;
            }

            if (act == GlobalCode.Actions.Detail && !CurrentAdminSession.HasDetailPermission)
            {
                isAuthorize = false;
            }

            if (!isAuthorize)
            {
                filterContext.Result = new RedirectResult("~/Home/Index?msg=unauthorize");
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
