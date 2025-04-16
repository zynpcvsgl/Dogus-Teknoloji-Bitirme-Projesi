using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Models
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Ad soyad alanı boş bırakılamaz.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public IFormFile? ImageFile { get; set; }

        // Kullanıcının önceki profil fotoğrafını göstermek için gerekli
        public string? ExistingImage { get; set; }
    }
}
