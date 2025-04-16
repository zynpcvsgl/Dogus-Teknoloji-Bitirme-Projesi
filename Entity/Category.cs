using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entity
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public List<Post> Posts { get; set; } = new List<Post>();  
        [Required, StringLength(255)] 
        public string Url { get; set; } = string.Empty;
    }
}
