using Microsoft.AspNetCore.Identity;

namespace OurMarketBackend.Models
{
	public class ApplicationUser : IdentityUser
	{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? State { get; set; }
	}
}
