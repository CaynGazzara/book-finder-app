using BookFinder.API.Models;
using BookFinder.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookFinder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<ActionResult<BookSearchResult>> SearchBooks(
            [FromQuery] string query,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12) // MUDAR DE 20 PARA 12
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest("Search query is required");
                }

                _logger.LogInformation($"Search request - Query: {query}, Page: {page}, PageSize: {pageSize}");

                var result = await _bookService.SearchBooksAsync(query, page, pageSize);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling Google Books API");
                return StatusCode(503, "Service temporarily unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during book search");
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest("Book ID is required");
                }

                _logger.LogInformation($"Get book request - ID: {id}");

                var book = await _bookService.GetBookByIdAsync(id);

                if (book == null)
                {
                    return NotFound($"Book with ID {id} not found");
                }

                return Ok(book);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling Google Books API");
                return StatusCode(503, "Service temporarily unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching book details");
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpGet("search/author")]
        public async Task<ActionResult<BookSearchResult>> SearchByAuthor(
            [FromQuery] string author,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12) // MUDAR DE 20 PARA 12
        {
            try
            {
                if (string.IsNullOrWhiteSpace(author))
                {
                    return BadRequest("Author name is required");
                }

                _logger.LogInformation($"Search by author request - Author: {author}, Page: {page}, PageSize: {pageSize}");

                var result = await _bookService.SearchByAuthorAsync(author, page, pageSize);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling Google Books API");
                return StatusCode(503, "Service temporarily unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during author search");
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpGet("search/category")]
        public async Task<ActionResult<BookSearchResult>> SearchByCategory(
            [FromQuery] string category,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12) // MUDAR DE 20 PARA 12
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category))
                {
                    return BadRequest("Category is required");
                }

                _logger.LogInformation($"Search by category request - Category: {category}, Page: {page}, PageSize: {pageSize}");

                var result = await _bookService.SearchByCategoryAsync(category, page, pageSize);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling Google Books API");
                return StatusCode(503, "Service temporarily unavailable. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during category search");
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
        }
    }
}