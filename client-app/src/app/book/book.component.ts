import { Component, Inject, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BookService } from './book.service'
import { BookSearchInput } from './model/book-search-input';
import { AuthorResponse, BookResponse } from './model/paged-book-response';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthorService } from '../author/author.service';
import { CreateBookRequest } from './model/create-book-request';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
})
export class BookComponent implements OnInit {
  public searchField: string
  public displayedColumns = [
    'id',
    'bookTitle',
    'isbn',
    'authors',
    'actions'
  ];
  public filterDetails: BookSearchInput = <BookSearchInput>{
    currentPage: 1,
    orderby: null,
    pageSize: 10,
    searchFields: 'isbn,bookTitle,id',
    searchValue: ''
  }
  public totalCount: number;
  public dataSource = new MatTableDataSource<BookResponse>();

  constructor(
    public bookService: BookService,
    public dialog: MatDialog
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

  public delete(id: number) {
    this.bookService.delete(id).subscribe(() => {
      this._reloadTable();
    });
  }

  public ngOnInit(): void {
    this._reloadTable();

  }

  public onPageChanged(event) {
    this.filterDetails.currentPage = event.Selected;
    this._reloadTable();

  }

  public openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateBookDialog, {
      width: '400px'
    });
    dialogRef.afterClosed().subscribe(result => {
    });
  }

  public openEditDialog(id: number): void {
    this.bookService.getById(id).subscribe((response) => {
      const dialogRef = this.dialog.open(CreateBookDialog, {
        width: '400px',
        data: response

      });
      dialogRef.afterClosed().subscribe(result => {
        if (result.saved)
          this._reloadTable();
      });
    });

  }

  private _reloadTable(): void {
    this.bookService.get(this.filterDetails).subscribe((response) => {
      this.dataSource.data = response.records
      this.totalCount = response.totalSize;
    });
  }
}


@Component({
  selector: 'create-new-book-dialog',
  templateUrl: './create-new-book-dialog.html',
})
export class CreateBookDialog implements OnInit {
  public createBookForm: FormGroup
  public authorSearchInput: FormControl = new FormControl();
  public authorsList: AuthorResponse[] = []
  public authorsDataSource: MatTableDataSource<AuthorResponse> = new MatTableDataSource<AuthorResponse>(this.authorsList)
  public filteredOptions: AuthorResponse[]
  public displayedColumns = ['name', 'action'];
  public get editMode(): boolean {
    return this.data != null;
  };
  constructor(
    public authorService: AuthorService,
    public bookService: BookService,
    private _formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<CreateBookDialog>, @Inject(MAT_DIALOG_DATA) public data: BookResponse
  ) {}

  public ngOnInit(): void {
    this.createBookForm = this._formBuilder.group({
      isbn: new FormControl('', [Validators.maxLength(15), Validators.required]),
      title: new FormControl('', [Validators.maxLength(50), Validators.required]),
      publishedAt: new FormControl('', [Validators.required])
    });
    if (this.editMode) {
      this.createBookForm.patchValue({
        isbn: this.data.isbn,
        title: this.data.title,
        publishedAt: this.data.publishedAt
      });
      this.authorsList = this.data.authors;
      this.authorsDataSource.data = this.authorsList;
    }
  }

  public searchOnAuthors($event) {
    if ($event.target.value.length < 2)
      return;
    this._filter($event.target.value);
  }

  public addToAuthor(id: number, name: string) {
    this.authorSearchInput.setValue('');
    this.authorsList.push({ id: id, name: name });
    this.authorsDataSource.data = this.authorsList;
  }

  public deleteAuthor(id: number) {
   
    this.authorsList = this.authorsList.filter(item =>  item.id != id );
    this.authorsDataSource.data = this.authorsList;
  }

  public onSubmit = () => {
    let model = <CreateBookRequest>{
      authorIds: this.authorsList.map(item => item.id),
      iSBN: this.createBookForm.controls['isbn'].value,
      publishedAt: this.createBookForm.controls['publishedAt'].value,
      title: this.createBookForm.controls['title'].value
    }
    if (!this.editMode) {
      this.bookService.create(model).subscribe(response => {
        this.dialogRef.close({ 'saved': true });
      }, (error) => { this.onNoClick() });
    }
    else {
      this.bookService.edit(this.data.id,model).subscribe(response => {
        this.dialogRef.close({ 'saved': true });
      }, (error) => { this.onNoClick() });
    }
  }

  private _filter(value: string): void {
    const filterValue = value.toLowerCase();
    if (filterValue.length < 3) {
      this.filteredOptions = [];
      return;
    }
    const authorSearchInput = <BookSearchInput>{ currentPage: 1, orderby: null, pageSize: 4, searchFields: 'name,id', searchValue: filterValue }
    this.authorService.get(authorSearchInput).subscribe(c => { this.filteredOptions = c.records });

  }

  onNoClick(): void {
    this.dialogRef.close({'saved':false});
  }
}
