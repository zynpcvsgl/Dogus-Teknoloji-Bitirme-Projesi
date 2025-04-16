using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity; // ApplicationUser için doğru namespace

namespace BlogApp.Entity
{
    public class Comment
    {
        public int CommentId { get; set; }

        public string Text { get; set; } = string.Empty;

        public DateTime PublishedOn { get; set; }

        public int PostId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public Post? Post { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public Comment() { }

        public Comment(string text, Post post, ApplicationUser user)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Post = post ?? throw new ArgumentNullException(nameof(post));
            User = user ?? throw new ArgumentNullException(nameof(user));
            PublishedOn = DateTime.Now;
        }
    }
}
