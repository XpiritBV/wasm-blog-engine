using System.Globalization;
using System.Net.Http.Json;
using wasm_blog_engine.Models;

namespace wasm_blog_engine.Services;

public class BlogPostsRetriever : IBlogPostsRetriever
{
    private readonly IConfiguration configuration;

    public BlogPostsRetriever(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<List<BlogPost>> GetAll()
    {
        List<BlogPost> blogPosts = new List<BlogPost>();

        // Get raw markdown from a file
        var client = new HttpClient();

        // Build the API URL
        // https://api.github.com/repos/xpiritbv/wasm-blog-engine/contents/blogs?ref=gh-pages
        // Todo: Make this matching with config and local repository
        string url = $"https://api.github.com/repos/{configuration["github:repositoryName"]}/contents/blogs?ref={configuration["github:pagesBranch"]}";

        // Make the GET request
        var blogMarkdownFiles = await client.GetFromJsonAsync<List<BlogMarkdownFile>>(url);

        if (blogMarkdownFiles == null)
            return new List<BlogPost>();

        foreach (var blogMarkdownFile in blogMarkdownFiles)
        {
            var rawMarkdown = await client.GetStringAsync(blogMarkdownFile.download_url);
            if (string.IsNullOrWhiteSpace(rawMarkdown))
            {
                continue;
            }

            var openTag = rawMarkdown.IndexOf('{');
            var closeTag = rawMarkdown.IndexOf('}');

            // Check formatting (simple)
            if (openTag == -1 || closeTag == -1)
            {
                continue;
            }

            if (openTag > 0 || closeTag < 1)
            {
                continue;
            }

            var metadataString = rawMarkdown.Substring(openTag, closeTag + 1);
            var metadata = System.Text.Json.JsonSerializer.Deserialize<MetaData>(metadataString);

            if (metadata == null)
                continue;

            blogPosts.Add(new BlogPost()
            {
                Excerpt = metadata.excerpt,
                Title = metadata.title,
                ImageUrl = metadata.imageUrl,
                PublishDate = DateTime.ParseExact(metadata.publishDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)
            });

            // Not used in this summary
            var restOfTheContent = rawMarkdown.Substring(closeTag + 1 + 1);
        }

        return blogPosts.OrderByDescending(x => x.PublishDate).ToList();
    }
}

public interface IBlogPostsRetriever
{
    Task<List<BlogPost>> GetAll();
}