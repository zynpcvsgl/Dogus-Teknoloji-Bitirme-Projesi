using BlogApp.Data.Abstract;
using BlogApp.Entity;
using System.Linq;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfCommentRepository : ICommentRepository
    {
        private readonly BlogContext _context;

        public EfCommentRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Comment> Comments => _context.Comments;

        public void CreateComment(Comment comment)
        {
            _context.Comments.Add(comment);
            Save();
        }

        public void RemoveComment(Comment comment)
        {
            _context.Comments.Remove(comment);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
