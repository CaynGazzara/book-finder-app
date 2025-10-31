import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent {
  @Output() search = new EventEmitter<{ query: string, type: string, pageSize: number }>();

  searchQuery: string = '';
  searchType: string = 'all';
  pageSize: number = 20;
  showAdvanced: boolean = false;

  onSearch() {
    if (this.searchQuery.trim()) {
      this.search.emit({
        query: this.searchQuery.trim(),
        type: this.searchType,
        pageSize: this.pageSize
      });
    }
  }

  toggleAdvanced() {
    this.showAdvanced = !this.showAdvanced;
  }
}