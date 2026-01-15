using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models
{
    public class FileUploadProgress
    {

        public int TotalRecord { get; set; }

        public int RecordCount { get; set; }

        public int ErrorCount { get; set; }

        public string ErrorMessage { get; set; }

        public bool Inprogress { get; set; }
    }
}
