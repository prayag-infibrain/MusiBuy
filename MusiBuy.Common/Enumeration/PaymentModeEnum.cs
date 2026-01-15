namespace MusiBuy.Common.Enumeration
{
    public enum PaymentModeEnum : int
    {
#if DEBUG

        #region Local
        Monthly = 7,
        Yearly = 10,
        Quarterly = 11,
        HalfYearly = 12,

        #endregion

#else
        #region Live
        Monthly = 7,
        Yearly = 10,
        Quarterly = 11,
        HalfYearly = 12,
        #endregion
#endif
    }
}
