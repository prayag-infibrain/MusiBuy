using MusiBuy.Common.Helpers;

namespace MusiBuy.Common.Models
{
    public class DropDownBindViewModel
    {
        public Int64 value { get; set; }
        public string name { get; set; } = string.Empty;
        public string key { get; set; } = string.Empty;

        /// <summary>
        /// Encrypted value
        /// </summary>
        public string EncValue { get { return Encryption.Encrypt(value.ToString()); } }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public decimal? AverageRating { get; set; }
    }
}