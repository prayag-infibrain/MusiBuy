using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models.API
{
    public class CreatorProfileViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Bio { get; set; }
        public string RoleName { get; set; }
        public string ProfilePicture { get; set; }
        public string Followers { get; set; }
        public string Followings { get; set; }
        public string TokenErned { get; set; }

        public List<PostManagementViewModel> PostList { get; set; }

        public bool IsActive { get; set; }


    }
}
