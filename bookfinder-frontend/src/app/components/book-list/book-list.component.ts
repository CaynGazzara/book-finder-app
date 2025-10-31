import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Book, BookSearchResult } from '../../models/book';
import { BookService } from '../../services/book.service';
import { LoadingService } from '../../services/loading.service';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  books: Book[] = [];
  searchResult: BookSearchResult | null = null;
  currentPage: number = 1;
  currentQuery: string = '';
  currentPageSize: number = 20;
  hasSearched: boolean = false;

  constructor(
    private bookService: BookService,
    private loadingService: LoadingService,
    private router: Router
  ) {}

  ngOnInit() {
    // Carregar alguns livros populares inicialmente
    this.searchBooks('best sellers');
  }

  onSearch(params: { query: string, type: string, pageSize: number }) {
    this.currentQuery = params.query;
    this.currentPage = 1;
    this.currentPageSize = params.pageSize;
    this.hasSearched = true;

    this.performSearch(params.query, params.type, 1, params.pageSize);
  }

  private performSearch(query: string, type: string, page: number, pageSize: number) {
    this.loadingService.show();

    let searchObservable;

    switch (type) {
      case 'author':
        searchObservable = this.bookService.searchByAuthor(query, page, pageSize);
        break;
      case 'category':
        searchObservable = this.bookService.searchByCategory(query, page, pageSize);
        break;
      default:
        searchObservable = this.bookService.searchBooks(query, page, pageSize);
    }

    searchObservable.subscribe({
      next: (result) => {
        this.searchResult = result;
        this.books = result.items;
        this.currentPage = result.currentPage;
        this.loadingService.hide();
      },
      error: (error) => {
        console.error('Erro na busca:', error);
        this.loadingService.hide();
        alert('Erro ao buscar livros. Tente novamente.');
      }
    });
  }

  nextPage() {
    if (this.searchResult?.hasNextPage) {
      this.currentPage++;
      this.performSearch(this.currentQuery, 'all', this.currentPage, this.currentPageSize);
    }
  }

  previousPage() {
    if (this.searchResult?.hasPreviousPage) {
      this.currentPage--;
      this.performSearch(this.currentQuery, 'all', this.currentPage, this.currentPageSize);
    }
  }

  viewBookDetails(bookId: string) {
    this.router.navigate(['/book', bookId]);
  }

  handleImageError(event: any) {
    event.target.src = 'assets/no-cover.png';
  }

  searchBooks(query: string) {
    this.onSearch({ query: query, type: 'all', pageSize: 20 });
  }
}