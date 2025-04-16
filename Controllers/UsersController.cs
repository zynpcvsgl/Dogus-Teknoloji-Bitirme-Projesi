using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Giriş - GET
        [HttpGet]
        public IActionResult Login() => View();

        // Giriş - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Posts");

            ModelState.AddModelError("", "Geçersiz giriş bilgileri.");
            return View(model);
        }

        // Kayıt - GET
        [HttpGet]
        public IActionResult Register() => View();

        // Kayıt - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Posts");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // Çıkış
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Users");
        }

        public IActionResult AccessDenied() => View("AccessDenied");

        // Profil Görüntüleme ve Güncelleme - GET
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var username = User.Identity?.Name;
            var user = await _userManager.FindByNameAsync(username!);

            if (user == null)
                return NotFound();

            var model = new UserProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Image = user.Image
            };

            return View(model);
        }

        // Profil Güncelleme - POST
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            var username = User.Identity?.Name;
            var user = await _userManager.FindByNameAsync(username!);

            if (user == null)
                return NotFound();

            user.Name = model.Name;

            // Profil fotoğrafı güncelleme
            if (model.ImageFile != null)
            {
                var extension = Path.GetExtension(model.ImageFile.FileName);
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

                if (!allowedExtensions.Contains(extension.ToLower()))
                {
                    TempData["Error"] = "Sadece .jpg, .jpeg, .png dosyalarına izin verilir.";
                    return View(model);
                }

                var imageName = Guid.NewGuid().ToString() + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", imageName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                user.Image = imageName;
            }

            // Şifre güncelleme
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);

                    return View(model);
                }
            }

            await _userManager.UpdateAsync(user);
            TempData["Success"] = "Profiliniz güncellendi.";
            return RedirectToAction("Profile");
        }
    }
}
