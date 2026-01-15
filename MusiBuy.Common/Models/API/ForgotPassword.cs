using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiBuy.Common.Models.API
{
    public class ForgotPassword
    {
        public string EmailId { get; set; }
        public bool ResendOtp { get; set; }
    }

    public class OTPVerifaction
    {
        public string EmailId { get; set; }
        public int OTP { get; set; }
    }

    public class UpdatePassword
    {
        public string EmailId { get; set; }
        public string password { get; set; }
    }
    public class ChangePassword
    {
        public int Id { get; set; }
        public string Oldpassword { get; set; }
        public string Newpassword { get; set; }
    }
}
