import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Book } from '../../models/book';
import { BookService } from '../../services/book.service';
import { LoadingService } from '../../services/loading.service';

@Component({
  standalone: false,
  selector: 'app-book-details',
  templateUrl: './book-details.component.html',
  styleUrls: ['./book-details.component.css']
})
export class BookDetailsComponent implements OnInit {
  book: Book | null = null;
  loading: boolean = true;
  error: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private bookService: BookService,
    private loadingService: LoadingService
  ) {}

  ngOnInit(): void {
    this.loadBookDetails();
  }

  loadBookDetails(): void {
    const bookId = this.route.snapshot.paramMap.get('id');
    
    console.log('Carregando detalhes do livro ID:', bookId);
    
    if (!bookId) {
      this.error = 'ID do livro não encontrado na URL';
      this.loading = false;
      return;
    }

    this.loadingService.show();
    this.loading = true;

    this.bookService.getBookById(bookId).subscribe({
      next: (book) => {
        console.log('Livro carregado:', book);
        this.book = book;
        this.loading = false;
        this.loadingService.hide();
      },
      error: (error) => {
        console.error('Erro ao carregar detalhes do livro:', error);
        this.error = 'Erro ao carregar detalhes do livro. Tente novamente.';
        this.loading = false;
        this.loadingService.hide();
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  openPreview(): void {
    if (this.book?.previewLink && this.book.previewLink !== '#') {
      window.open(this.book.previewLink, '_blank');
    } else {
      alert('Preview não disponível para este livro.');
    }
  }

  handleImageError(event: any): void {
    event.target.src = 'assets/no-cover.png';
  }

  // Método para formatar a lista de autores
  getAuthorsString(): string {
    return this.book?.authors?.join(', ') || 'Autor desconhecido';
  }

  // Método para formatar categorias
  getCategoriesString(): string {
    return this.book?.categories?.join(', ') || 'Nenhuma categoria informada';
  }
}