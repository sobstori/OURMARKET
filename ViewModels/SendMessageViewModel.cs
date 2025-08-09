using System.ComponentModel.DataAnnotations;

namespace OurMarketBackend.ViewModels
{
    public class SendMessageViewModel
    {
        [Required]
        public string ReceiverId { get; set; } = null!;

        // Nullable since not posted back
        public string? ReceiverEmail { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Message cannot be longer than 500 characters")]
        public string Content { get; set; } = null!;
    }
}
