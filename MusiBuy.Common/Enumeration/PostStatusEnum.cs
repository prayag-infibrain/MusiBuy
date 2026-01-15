namespace MusiBuy.Common.Enumeration
{
    public enum PostStatusEnum : int
    {
#if DEBUG
        Draft = 7,
        In_Review = 8,
        Published = 9,
        Rejected = 10,

#else


         Draft = 7,
         In_Review = 8,
        Published = 9,
        Rejected = 10,

#endif

    }
}
