using System.ComponentModel.DataAnnotations;

public class ListingCreateViewModel : IValidatableObject
{
    // base details
    [Required, StringLength(120)]
    public string? Title { get; set; }

    public string? Description { get; set; }
    public string? Category { get; set; }
    [Required, Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
    public double Price { get; set; }

    // zip code
    [Required, RegularExpression(@"^\d{5}$")]
    public string Zip { get; set; } = null!;
    public string City { get; set; } = "";
    public string State { get; set; } = "";

    // images
    [Required(ErrorMessage = "Please upload 1–3 images.")]
    public List<IFormFile>? Photos { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext _)
    {
        if (Photos is null || Photos.Count is < 1 or > 3)
            yield return new ValidationResult("Upload between 1 and 3 images.", new[] { nameof(Photos) });

        var okTypes = new HashSet<string> { "image/jpeg", "image/png", "image/webp", "image/gif" };
        const long maxBytes = 5 * 1024 * 1024; // 5MB each
        foreach (var f in Photos ?? Enumerable.Empty<IFormFile>())
        {
            if (!okTypes.Contains(f.ContentType))
                yield return new ValidationResult("Only JPG, PNG, WEBP, or GIF.", new[] { nameof(Photos) });
            if (f.Length <= 0 || f.Length > maxBytes)
                yield return new ValidationResult("Each image must be ≤ 5MB.", new[] { nameof(Photos) });
        }
    }
}
