using BlogApp.Entity;
using System.Collections.Generic;

namespace BlogApp.Data.Abstract
{
    public interface IPostRepository
    {
        // posts koleksiyonunu ekledik
        IQueryable<Post> Posts { get; }   // IQueryable yerine List ya da IQueryable olarak kullanabilirsiniz.
        void CreatePost(Post post);
        Post GetPostById(int id);
        void EditPost(Post post);
    }
}
