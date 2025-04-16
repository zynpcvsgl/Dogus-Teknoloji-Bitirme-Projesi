using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BlogApp.Models
{
    public class PostCreateViewModel
    {
        public int PostId { get; set; }

        [Required]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Yayında mı?")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Kategori")]
        public int? CategoryId { get; set; }

        [Display(Name = "Görsel")]
        public IFormFile? ImageFile { get; set; }

        // Tags listesi eklendi
        [Display(Name = "Etiketler")]
        public List<string> Tags { get; set; } = new List<string>(); // Etiketlerin listesi
    }
}
