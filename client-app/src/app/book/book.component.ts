import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BookService } from './book.service'
import { BookSearchInput } from './model/book-search-input';
import { BookResponse } from './model/paged-book-response';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { map, startWith } from 'rxjs/operators';
import { Observable, Subject } from 'rxjs';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
 })
export class BookComponent implements OnInit {

  constructor(
    public bookService: BookService,
    public dialog: MatDialog
  ) {
    this.filterDetails = <BookSearchInput>{ currentPage: 1, orderby: null, pageSize: 10, searchFields: 'isbn,bookTitle,id', searchValue: '' }
  }
  isbn:string
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
    this.filterDetails.currentPage = 1;
    this.filterDetails.searchValue = $event.target.value
    this.reloadTable();
  }

  delete(id: number) {
    this.bookService.delete(id).subscribe(() => {
      this.reloadTable();
    });
  }

  ngOnInit(): void {
    this.reloadTable();

  }

  onPageChanged(event) {
    this.filterDetails.currentPage = event.Selected;
    this.reloadTable();

  }

  reloadTable(): void {
    this.bookService.get(this.filterDetails).subscribe((response) => {
      this.dataSource.data = response.records
      this.totalCount = response.totalSize;
    });
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateBookDialog, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
    });
  }

}
@Component({
  selector: 'create-new-book-dialog',
  templateUrl: './create-new-book-dialog.html',
})
export class CreateBookDialog implements OnInit, OnDestroy {
  public createBookForm: FormGroup
  public authorSearchInput: FormControl = new FormControl();
  public authorsList: any[] = []

  protected _onDestroy = new Subject();

  constructor(
    private _formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<CreateBookDialog>, @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnDestroy(): void {
    this._onDestroy.next();
    this._onDestroy.complete();
  }

  getFormValidationErrors() {
    Object.keys(this.createBookForm.controls).forEach(key => {
      const controlErrors: ValidationErrors = this.createBookForm.get(key).errors;
      if (controlErrors != null) {
        Object.keys(controlErrors).forEach(keyError => {
          console.log('Key control: ' + key + ', keyError: ' + keyError + ', err value: ', controlErrors[keyError]);
        });
      }
    });
  }

  ngOnInit(): void {
    this.createBookForm = this._formBuilder.group({
      isbn: new FormControl('', [Validators.maxLength(15), Validators.required]),
      title: new FormControl('', [Validators.maxLength(50), Validators.required]),
      publishedAt: new FormControl('', [Validators.required]),
      authors: new FormControl('', [Validators.required])
    });
    this.filteredOptions = this.authorSearchInput.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value)),
    );
  
  }

  onSubmit = () => {
   
  }

  options: string[] = ['One', 'Two', 'Three'];

  filteredOptions: Observable<string[]>;

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
