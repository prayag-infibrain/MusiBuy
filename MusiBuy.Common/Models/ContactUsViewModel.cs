using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusiBuy.Common.Common;

namespace MusiBuy.Common.Models
{
    public class ContactUsViewModel
    {

        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Subject { get; set; } = null!;

        public string Message { get; set; } = null!;
        public int UserId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
