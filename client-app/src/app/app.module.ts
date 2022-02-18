import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BidiModule } from '@angular/cdk/bidi';

import { AppComponent } from './app.component';
import { BookComponent, CreateBookDialog } from './book/book.component';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatTableModule } from '@angular/material/table';
import { HttpClientModule } from '@angular/common/http';
import { MatSortModule } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { PagerComponent } from './pager/pager.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialogModule } from '@angular/material/dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker'
import { MatNativeDateModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { AuthorComponent, CreateAuthorDialog } from './author/author.component';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    BookComponent,
    PagerComponent,
    CreateBookDialog,
    CreateAuthorDialog,
    AuthorComponent
  ],
  imports: [
    BrowserModule,
    MatTableModule,
    MatInputModule,
    HttpClientModule,
    MatSortModule,
    BidiModule,
    MatButtonModule,
    MatDialogModule,
    FormsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    MatIconModule,
    BrowserAnimationsModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
