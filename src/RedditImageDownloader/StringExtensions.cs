using System.Text;

namespace RedditImageDownloader;

static class StringExtensions
{
    internal static bool HasImageFileExtension(this string s)
    {
        return s.EndsWith(".jpg") || s.EndsWith(".jpeg") || s.EndsWith(".png") || s.EndsWith(".gif");
    }
}