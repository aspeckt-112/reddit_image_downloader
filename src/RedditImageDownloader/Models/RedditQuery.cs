namespace RedditImageDownloader.Models;

public record Child([property: JsonPropertyName("kind")] string Kind, [property: JsonPropertyName("data")] Data Data);

public record Data([property: JsonPropertyName("children")]
    IReadOnlyList<Child> Children, [property: JsonPropertyName("url_overridden_by_dest")]
    string UrlOverriddenByDest);

public record RedditQuery([property: JsonPropertyName("kind")] string Kind,
    [property: JsonPropertyName("data")] Data Data);