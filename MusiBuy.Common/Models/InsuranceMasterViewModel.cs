using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusiBuy.Common.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MusiBuy.Common.Models
{
    public class InsuranceMasterViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Catagory name is required.")]
        [DisplayName("Catagory name")]
        public int CatagoryId { get; set; }

        public SelectList? Catagorys { get; set; }

        public string? CatagoryName { get; set; }
        [Required(ErrorMessage = "Agency name is required.")]
        [DisplayName("Agency name")]
        public int AgencyId { get; set; }

        public SelectList? Agencys { get; set; }

        public string AgencyName { get; set; }
        [Required(ErrorMessage = "Product name is required.")]
        [DisplayName("Product name")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Insured name is required.")]
        [DisplayName("Insured name")]
        public string InsuredName { get; set; }
        [Required(ErrorMessage = "Policy number is required.")]
        [DisplayName("Policy number")]
        public string PolicyNumber { get; set; }
        [Required(ErrorMessage = "Premium amount is required.")]
        [DisplayName("Premium amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount cannot be negative.")]
        public decimal PremiumAmount { get; set; }
        [Required(ErrorMessage = "Login date is required.")]
        [DisplayName("Login date")]
        public DateOnly StartDate { get; set; }
        [Required(ErrorMessage = "Login date is required.")]
        [DisplayName("Login date")]
        public string strStartDate { get; set; }

        public DateOnly? NextRenewalDate { get; set; }
        [Required(ErrorMessage = "Renewal date is required.")]
        [DisplayName("Renewal date")]
        public string strRenewalDate { get; set; }

        public DateOnly RenewalDate { get; set; }
        [Required(ErrorMessage = "Payment mode is required.")]
        [DisplayName("Payment mode")]
        public int PaymentMode { get; set; }
        public string  strPaymentMode { get; set; }

        public SelectList? PaymentModeEnum { get; set; }

        public IFormFile? InsuranceDocument { get; set; }

        public string? Document { get; set; }
       
        public IFormFile? strDocument { get; set; }

        public bool IsActive { get; set; }
        public string? Active { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        [Required(ErrorMessage = "Birthdate is required.")]
        [DisplayName("Birthdate")]
        public string strBirthDate { get; set; }
        public DateOnly BirthDate { get; set; }
        [Required(ErrorMessage = "WhatsApp number is required.")]
        [DisplayName("WhatsApp number")]
        public string WhatsAppNumber { get; set; }
    }
}
