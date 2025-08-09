using System.Collections.Generic;
using OurMarketBackend.Models;

namespace OurMarketBackend.ViewModels
{
    public class ConversationViewModel
    {
        public ApplicationUser OtherUser { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public List<Message> Messages { get; set; }
    }
}
