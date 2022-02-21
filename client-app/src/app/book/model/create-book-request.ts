export interface CreateBookRequest {
  title: string;
  authorIds: number[];
  iSBN: string;
  publishedAt: string;
  description: string;
}
