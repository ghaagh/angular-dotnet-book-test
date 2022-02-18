import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AuthorService } from './author.service'
import { AuthorSearchInput } from './model/author-search-input';
import { AuthorResponse } from '../book/model/paged-book-response';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-author',
  templateUrl: './author.component.html',
  styleUrls: ['./author.component.css']
})
export class AuthorComponent implements OnInit {


  constructor(
    public authorService: AuthorService,
    public dialog: MatDialog
  ) {
    this.filterDetails = <AuthorSearchInput>{ currentPage: 1, orderby: null, pageSize: 10, searchFields: 'name,id', searchValue: '' }
  }
  public searchField: string
  displayedColumns = [
    'id',
    'name',
    'actions'
  ];
  public totalCount: number;
  public dataSource = new MatTableDataSource<AuthorResponse>()
  public filterDetails: AuthorSearchInput

  public customSort = (event: Sort) => {
    this.filterDetails.currentPage = 1
    this.filterDetails.orderby = `${event.active} ${event.direction}`
    this.reloadTable();
  }

  search($event: any) {
    this.filterDetails.currentPage = 1;
    this.filterDetails.searchValue = $event.target.value
    this.reloadTable();
  }

  delete(id: number) {
    this.authorService.delete(id).subscribe(() => {
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
    this.authorService.get(this.filterDetails).subscribe((response) => {
      this.dataSource.data = response.records
      this.totalCount = response.totalSize;
    });
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreateAuthorDialog, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
    });
  }

}



@Component({
  selector: 'create-author-dialog',
  templateUrl: './create-author-dialog.html',
})
export class CreateAuthorDialog implements OnInit, OnDestroy {
  public createAuthorForm: FormGroup
  public authorSearchInput: FormControl = new FormControl();
  public authorsList: any[] = []

  protected _onDestroy = new Subject();

  constructor(
    private _formBuilder: FormBuilder,
    private _authorService: AuthorService,
    public dialogRef: MatDialogRef<CreateAuthorDialog>, @Inject(MAT_DIALOG_DATA) public data: any
  ) {


  }
  ngOnDestroy(): void {
    this._onDestroy.next();
    this._onDestroy.complete();
  }


  getFormValidationErrors() {
    Object.keys(this.createAuthorForm.controls).forEach(key => {
      const controlErrors: ValidationErrors = this.createAuthorForm.get(key).errors;
      if (controlErrors != null) {
        Object.keys(controlErrors).forEach(keyError => {
          console.log('Key control: ' + key + ', keyError: ' + keyError + ', err value: ', controlErrors[keyError]);
        });
      }
    });
  }
  ngOnInit(): void {
    this.createAuthorForm = this._formBuilder.group({
      name: new FormControl('', [Validators.maxLength(15), Validators.required]),
    });

  }
  onSubmit = () => {
    this._authorService.create(this.createAuthorForm.controls['name'].value).subscribe((response) => {
      this.dialogRef.close();
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
