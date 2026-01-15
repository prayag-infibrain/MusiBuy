using MusiBuy.Common.Models;

namespace MusiBuy.Common.Interfaces
{
    public interface IStripeToken
    {
        string CreateTokenCheckoutSession(int planId, int UserId);
        //public UserViewModel ValidateFrontLogin(FrontLoginViewModel objLoginViewModel);
    }
}