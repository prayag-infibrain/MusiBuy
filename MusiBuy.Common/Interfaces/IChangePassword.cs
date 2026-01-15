using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface IChangePassword
    {
        bool UpdatePassword(ChangePasswordViewModel objChangePasswordViewModel);
        string GetOldPassword(int userId);
        public string GetOldPasswordFrontuser(int userId);
        public bool UpdatePasswordFrontUser(ChangePasswordViewModel objChangePasswordViewModel);

    }
}