using CommandLine;

namespace RedditImageDownloader
{
    public class Options
    {
        [Option('u', "username", Required = true, HelpText = "Reddit username.")]
        public string Username { get; set; }
        
        [Option('o', "output", Required = true, HelpText = "Output directory.")]
        public string Output { get; set; }
    }
}