import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Book, BookSearchResult } from '../../models/book';
import { BookService } from '../../services/book.service';
import { LoadingService } from '../../services/loading.service';

@Component({
  standalone: false,
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  books: Book[] = [];
  searchResult: BookSearchResult | null = null;
  currentPage: number = 1;
  currentQuery: string = '';
  currentPageSize: number = 12; // MUDAR DE 20 PARA 12
  currentSearchType: string = 'all';
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
    console.log('Busca recebida:', params);
    this.currentQuery = params.query;
    this.currentPage = 1;
    this.currentPageSize = params.pageSize;
    this.currentSearchType = params.type;
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
        console.log('Resultado da busca:', result);
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
      this.performSearch(this.currentQuery, this.currentSearchType, this.currentPage, this.currentPageSize);
    }
  }

  previousPage() {
    if (this.searchResult?.hasPreviousPage) {
      this.currentPage--;
      this.performSearch(this.currentQuery, this.currentSearchType, this.currentPage, this.currentPageSize);
    }
  }

  viewBookDetails(bookId: string) {
    console.log('Navegando para livro ID:', bookId);
    
    if (bookId && bookId.trim() !== '') {
      this.router.navigate(['/book', bookId]);
    } else {
      console.error('ID do livro é inválido:', bookId);
      alert('Erro: ID do livro não encontrado.');
    }
  }

  handleImageError(event: any) {
    event.target.src = 'assets/no-cover.png';
  }

  // Método interno para busca inicial
  private searchBooks(query: string) {
    this.onSearch({ query: query, type: 'all', pageSize: 12 }); // MUDAR DE 20 PARA 12
  }
}