
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusiBuy.Common.DB;

namespace MusiBuy.Common.Models.API
{
    public class FrontUserViewModel
    {
        public int Id { get; set; }
        public int UserTypeId { get; set; }
        [ValidateNever]
        public string UserTypeName { get; set; }
        public SelectList? SelectUserType { get; set; }
        public int? RoleId { get; set; }
        [ValidateNever]
        public string RoleName { get; set; }
        public SelectList? SelectRole { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public string Mobile { get; set; }
        public string? Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public string Bio { get; set; }

        public string? StrImage { get; set; }
        public List<int>? UserPrefrence { get; set; } = new List<int>();
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; }
        public string? Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        [ValidateNever]
        public bool IsFollowing { get; set; }
        [ValidateNever]
        public int Followers { get; set; }
        [ValidateNever]
        public int Followings { get; set; }
        [ValidateNever]
        public int TokenErned { get; set; }
    }

    public class ContactUsApiModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
    }
}
