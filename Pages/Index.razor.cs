namespace wasm_blog_engine.Pages;

public partial class Index
{
    string markdownContent = "";

    protected override async Task OnInitializedAsync()
    {
        // Get raw markdown from a file
        var client = new HttpClient();

        // DEMO 1
        var rawMarkdown = await client.GetStringAsync("https://localhost:5001/blogs/helloworld.md");
        if (rawMarkdown == null) { return; }

        markdownContent = rawMarkdown;
    }
}