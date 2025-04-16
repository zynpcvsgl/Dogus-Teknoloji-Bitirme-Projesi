using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IO;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly BlogContext _context;

        public PostsController(IPostRepository postRepository, ICommentRepository commentRepository, BlogContext context)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        public async Task<IActionResult> Index(string tag, int? categoryId, string q)
        {
            var posts = _postRepository.Posts;

            if (!string.IsNullOrEmpty(tag))
            {
                tag = tag.ToLower();
                posts = posts.Where(p =>
                    p.Tags != null &&
                    p.Tags.Any(t => t.Url != null && t.Url.ToLower() == tag));
            }

            if (categoryId.HasValue)
                posts = posts.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrEmpty(q))
            {
                q = q.ToLower();
                posts = posts.Where(p =>
                    (p.Title ?? "").ToLower().Contains(q) ||
                    (p.Description ?? "").ToLower().Contains(q) ||
                    (p.Content ?? "").ToLower().Contains(q));
            }

            var postList = await posts.Include(p => p.Category).ToListAsync();
            ViewBag.Categories = _context.Categories.ToList();

            return View(new PostViewModel { Posts = postList });
        }

        public async Task<IActionResult> Details(string url)
        {
            var post = await _postRepository.Posts
                .Include(p => p.Tags)
                .Include(p => p.Category)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Url == url);

            if (post == null)
                return NotFound();

            return View(post);
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddComment(int PostId, string Text)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return Json(new { success = false, message = "Yorum boş olamaz." });

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                return Json(new { success = false, message = "Geçersiz kullanıcı kimliği." });

            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userIdStr);

            if (user == null)
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });

            var comment = new Comment
            {
                Text = Text,
                PublishedOn = DateTime.Now,
                PostId = PostId,
                UserId = userIdStr
            };

            _commentRepository.CreateComment(comment);

            return Json(new
            {
                success = true,
                username = user.UserName,
                text = comment.Text,
                published = comment.PublishedOn.ToString("g"),
                avatar = user.Image ?? "default.jpg"
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(PostCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(model);
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized();

            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userIdStr);
            if (user == null)
                return Unauthorized();

            var url = GenerateUrlSlug(model.Title);

            string imageName = "default.jpg";
            if (model.ImageFile != null)
            {
                var extension = Path.GetExtension(model.ImageFile.FileName);
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowed.Contains(extension.ToLower()))
                {
                    ModelState.AddModelError("ImageFile", "Geçerli bir görsel dosyası yükleyin.");
                    ViewBag.Categories = _context.Categories.ToList();
                    return View(model);
                }

                imageName = Guid.NewGuid().ToString() + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", imageName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    model.ImageFile.CopyTo(stream);
                }
            }

            var post = new Post
            {
                Title = model.Title!,
                Description = model.Description!,
                Content = model.Content!,
                Url = url,
                Image = imageName,
                PublishedOn = DateTime.Now,
                IsActive = true,
                UserId = userIdStr,
                CategoryId = model.CategoryId!.Value
            };

            // Tag'ları ekliyoruz
            if (model.Tags != null && model.Tags.Any())
            {
                var tags = model.Tags.Select(tag => new Tag
                {
                    Name = tag,
                    Url = GenerateUrlSlug(tag)
                }).ToList();

                post.Tags.AddRange(tags); // Post'a etiketleri ekliyoruz
            }

            _postRepository.CreatePost(post);
            return RedirectToAction("Index");
        }

        private string GenerateUrlSlug(string? title)
        {
            return title?.ToLower()
                .Replace(" ", "-")
                .Replace("ç", "c").Replace("ğ", "g")
                .Replace("ı", "i").Replace("ö", "o")
                .Replace("ş", "s").Replace("ü", "u")
                .Replace(".", "").Replace(",", "")
                .Replace("?", "").Replace("!", "")
                .Replace(":", "").Replace(";", "") ?? "";
        }
    }
}
