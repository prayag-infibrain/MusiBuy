namespace MusiBuy.Common.Interfaces.Marketing
{
    public interface IMarketingResetPassword
    {
        bool ResetUserPassword(string password, int userID);
        public bool ResetFrontUserPassword(string password, int userID);
    }
}
