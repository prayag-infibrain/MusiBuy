using MusiBuy.Common.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MusiBuy.Marketing.Helper
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
            if (act == GlobalCode.Actions.Index && !CurrentUserSession.HasViewPermission)
            {
                isAuthorize = false;
            }

            if (act == GlobalCode.Actions.Create && !CurrentUserSession.HasAddPermission)
            {
                isAuthorize = false;
            }

            if (act == GlobalCode.Actions.Edit && !CurrentUserSession.HasEditPermission)
            {
                isAuthorize = false;
            }

            if (act == GlobalCode.Actions.Delete && !CurrentUserSession.HasDeletePermission)
            {
                isAuthorize = false;
            }

            if (act == GlobalCode.Actions.Detail && !CurrentUserSession.HasDetailPermission)
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
