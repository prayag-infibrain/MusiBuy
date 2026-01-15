using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models
{
    public class UserPrefrenceViewModel
    {
        public int Id { get; set; }

        public int FrontUserId { get; set; }

        public int PrefrenceId { get; set; }
        public string PrefrenceName { get; set; }
    }
}
