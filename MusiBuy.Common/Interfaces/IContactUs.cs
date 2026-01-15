using MusiBuy.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Interfaces
{
    public interface IContactUs
    {
        bool Save(ContactUsViewModel ContactUsViewModel);
    }
}
