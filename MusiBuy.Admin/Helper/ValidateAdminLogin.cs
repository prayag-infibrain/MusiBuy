using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace MusiBuy.Admin.Helper
{
    public class ValidateAdminLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CurrentAdminSession.UserID < 1)
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
            if (controller == "Recipient")
            {
                if (CurrentAdminSession.ViewRolePrivileges != null)
                {
                    CurrentAdminPermission Permission = new CurrentAdminPermission();
                    Permission.HasViewPermission = true;
                    Permission.HasAddPermission = true;
                    Permission.HasEditPermission = true;
                    Permission.HasDeletePermission = true;
                    Permission.HasDetailPermission = true;
                    CurrentAdminSession.Permission = Permission;
                }
            }
            else
            {
                if (CurrentAdminSession.ViewRolePrivileges == null)
                {
                    CurrentAdminPermission Permission = new CurrentAdminPermission();
                    Permission.HasViewPermission = false;
                    Permission.HasAddPermission = false;
                    Permission.HasEditPermission = false;
                    Permission.HasDeletePermission = false;
                    Permission.HasDetailPermission = false;
                    CurrentAdminSession.Permission = Permission;
                }
                else
                {
                    var viewRolePrivileges = CurrentAdminSession.ViewRolePrivileges.ViewRolePrivileges.Where(p => p.MenuItem.MenuItemController != null && p.MenuItem.MenuItemController.Trim() == controller && p.IsActive).FirstOrDefault();
                    if (viewRolePrivileges != null)
                    {
                        CurrentAdminPermission Permission = new CurrentAdminPermission();
                        Permission.HasViewPermission = viewRolePrivileges.View.Value;
                        Permission.HasAddPermission = viewRolePrivileges.Add.Value;
                        Permission.HasEditPermission = viewRolePrivileges.Edit.Value;
                        Permission.HasDeletePermission = viewRolePrivileges.Delete.Value;
                        Permission.HasDetailPermission = viewRolePrivileges.Detail.Value;
                        CurrentAdminSession.Permission = Permission;
                    }
                    else
                    {
                        CurrentAdminPermission Permission = new CurrentAdminPermission();
                        Permission.HasViewPermission = false;
                        Permission.HasAddPermission = false;
                        Permission.HasEditPermission = false;
                        Permission.HasDeletePermission = false;
                        Permission.HasDetailPermission = false;
                        CurrentAdminSession.Permission = Permission;
                    }
                }
            }
        }
    }
}
