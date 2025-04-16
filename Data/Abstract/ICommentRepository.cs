using BlogApp.Entity;
using System.Linq;

namespace BlogApp.Data.Abstract
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }

        void CreateComment(Comment comment);

        void RemoveComment(Comment comment); // Yorum silme metodu

        void Save(); // Değişiklikleri veritabanına kaydet
    }
}
