using OurMarketBackend.Models;

namespace OurMarketBackend.models
{
    public class ListingImage
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public Listing Listing { get; set; } = null!;
        public string FileName { get; set; } = null!;   // images stored under wwwroot/images/listings

    }
}
