using BlogApp.Data;
using BlogApp.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class SeedData
    {
        private readonly BlogContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedData(BlogContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            var role = await _roleManager.FindByNameAsync("admin");
            if (role == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("admin"));
            }

            var adminUser = await _userManager.FindByEmailAsync("admin@blog.com");
            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@blog.com",
                    Email = "admin@blog.com"
                };

                var result = await _userManager.CreateAsync(user, "Password123!");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "admin");
                    adminUser = user; // güncel adminUser'ı ata
                }
            }

            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(
                    new Category { Name = "Technology", Url = "technology" },
                    new Category { Name = "Health", Url = "health" }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.Posts.Any())
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Url == "technology");

                if (category != null && adminUser != null)
                {
                    var post = new Post
                    {
                        Title = "New Technology Trends",
                        Description = "Short summary of the post",
                        Content = "Content of the blog post about technology trends...",
                        Url = "new-technology-trends",
                        UserId = adminUser.Id,
                        Image = "default.jpg",
                        PublishedOn = DateTime.Now,
                        IsActive = true,
                        CategoryId = category.Id
                    };

                    _context.Posts.Add(post);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}