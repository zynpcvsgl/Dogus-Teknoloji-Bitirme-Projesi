using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity; // ApplicationUser için doğru namespace

namespace BlogApp.Entity
{
    public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;

    public bool IsActive { get; set; }
    public DateTime PublishedOn { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }

    public List<Comment> Comments { get; set; } = new List<Comment>();
    public List<Tag> Tags { get; set; } = new List<Tag>();  // Tags property here

    public Post() { }

    public Post(string title, string description, string content, string url, string image)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Content = content ?? throw new ArgumentNullException(nameof(content));
        Url = url ?? throw new ArgumentNullException(nameof(url));
        Image = image ?? throw new ArgumentNullException(nameof(image));
        PublishedOn = DateTime.Now;
        IsActive = true;
    }
}

}
