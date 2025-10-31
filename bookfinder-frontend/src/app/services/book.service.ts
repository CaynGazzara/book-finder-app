import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book, BookSearchResult } from '../models/book';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private apiUrl = '/api/books';

  constructor(private http: HttpClient) { }

  searchBooks(query: string, page: number = 1, pageSize: number = 20): Observable<BookSearchResult> {
    let params = new HttpParams()
      .set('query', query)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<BookSearchResult>(`${this.apiUrl}/search`, { params });
  }

  getBookById(id: string): Observable<Book> {
    return this.http.get<Book>(`${this.apiUrl}/${id}`);
  }

  searchByAuthor(author: string, page: number = 1, pageSize: number = 20): Observable<BookSearchResult> {
    let params = new HttpParams()
      .set('author', author)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<BookSearchResult>(`${this.apiUrl}/search/author`, { params });
  }

  searchByCategory(category: string, page: number = 1, pageSize: number = 20): Observable<BookSearchResult> {
    let params = new HttpParams()
      .set('category', category)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<BookSearchResult>(`${this.apiUrl}/search/category`, { params });
  }
}