import { Component } from '@angular/core';
import { ApartmentListComponent } from '../apartment-list/apartment-list.component';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ApartmentListComponent, CommonModule, FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  searchTerm: string = '';

  constructor(
    private router: Router
  ){

  }
  profileButtonClick() {
    this.router.navigate(['/profile/user'])
  }

  onSearch(event: any) {
    this.searchTerm = event.target.value;
  }
}
