namespace OurMarketBackend.Models
{
    public class Listing
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = null!; 
        
        public string Description { get; set; } = null!;
        
        public decimal Price { get; set; }
        
        public string Location { get; set; } = null!;  
        
        public string ImageUrl { get; set; } = null!;  
        
        public string Category { get; set; } = null!; 
    }
}
