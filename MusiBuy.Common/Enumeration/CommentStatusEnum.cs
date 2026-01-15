namespace MusiBuy.Common.Enumeration
{
    public enum CommentStatusEnum : int
    {
#if DEBUG
        Approved = 18,
        Pending = 19,
        Rejected = 20,
        Delete = 21,

#else
        Approved = 18,
        Pending = 19,
        Rejected = 20,
        Delete = 21,
#endif

    }
}
