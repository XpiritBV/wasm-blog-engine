namespace wasm_blog_engine.Pages;

public partial class Index
{
    string markdownContent = "";

    protected override async Task OnInitializedAsync()
    {
#if DEBUG
        // Get raw markdown from a file
        var client = new HttpClient();

        // DEMO 1
        var rawMarkdown = await client.GetStringAsync("https://localhost:5001/blogs/helloworld.md");
        if (rawMarkdown == null) { return; }

        var openTag = rawMarkdown.IndexOf('{');
        var closeTag = rawMarkdown.IndexOf('}');

        // Check formatting (simple)
        if (openTag == -1 || closeTag == -1) { return; }
        if (openTag > 0 || closeTag < 1) { return; }

        var metadataString = rawMarkdown.Substring(openTag, closeTag + 1);
        var metadata = System.Text.Json.JsonSerializer.Deserialize<Metadata>(metadataString);

        var restOfTheContent = rawMarkdown.Substring(closeTag + 1 + 1);

        markdownContent = restOfTheContent;
#endif
    }

    class Metadata
    {
        public string title { get; set; }
        public string author { get; set; }
        public string publishDate { get; set; }
    }
}