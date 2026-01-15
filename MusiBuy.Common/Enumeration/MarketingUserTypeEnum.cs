namespace MusiBuy.Common.Enumeration
{
    public enum MarketingUserTypeEnum : int
    {
#if DEBUG
        Publishers = 1,
        Advertiser = 2,

#else
        Publishers = 1,
        Advertiser = 2,
#endif

    }
}
