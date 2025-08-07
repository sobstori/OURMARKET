using System.ComponentModel.DataAnnotations;

namespace OurMarketBackend.Models
{
    public class Listing
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Posted At")]
        public DateTime PostedAt { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Seller Email")]
        [EmailAddress]
        public string SellerEmail { get; set; } = string.Empty;
    }
}
