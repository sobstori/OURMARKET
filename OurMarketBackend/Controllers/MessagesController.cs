using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OurMarketBackend.Models;
using OurMarketBackend.ViewModels;
using OurMarketBackend.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OurMarketBackend.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<MessagesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // Inbox: list of users you have messages with (distinct)
        public async Task<IActionResult> Inbox()
        {
            var currentUserId = _userManager.GetUserId(User);

            var conversations = await _context.Messages
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .ToListAsync();

            var conversationUsers = conversations
                .Select(m => m.SenderId == currentUserId ? m.Receiver : m.Sender)
                .Distinct()
                .ToList();

            return View(conversationUsers);
        }

        // View conversation between current user and another user
        public async Task<IActionResult> Conversation(string otherUserId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var otherUser = await _userManager.FindByIdAsync(otherUserId);

            if (otherUser == null || currentUser == null)
            {
                return NotFound();
            }

            var messages = await _context.Messages
                .Where(m =>
                    (m.SenderId == currentUser.Id && m.ReceiverId == otherUser.Id) ||
                    (m.SenderId == otherUser.Id && m.ReceiverId == currentUser.Id))
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            var viewModel = new ConversationViewModel
            {
                CurrentUser = currentUser,
                OtherUser = otherUser,
                Messages = messages
            };

            return View(viewModel);
        }

        // GET: Send message form
        [HttpGet]
        public async Task<IActionResult> Send(string receiverId)
        {
            if (string.IsNullOrEmpty(receiverId))
                return RedirectToAction(nameof(Inbox));

            var receiver = await _userManager.FindByIdAsync(receiverId);
            if (receiver == null)
                return RedirectToAction(nameof(Inbox));

            var model = new SendMessageViewModel
            {
                ReceiverId = receiver.Id,
                ReceiverEmail = receiver.Email
            };

            return View(model);
        }

        // POST: Send message
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(SendMessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Send message validation failed: {Errors}", string.Join("; ", errors));
                return View(model);
            }

            var senderId = _userManager.GetUserId(User);

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = model.ReceiverId,
                Content = model.Content,
                SentAt = System.DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Conversation), new { otherUserId = model.ReceiverId });
        }
    }
}
