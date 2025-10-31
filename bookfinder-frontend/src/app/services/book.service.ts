import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book, BookSearchResult } from '../models/book';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private apiUrl = 'https://localhost:7000/api/books';

  constructor(private http: HttpClient) { }

  // MUDAR pageSize padrão de 20 para 12
  searchBooks(query: string, page: number = 1, pageSize: number = 12): Observable<BookSearchResult> {
    let params = new HttpParams()
      .set('query', query)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<BookSearchResult>(`${this.apiUrl}/search`, { params });
  }

  getBookById(id: string): Observable<Book> {
    return this.http.get<Book>(`${this.apiUrl}/${id}`);
  }

  // MUDAR pageSize padrão de 20 para 12
  searchByAuthor(author: string, page: number = 1, pageSize: number = 12): Observable<BookSearchResult> {
    let params = new HttpParams()
      .set('author', author)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<BookSearchResult>(`${this.apiUrl}/search/author`, { params });
  }

  // MUDAR pageSize padrão de 20 para 12
  searchByCategory(category: string, page: number = 1, pageSize: number = 12): Observable<BookSearchResult> {
    let params = new HttpParams()
      .set('category', category)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<BookSearchResult>(`${this.apiUrl}/search/category`, { params });
  }
}