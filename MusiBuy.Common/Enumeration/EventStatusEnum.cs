namespace MusiBuy.Common.Enumeration
{
    public enum EventStatusEnum : int
    {
#if DEBUG
        Upcoming = 14,
        Ongoing = 15,
        Completed = 16,
        Cancelled = 17,

#else
        Upcoming = 14,
        Ongoing = 15,
        Completed = 16,
        Cancelled = 17,
#endif

    }
}
