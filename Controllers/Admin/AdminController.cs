using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly BlogContext _context;

        public AdminController(BlogContext context)
        {
            _context = context;
        }

        // Admin Panel Dashboard
        public IActionResult Dashboard()
        {
            ViewBag.TotalPosts = _context.Posts.Count();
            ViewBag.TotalComments = _context.Comments.Count();
            ViewBag.TotalCategories = _context.Categories.Count();
            return View();
        }

        // Yorum Yönetimi
        public IActionResult ManageComments()
        {
            var comments = _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .OrderByDescending(c => c.PublishedOn)
                .ToList();

            return View(comments);
        }

        // Post Yönetimi
        public IActionResult ManagePosts()
        {
            var posts = _context.Posts
                .Include(p => p.Category)
                .OrderByDescending(p => p.PublishedOn)
                .ToList();

            return View(posts);
        }

        // Post Onaylama
        public IActionResult ApprovePost(int id)
        {
            var post = _context.Posts.Find(id);
            if (post != null)
            {
                post.IsActive = true;
                _context.SaveChanges();
            }
            return RedirectToAction("ManagePosts");
        }

        // Post Silme
        public IActionResult DeletePost(int id)
        {
            var post = _context.Posts.Find(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                _context.SaveChanges();
            }
            return RedirectToAction("ManagePosts");
        }

        // Yorum Silme
        public IActionResult DeleteComment(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageComments");
        }
    }
}
