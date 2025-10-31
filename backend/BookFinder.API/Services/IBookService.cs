using BookFinder.API.Models;

namespace BookFinder.API.Services
{
    public interface IBookService
    {
        Task<BookSearchResult> SearchBooksAsync(string query, int page = 1, int pageSize = 12);
        Task<Book> GetBookByIdAsync(string id);
        Task<BookSearchResult> SearchByAuthorAsync(string author, int page = 1, int pageSize = 12); 
        Task<BookSearchResult> SearchByCategoryAsync(string category, int page = 1, int pageSize = 12); 
    }
}