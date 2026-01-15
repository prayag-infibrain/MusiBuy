namespace MusiBuy.Common.Enumeration
{
    public enum EnumTypes : int
    {
#if DEBUG
        ContentManagement = 1,
        PostMediaType = 2,
        PostStatus = 3,
        CommentStatus = 4,
        EventType = 5,
        EventStatus = 6,
        TokenType = 7,
        CreatorType = 9,
        ContentType = 10,
        ContentProdcastCategory = 11,
        ContentSocialMediaCategory = 12,
        ContentMusicProdcastType = 13

#else


         ContentManagement = 1,
         PostMediaType = 2,
        PostStatus = 3,
        CommentStatus = 4,
        EventType = 5,
        EventStatus = 6,
        TokenType = 7,
        CreatorType = 9,
        ContentType = 10,
        ContentProdcastCategory = 11,
        ContentSocialMediaCategory = 12,
        ContentMusicProdcastType = 13

#endif

    }
}
