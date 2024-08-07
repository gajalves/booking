export class BaseResultDto {
  value: object;
  items: object[];
  pageIndex: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean

  constructor(
    value: object,
    items: object[],
    pageIndex: number,
    totalPages: number,
    hasPreviousPage: boolean,
    hasNextPage: boolean
  ){
    this.value = value;
    this.items = items;
    this.pageIndex = pageIndex;
    this.totalPages = totalPages;
    this.hasPreviousPage = hasPreviousPage;
    this.hasNextPage = hasNextPage;
  }
}
