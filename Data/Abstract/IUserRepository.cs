using BlogApp.Entity;
using System.Linq;

namespace BlogApp.Data.Abstract
{
    public interface IUserRepository
    {
        IQueryable<ApplicationUser> Users { get; } // Users özelliğini ekleyin
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task CreateUserAsync(ApplicationUser user, string password);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
    }
}
