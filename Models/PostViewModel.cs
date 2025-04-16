using BlogApp.Entity;  // Post sınıfı için gerekli using direktifi

namespace BlogApp.Models
{
    public class PostViewModel
    {
        public List<Post> Posts { get; set; } = new();
    }
}
