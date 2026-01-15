using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models
{
    public class HomeViewModel
    {
        public int TotalUser { get; set; }
        public int TotalCreators { get; set; }
        public int TotalTokenPuchased { get; set; }
        public int RevenueGenerated { get; set; }


        public int TotalAudioPost { get; set; }
        public int TotalVideoPost { get; set; }
        public int TotalImagePost { get; set; }
        public int TotalTextPost { get; set; }

    }
}
