using MusiBuy.Common.DB;
using MusiBuy.Common.Interfaces;
using MusiBuy.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Repositories
{
    public class ContactUsRepository : IContactUs
    {
        #region Member Declaration
        private readonly MusiBuyDB_Connection _Context;
        public ContactUsRepository(MusiBuyDB_Connection context)
        {
            this._Context = context;
        }
        #endregion


        #region Save
        public bool Save(ContactUsViewModel ContactUsViewModel)
        {
            var ContactUs = new ContactU();
            ContactUs.FullName = ContactUsViewModel.FullName;
            ContactUs.Email = ContactUsViewModel.Email;
            ContactUs.Subject = ContactUsViewModel.Subject;
            ContactUs.Message = ContactUsViewModel.Message;
            ContactUs.UserId = ContactUsViewModel.UserId;
            ContactUs.CreatedBy = ContactUsViewModel.UserId;
            ContactUs.CreatedOn = DateTime.Now;
            _Context.ContactUs.Add(ContactUs);
            if (_Context.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        #endregion
    }
}
