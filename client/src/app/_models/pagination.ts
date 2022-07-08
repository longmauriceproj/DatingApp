export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totaPages: number;
}

export class PaginatedResult<T> {
  result: T;
  pagination: Pagination;
}
