namespace BlogApp.Models
{
    public class UserProfileViewModel
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }

    public string? Image { get; set; }

    // Düzenleme için ekstra alanlar
    public string? NewPassword { get; set; }
    public IFormFile? ImageFile { get; set; }
}

}
