using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces.Marketing
{
    public interface IMarketingChangePassword
    {
        bool UpdatePassword(ChangePasswordViewModel objChangePasswordViewModel);
        string GetOldPassword(int userId);
        public string GetOldPasswordFrontuser(int userId);
        public bool UpdatePasswordFrontUser(ChangePasswordViewModel objChangePasswordViewModel);

    }
}