using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;  // Use this for ASP.NET Core


using Microsoft.AspNetCore.Mvc;

namespace MusiBuy.Common.Models
{
    public class TokenPlanViewModel
    {
        public int Id { get; set; }

        public int TokenTypeId { get; set; }
        public string? TokenTypeName { get; set; }
        public SelectList? SelectTokenType { get; set; }

        public decimal? TokenQuantity { get; set; }

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }



    }
}
