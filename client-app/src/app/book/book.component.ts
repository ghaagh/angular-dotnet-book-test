import { Component, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BookService } from './book.service'
import { BookSearchInput } from './model/book-search-input';
import { BookResponse, PagedBookResponse } from './model/paged-book-response';
@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css']
})
export class BookComponent implements OnInit {

  constructor(bookService: BookService) {
    this._bookService = bookService;
    this.filterDetails = <BookSearchInput>{ currentPage: 1, orderby: null, pageSize: 10, searchFields: 'isbn,bookTitle,id', searchValue: '' }
  }
  private _bookService: BookService;
  displayedColumns = [
    'id',
    'bookTitle',
    'isbn',
    'actions'
  ];
  public totalCount: number;
  public dataSource = new MatTableDataSource<BookResponse>();
  public filterDetails: BookSearchInput;

  public customSort = (event: Sort) => {
    this.filterDetails.currentPage = 1;
    this.filterDetails.orderby = `${event.active} ${event.direction}`;
    this.reloadTable();
  }

  search($event: any) {
    console.log($event)
    console.log($event.target)
    this.filterDetails.currentPage = 1;
    this.filterDetails.searchValue = $event.target.value
    this.reloadTable();
  }

  ngOnInit(): void {
    this.reloadTable();
  }
  onPageChanged(event) {
    this.filterDetails.currentPage = event.Selected;
    this.reloadTable();

  }

  reloadTable(): void{
    this._bookService.get(this.filterDetails).subscribe((response) => {
      this.dataSource.data = response.records
      this.totalCount = response.totalSize;
    });
  }


}
