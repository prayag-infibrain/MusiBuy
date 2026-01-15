namespace MusiBuy.Common.Interfaces
{
    public interface IResetPassword
    {
        bool ResetUserPassword(string password, int userID);
        public bool ResetFrontUserPassword(string password, int userID);
    }
}
