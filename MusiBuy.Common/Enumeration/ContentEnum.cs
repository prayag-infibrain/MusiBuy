namespace MusiBuy.Common.Enumeration
{
    public enum ContentEnum : int
    {
#if DEBUG

        #region Local
        Privacy = 40,
        Terms =38,
        AboutUs=1,
        FAQ = 39

        #endregion

#else
        #region Live
        Privacy = 40,
        Terms = 38,
        AboutUs=1,
        FAQ = 39
#endregion
#endif
    }
}
