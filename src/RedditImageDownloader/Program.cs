using System.Text.Json;
using CommandLine;
using RedditImageDownloader.Models;

namespace RedditImageDownloader
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class Program
    {
        static Task Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .WithParsedAsync(async o =>
                {
                    var url = $"https://www.reddit.com/user/{o.Username}/submitted.json?limit=100";

                    var httpClient = new HttpClient
                    {
                        DefaultRequestHeaders =
                        {
                            {
                                "User-Agent",
                                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36"
                            }
                        }
                    };

                    var response = await httpClient.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();
                    var allPosts = JsonSerializer.Deserialize<RedditQuery>(content);

                    var allImages = from c in allPosts.Data.Children
                        where !string.IsNullOrWhiteSpace(c.Data.UrlOverriddenByDest)
                        where c.Data.UrlOverriddenByDest.HasImageFileExtension()
                        select new
                        {
                            Url = c.Data.UrlOverriddenByDest, FileName = Path.GetFileName(c.Data.UrlOverriddenByDest)
                        };

                    var outputDirectory = Path.Combine(o.Output, o.Username);
                    Directory.CreateDirectory(outputDirectory);

                    List<(string FilePath, Task<byte[]> DownloadTasks)> FileByteMappings = new();

                    foreach (var image in allImages)
                    {
                        var filePath = Path.Combine(outputDirectory, image.FileName);
                        var downloadTask = httpClient.GetByteArrayAsync(image.Url);
                        FileByteMappings.Add((filePath, downloadTask));
                    }

                    await Task.WhenAll(FileByteMappings.Select(x => x.DownloadTasks));

                    await Task.WhenAll(FileByteMappings.Select(x =>
                        File.WriteAllBytesAsync(x.FilePath, x.DownloadTasks.Result)));
                });
        }
    }
}