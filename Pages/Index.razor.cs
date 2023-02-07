using Microsoft.AspNetCore.Components;
using wasm_blog_engine.Models;
using wasm_blog_engine.Services;

namespace wasm_blog_engine.Pages;

public partial class Index
{
    [Inject] public IBlogPostsRetriever BlogPostsRetriever { get; set; }
    
    private List<BlogPost> Blogs = new();

    protected override async Task OnInitializedAsync()
    {
        Blogs = await BlogPostsRetriever.GetAll();
    }
}