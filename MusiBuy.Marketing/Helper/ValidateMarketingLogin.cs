using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace MusiBuy.Marketing.Helper
{
    public class ValidateMarketingLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CurrentUserSession.UserID < 0)
            {
                filterContext.HttpContext.Session.Clear();
                filterContext.Result = new RedirectResult("~/Home/Index?msg=unauthorize");
                return;
            }
            var controllerName = ((ControllerActionDescriptor)filterContext.ActionDescriptor).ControllerName;
            ModulePrivileges(controllerName);
            base.OnActionExecuting(filterContext);
        }

        private void ModulePrivileges(string controller)
        {
            CurrentAdminPermission Permission = new CurrentAdminPermission();
            Permission.HasViewPermission = true;
            Permission.HasAddPermission = true;
            Permission.HasEditPermission = true;
            Permission.HasDeletePermission = true;
            Permission.HasDetailPermission = true;
            CurrentUserSession.Permission = Permission;
            //if (controller == "Recipient")
            //{
            //    if (CurrentUserSession.ViewRolePrivileges != null)
            //    {
            //        CurrentAdminPermission Permission = new CurrentAdminPermission();
            //        Permission.HasViewPermission = true;
            //        Permission.HasAddPermission = true;
            //        Permission.HasEditPermission = true;
            //        Permission.HasDeletePermission = true;
            //        Permission.HasDetailPermission = true;
            //        CurrentUserSession.Permission = Permission;
            //    }
            //}
            //else
            //{
            //    if (CurrentUserSession.ViewRolePrivileges == null)
            //    {
            //        CurrentAdminPermission Permission = new CurrentAdminPermission();
            //        Permission.HasViewPermission = false;
            //        Permission.HasAddPermission = false;
            //        Permission.HasEditPermission = false;
            //        Permission.HasDeletePermission = false;
            //        Permission.HasDetailPermission = false;
            //        CurrentUserSession.Permission = Permission;
            //    }
            //    else
            //    {
            //        var viewRolePrivileges = CurrentUserSession.ViewRolePrivileges.ViewRolePrivileges.Where(p => p.MenuItem.MenuItemController != null && p.MenuItem.MenuItemController.Trim() == controller && p.IsActive).FirstOrDefault();
            //        if (viewRolePrivileges != null)
            //        {
            //            CurrentAdminPermission Permission = new CurrentAdminPermission();
            //            Permission.HasViewPermission = viewRolePrivileges.View.Value;
            //            Permission.HasAddPermission = viewRolePrivileges.Add.Value;
            //            Permission.HasEditPermission = viewRolePrivileges.Edit.Value;
            //            Permission.HasDeletePermission = viewRolePrivileges.Delete.Value;
            //            Permission.HasDetailPermission = viewRolePrivileges.Detail.Value;
            //            CurrentUserSession.Permission = Permission;
            //        }
            //        else
            //        {
            //            CurrentAdminPermission Permission = new CurrentAdminPermission();
            //            Permission.HasViewPermission = false;
            //            Permission.HasAddPermission = false;
            //            Permission.HasEditPermission = false;
            //            Permission.HasDeletePermission = false;
            //            Permission.HasDetailPermission = false;
            //            CurrentUserSession.Permission = Permission;
            //        }
            //    }
            //}
        }
    }
}
