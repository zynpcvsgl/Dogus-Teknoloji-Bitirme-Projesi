using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfPostRepository : IPostRepository
    {
        private readonly BlogContext _context;

        public EfPostRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Posts
        {
            get
            {
                return _context.Posts
                    .Include(p => p.Category)
                    .Include(p => p.Tags)
                    .Include(p => p.Comments)
                    .AsQueryable();
            }
        }

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public Post GetPostById(int id)
        {
            return _context.Posts
                .Include(p => p.Tags)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.PostId == id)!; // <-- Warning temizlendi
        }

        public void EditPost(Post post)
        {
            _context.Posts.Update(post);
            _context.SaveChanges();
        }
    }
}
