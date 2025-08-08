using System.ComponentModel.DataAnnotations;

namespace OurMarketBackend.Models
{
    public class Listing
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a title")]
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a price")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please enter a location")]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "Please enter an image URL")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Please select a category")]
        public string Category { get; set; } = null!;
    }
}
