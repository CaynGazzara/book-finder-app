import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  standalone: false,
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent {
  @Output() search = new EventEmitter<{ query: string, type: string, pageSize: number }>();

  searchQuery: string = '';
  searchType: string = 'all';
  pageSize: number = 12; // MUDAR DE 20 PARA 12
  showAdvanced: boolean = false;

  onSearch() {
    if (this.searchQuery.trim()) {
      console.log('Emitindo busca:', {
        query: this.searchQuery.trim(),
        type: this.searchType,
        pageSize: this.pageSize
      });
      
      this.search.emit({
        query: this.searchQuery.trim(),
        type: this.searchType,
        pageSize: this.pageSize
      });
    } else {
      console.log('Campo de busca vazio');
      alert('Por favor, digite algo para buscar.');
    }
  }

  toggleAdvanced() {
    this.showAdvanced = !this.showAdvanced;
  }
}