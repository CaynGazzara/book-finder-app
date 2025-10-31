using System.Net;
using BookFinder.API.Models;
using System.Text.Json;

namespace BookFinder.API.Services
{
    public class GoogleBooksService : IBookService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GoogleBooksService> _logger;

        public GoogleBooksService(HttpClient httpClient, ILogger<GoogleBooksService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<BookSearchResult> SearchBooksAsync(string query, int page = 1, int pageSize = 12) // MUDAR DE 20 PARA 12
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    throw new ArgumentException("Search query cannot be empty");

                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 40) pageSize = 12; // MUDAR DE 20 PARA 12

                var startIndex = (page - 1) * pageSize;

                _logger.LogInformation($"Searching books with query: {query}, page: {page}, pageSize: {pageSize}");

                var response = await _httpClient.GetAsync($"volumes?q={WebUtility.UrlEncode(query)}&startIndex={startIndex}&maxResults={pageSize}&printType=books");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Google Books API returned status code: {response.StatusCode}");
                    throw new HttpRequestException($"API request failed with status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<GoogleBooksApiResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (apiResponse == null)
                {
                    _logger.LogWarning("Failed to deserialize API response");
                    return new BookSearchResult { TotalItems = 0, CurrentPage = page, TotalPages = 0 };
                }

                var result = MapToBookSearchResult(apiResponse, page, pageSize);
                _logger.LogInformation($"Found {result.TotalItems} books, returning {result.Items.Count} items");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching books");
                throw;
            }
        }

        public async Task<Book> GetBookByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    throw new ArgumentException("Book ID cannot be empty");

                _logger.LogInformation($"Fetching book with ID: {id}");

                var response = await _httpClient.GetAsync($"volumes/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogWarning($"Book with ID {id} not found");
                        return null;
                    }

                    throw new HttpRequestException($"API request failed with status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var volumeItem = JsonSerializer.Deserialize<VolumeItem>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (volumeItem == null)
                {
                    _logger.LogWarning("Failed to deserialize book data");
                    return null;
                }

                return MapToBook(volumeItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching book with ID: {id}");
                throw;
            }
        }

        public async Task<BookSearchResult> SearchByAuthorAsync(string author, int page = 1, int pageSize = 12) // MUDAR DE 20 PARA 12
        {
            return await SearchBooksAsync($"inauthor:{author}", page, pageSize);
        }

        public async Task<BookSearchResult> SearchByCategoryAsync(string category, int page = 1, int pageSize = 12) // MUDAR DE 20 PARA 12
        {
            return await SearchBooksAsync($"subject:{category}", page, pageSize);
        }

        private BookSearchResult MapToBookSearchResult(GoogleBooksApiResponse apiResponse, int currentPage, int pageSize)
        {
            var books = apiResponse.Items?
                .Where(item => item.VolumeInfo != null)
                .Select(MapToBook)
                .ToList() ?? new List<Book>();

            var totalPages = pageSize > 0 ? (int)Math.Ceiling((double)apiResponse.TotalItems / pageSize) : 0;

            return new BookSearchResult
            {
                Items = books,
                TotalItems = apiResponse.TotalItems,
                CurrentPage = currentPage,
                TotalPages = totalPages
            };
        }

        private Book MapToBook(VolumeItem item)
        {
            var volumeInfo = item.VolumeInfo;

            return new Book
            {
                Id = item.Id,
                Title = volumeInfo.Title ?? "No title available",
                Authors = volumeInfo.Authors ?? new List<string> { "Unknown Author" },
                Description = volumeInfo.Description ?? "No description available",
                PublishedDate = volumeInfo.PublishedDate ?? "Unknown date",
                Publisher = volumeInfo.Publisher ?? "Unknown publisher",
                PageCount = volumeInfo.PageCount,
                Thumbnail = volumeInfo.ImageLinks?.Thumbnail
                         ?? volumeInfo.ImageLinks?.SmallThumbnail
                         ?? "/images/no-cover.png",
                PreviewLink = volumeInfo.PreviewLink ?? "#",
                Categories = volumeInfo.Categories ?? new List<string>(),
                AverageRating = volumeInfo.AverageRating,
                RatingsCount = volumeInfo.RatingsCount
            };
        }
    }
}