using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models.API
{
    public class UserRegistrationModel
    {
        //public int Id { get; set; }
        public int UserTypeId { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public List<int>? UserPrefrence { get; set; } = new List<int>();
        public int CountryId { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
    }
    public class FrontUserUpdateProfileModel
    {
        public int Id { get; set; }
        public int UserTypeId { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int CountryId { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string? UserPrefrence { get; set; } = "";
        public string Bio { get; set; }
    }
}
