export interface PagedBookResponse {
  totalSize: number
  currentPage: number
  pageSize: number
  records: BookResponse[]
}

export interface BookResponse {
  id: number;
  title: string;
  isbn: string;
  publishedAt: string;
  description: string;
  authors: AuthorResponse[];
}

export interface AuthorResponse {
  id: number;
  name: string;
}
