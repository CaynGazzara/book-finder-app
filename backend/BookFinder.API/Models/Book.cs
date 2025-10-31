using System.Text.Json.Serialization;

namespace BookFinder.API.Models
{
    public class Book
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

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

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; } = string.Empty;

        [JsonPropertyName("previewLink")]
        public string PreviewLink { get; set; } = string.Empty;

        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; } = new();

        [JsonPropertyName("averageRating")]
        public double AverageRating { get; set; }

        [JsonPropertyName("ratingsCount")]
        public int RatingsCount { get; set; }
    }

    public class BookSearchResult
    {
        [JsonPropertyName("items")]
        public List<Book> Items { get; set; } = new();

        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }

        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage => CurrentPage < TotalPages;

        [JsonPropertyName("hasPreviousPage")]
        public bool HasPreviousPage => CurrentPage > 1;
    }
}