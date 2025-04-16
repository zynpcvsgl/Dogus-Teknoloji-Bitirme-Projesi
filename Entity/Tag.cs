namespace BlogApp.Entity
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        // 'Text' özelliğini ekleyebilirsiniz.
        public string Text => Name; // Tag'in adını 'Text' olarak kullanabilirsiniz.

        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
