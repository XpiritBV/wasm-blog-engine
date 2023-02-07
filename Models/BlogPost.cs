namespace wasm_blog_engine.Models;

public class BlogPost
{
    public string Title { get; set; }
    public string Excerpt { get; set; }
    public string ImageUrl { get; set; }
    public DateTime PublishDate { get; set; }
}