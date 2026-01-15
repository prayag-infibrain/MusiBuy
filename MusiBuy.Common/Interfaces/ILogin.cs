using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface ILogin
    {
       UserViewModel ValidateLogin(LoginViewModel objLoginViewModel);
        //public UserViewModel ValidateFrontLogin(FrontLoginViewModel objLoginViewModel);
    }
}