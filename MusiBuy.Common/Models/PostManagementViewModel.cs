using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // Use this for ASP.NET Core
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace MusiBuy.Common.Models
{
    public class PostManagementViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Creator")]
        public int CreatorId { get; set; }
        public string? CreatorName { get; set; }
        public string? CreatorType { get; set; }
        public SelectList? SelectCreator { get; set; }

        [Display(Name = "Media Type")]
        public int? MediaTypeId { get; set; }
        public SelectList? SelectMediaType { get; set; }
        public string? MediaTypeName { get; set; }



        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
        [StringLength(150, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MaxAllowedLength")]
        [Display(Name = "Title")]
        public string Title { get; set; } = null!;



        [Display(Name = "Description")]
        public string? Description { get; set; }


        public IFormFile? MediaFile { get; set; }
        public string? MediaFileName { get; set; }
        public string? StrMediaFile { get; set; }
        public string? MediaFileSize { get; set; }


        [Display(Name = "URL")]
        public string? Url { get; set; }


        [Display(Name = "Tags")]
        public string? Tags { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public SelectList? SelectStatus { get; set; }

        public DateTime PublishDate { get; set; }
        public int TotalCountOnPost { get; set; }


        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }




        [ValidateNever]
        public int UserTypeId { get; set; }
        [ValidateNever]
        public int RoleId { get; set; }
        [ValidateNever]
        public int UserId { get; set; }
        
        public int ContentTypeId { get; set; }
        [ValidateNever]
        public SelectList SelectContentType { get; set; }
        [ValidateNever]
        public string ContentTypeName { get; set; }
        
        public int? CountryId { get; set; }
        public SelectList? SelectCountry { get; set; }
        public string? CountryName { get; set; }
        
        public int? GenreId { get; set; }
        public SelectList? SelectGenre { get; set; }
        public string? GenreName { get; set; }
        [ValidateNever]
        public int? CategoryId { get; set; }
        public SelectList? SelectCategory { get; set; }
        public string? CategoryName { get; set; }

        public int? TypeId { get; set; }
        public string? TypeName { get; set; }
        public SelectList? SelectType { get; set; }

        [ValidateNever]
        public string DefaultImage { get; set; }
        [ValidateNever]
        public string DefaultIcon { get; set; }

        public bool? IsActive { get; set; }
    }
}
