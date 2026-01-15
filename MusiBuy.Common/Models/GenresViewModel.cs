using MusiBuy.Common.Common;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;  // Use this for ASP.NET Core


using Microsoft.AspNetCore.Mvc;

namespace MusiBuy.Common.Models
{
    public class GenresViewModel
    {
        public int Id { get; set; }

        public string GenreName { get; set; }
        public string? Description { get; set; }

        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public List<MySelectListItem> SelectCountry { get; set; }
        //public SelectList? SelectCountry { get; set; }

        public int RegionId { get; set; }
        public string? RegionName { get; set; }
        public SelectList? SelectRegion { get; set; }

        public bool IsActive { get; set; }
        public string IsActivevalue => IsActive ? "Active" : "In Active";

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
    public class MySelectListItem : SelectListItem
    {
        public string Key { get; set; }
    }
}
