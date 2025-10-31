export interface Book {
  id: string;
  title: string;
  authors: string[];
  description: string;
  publishedDate: string;
  publisher: string;
  pageCount: number;
  thumbnail: string;
  previewLink: string;
  categories: string[];
  averageRating: number;
  ratingsCount: number;
}

export interface BookSearchResult {
  items: Book[];
  totalItems: number;
  currentPage: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface SearchParams {
  query: string;
  page: number;
  pageSize: number;
}