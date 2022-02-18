
import { AuthorResponse } from '../../book/model/paged-book-response'

export interface PagedAuthorResponse {
  totalSize: number
  currentPage: number
  pageSize: number
  records: AuthorResponse[]
}

