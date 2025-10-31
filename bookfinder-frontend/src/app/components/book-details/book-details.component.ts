import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Book } from '../../models/book';
import { BookService } from '../../services/book.service';
import { LoadingService } from '../../services/loading.service';

@Component({
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
    
    if (!bookId) {
      this.error = 'ID do livro não encontrado';
      this.loading = false;
      return;
    }

    this.loadingService.show();
    this.loading = true;

    this.bookService.getBookById(bookId).subscribe({
      next: (book) => {
        this.book = book;
        this.loading = false;
        this.loadingService.hide();
      },
      error: (error) => {
        console.error('Erro ao carregar detalhes do livro:', error);
        this.error = 'Erro ao carregar detalhes do livro';
        this.loading = false;
        this.loadingService.hide();
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  openPreview(): void {
    if (this.book?.previewLink) {
      window.open(this.book.previewLink, '_blank');
    }
  }

  handleImageError(event: any): void {
    event.target.src = 'assets/no-cover.png';
  }
}