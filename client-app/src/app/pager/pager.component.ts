import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ChangePageEventModel } from './change-page-event.model'

@Component({
  selector: 'pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {
  private _totalCount = new BehaviorSubject<number>(0);
  @Input()
  set totalCount(value) {
    this._totalCount.next(value);
  };
  get totalCount() {
    return this._totalCount.getValue();
  }

  private _pageSize = new BehaviorSubject<number>(10);
  @Input()
  set pageSize(value) {
    this._pageSize.next(value);
  };
  get pageSize() {
    return this._pageSize.getValue();
  }

  private _selected = new BehaviorSubject<number>(10);
  @Input()
  set selected(value) {
    this._selected.next(value);
  };
  get selected() {
    return this._selected.getValue();
  }

  @Input() middlePartPagerCount: number = 3;
  @Output() public onPageChanged = new EventEmitter<ChangePageEventModel>();
  pager: number[];
  ngOnInit(): void {

    this._totalCount.subscribe(x => {

      if (x && this._pageSize && this._selected) {
        this.loadPager();
      }
    });

    this._pageSize.subscribe(x => {

      if (x && this._totalCount && this._selected) {
        this.loadPager();

      }
    });
    this._selected.subscribe(x => {

      if (x && this._totalCount && this._pageSize) {
        this.loadPager();

      }
    });

  }

  goToPage = (pageNumber: number) => {
    this.selected = pageNumber;
    this.loadPager();
    this.onPageChanged.emit({ Selected: this.selected, TotalCount: this.totalCount });
  }
  isLastPage(): boolean {
    return this.selected == Math.floor(this.totalCount / this.pageSize);
  }

  private loadPager = () => {
    this.clearPagers();
    let pageCount = Math.floor(this.totalCount / this.pageSize);
    if (pageCount <= 5) {
      for (let index = 0; index < pageCount; index++) {
        this.pager.push(index + 1);
      }
    }
    else {
      this.addToPager(1, 0, this.selected == 1 ? 2 : 0);
      if (this.selected != 1 && this.selected != pageCount)
        this.addToPager(this.selected, 1, 1);
      this.addToPager(pageCount, this.selected == pageCount ? 2 : 0, 0);

    }
  }

  private addToPager(selectedNumber: number, prevCount: number, nextCount: number) {
    for (let index = prevCount; index >= 1; index--) {
      if (this.pager.indexOf(selectedNumber - index) == -1)
        this.pager.push(selectedNumber - index);
    }

    if (this.pager.indexOf(selectedNumber) == -1) {
      this.pager.push(selectedNumber);
    }

    for (let index = 1; index <= nextCount; index++) {
      if (this.pager.indexOf(selectedNumber + index) == -1)
        this.pager.push(selectedNumber + index);
    }
  }

  private clearPagers = () => {
    this.pager = [];
  }

}
