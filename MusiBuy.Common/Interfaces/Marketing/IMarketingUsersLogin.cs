using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces.Marketing
{
    public interface IMarketingUsersLogin
    {
        MarketingUserViewModel ValidateLogin(LoginViewModel objLoginViewModel);
        //public UserViewModel ValidateFrontLogin(FrontLoginViewModel objLoginViewModel);
    }
}