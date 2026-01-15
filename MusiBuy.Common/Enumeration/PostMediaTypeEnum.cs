using System.ComponentModel.DataAnnotations;

namespace MusiBuy.Common.Enumeration
{
    public enum PostMediaTypeEnum : int
    {
#if DEBUG
        [Display(Name = "Audio")]
        Audio = 2,
        [Display(Name = "Video")]
        Video = 3,
        [Display(Name = "Image")]
        Image = 4,
        [Display(Name = "Text")]
        Text = 5,

#else
        [Display(Name = "Audio")]
        Audio = 2,
        [Display(Name = "Video")]
        Video = 3,
        [Display(Name = "Image")]
        Image = 4,
        [Display(Name = "Text")]
        Text = 5,
#endif

    }
}
