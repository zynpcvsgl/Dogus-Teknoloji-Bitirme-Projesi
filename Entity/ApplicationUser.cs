using Microsoft.AspNetCore.Identity;

namespace BlogApp.Entity
{
    public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }  // Eğer boş olabilir, null atanabilir olmalı
    public string? Image { get; set; } // Aynı şekilde
}


}
