using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models.API
{
    public class CreatorRegistrationModel
    {
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        //public int CountryId { get; set; }
        //public string? CountryName { get; set; }
        public string Mobile { get; set; }
        //public string? Username { get; set; }
        public string Password { get; set; }
        //public bool IsActive { get; set; }
    }
}
