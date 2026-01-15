namespace MusiBuy.Common.Enumeration
{
    public enum UserTypeEnum : int
    {
#if DEBUG

        #region Local
        Therapist = 4,
        Admin = 1
        #endregion

#else
        #region Live
         Therapist = 3,
        Admin = 1
#endregion
#endif
    }
}
