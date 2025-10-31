using System.Text.Json.Serialization;

namespace BookFinder.API.Models
{
    public class GoogleBooksApiResponse
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = string.Empty;

        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }

        [JsonPropertyName("items")]
        public List<VolumeItem> Items { get; set; } = new();
    }

    public class VolumeItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("volumeInfo")]
        public VolumeInfo VolumeInfo { get; set; } = new();
    }

    public class VolumeInfo
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("authors")]
        public List<string> Authors { get; set; } = new();

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("publishedDate")]
        public string PublishedDate { get; set; } = string.Empty;

        [JsonPropertyName("publisher")]
        public string Publisher { get; set; } = string.Empty;

        [JsonPropertyName("pageCount")]
        public int PageCount { get; set; }

        [JsonPropertyName("imageLinks")]
        public ImageLinks ImageLinks { get; set; } = new();

        [JsonPropertyName("previewLink")]
        public string PreviewLink { get; set; } = string.Empty;

        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; } = new();

        [JsonPropertyName("averageRating")]
        public double AverageRating { get; set; }

        [JsonPropertyName("ratingsCount")]
        public int RatingsCount { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; } = string.Empty;

        [JsonPropertyName("industryIdentifiers")]
        public List<IndustryIdentifier> IndustryIdentifiers { get; set; } = new();
    }

    public class ImageLinks
    {
        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; } = string.Empty;

        [JsonPropertyName("smallThumbnail")]
        public string SmallThumbnail { get; set; } = string.Empty;

        [JsonPropertyName("medium")]
        public string Medium { get; set; } = string.Empty;

        [JsonPropertyName("large")]
        public string Large { get; set; } = string.Empty;
    }

    public class IndustryIdentifier
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; } = string.Empty;
    }
}