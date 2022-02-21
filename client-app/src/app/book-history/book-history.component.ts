import { Component, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BookHistoryService } from './book-history.service';
import { BookSearchInput } from '../book/model/book-search-input';
import { BookHistoryModel } from './model/paged-book-response';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-book-history',
  templateUrl: './book-history.component.html',
  styleUrls: ['./book-history.component.css']
})
export class BookHistoryComponent implements OnInit {
  public searchField: string
  public displayedColumns = [
    'id',
    'logDate',
    'field',
    'oldValue',
    'currentValue',
    'description'
  ];
  public filterDetails: BookSearchInput = <BookSearchInput>{
    currentPage: 1,
    orderby: null,
    pageSize: 10,
    searchFields: 'field,oldValue,currentValue',
    searchValue: ''
  }
  public bookId: number;
  public totalCount: number;
  public dataSource = new MatTableDataSource<BookHistoryModel>();

  constructor(
    public bookHistoryService: BookHistoryService, public route: ActivatedRoute
  ) { }


  public customSort = (event: Sort) => {
    this.filterDetails.currentPage = 1;
    this.filterDetails.orderby = `${event.active} ${event.direction}`;
    this._reloadTable();
  }

  public search($event: any) {
    this.filterDetails.currentPage = 1;
    this.filterDetails.searchValue = $event.target.value
    this._reloadTable();
  }


  public ngOnInit(): void {
    this.bookId = parseInt( this.route.snapshot.paramMap.get('id'));
    this._reloadTable();

  }

  public onPageChanged(event) {
    this.filterDetails.currentPage = event.Selected;
    this._reloadTable();

  }

  private _reloadTable(): void {
    this.bookHistoryService.get(this.bookId, this.filterDetails).subscribe((response) => {
      this.dataSource.data = response.records
      this.totalCount = response.totalSize;
    });
  }

}
