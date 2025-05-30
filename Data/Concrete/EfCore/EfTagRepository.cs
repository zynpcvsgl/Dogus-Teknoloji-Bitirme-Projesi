using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data.Concrete;
using BlogApp.Data.Concrete.EfCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfTagRepository : ITagRepository
    {
        private readonly BlogContext _context;

        public EfTagRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Tag> Tags => _context.Tags;

        public void CreateTag(Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();
        }
    }
}

