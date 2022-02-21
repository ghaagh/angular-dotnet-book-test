export interface BookHistoryModel {
  id: number;
  logDate: string;
  oldValue: string | null;
  currentValue: string;
  field: string;
  description: string;
  bookId: number;
}
export interface PagedBookHistoryResponse {
  totalSize: number
  currentPage: number
  pageSize: number
  records: BookHistoryModel[]
}
