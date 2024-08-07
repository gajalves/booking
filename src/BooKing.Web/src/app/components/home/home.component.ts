import { Component } from '@angular/core';
import { ApartmentListComponent } from '../apartment-list/apartment-list.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ApartmentListComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  constructor(
    private router: Router
  ){

  }
  profileButtonClick() {
    this.router.navigate(['/profile/user'])
  }
}
