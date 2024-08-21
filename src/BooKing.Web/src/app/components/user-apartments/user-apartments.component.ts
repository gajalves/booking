import { Component } from '@angular/core';
import { ApartmentDto } from '../../dtos/apartment.dto';
import { ApartmentsService } from '../../services/apartments.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IdentityService } from '../../services/identity.service';
import { ApartmentModalComponent } from '../apartment-modal/apartment-modal.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-apartments',
  standalone: true,
  imports: [ApartmentModalComponent, CommonModule],
  templateUrl: './user-apartments.component.html',
  styleUrl: './user-apartments.component.css'
})
export class UserApartmentsComponent {
  apartments: ApartmentDto[] = [];
  selectedApartment?: ApartmentDto;

  constructor(
    private apartmentsService: ApartmentsService,
    private router: Router,
    private toastService: ToastrService,
    private identityService: IdentityService,
  ) {
    this.loadUserApartments();
  }

  loadUserApartments(): void {
    const userId = this.identityService.getUserId();
    this.apartmentsService.getUserApartments(userId!).subscribe({
      next: (result) => {
        if (result.isSuccess) {
          this.apartments = result.value!;
        }
      },
      error: (err) => {
        this.toastService.error(err);
      }
    });
  }

  openDetailsModal(apartment: ApartmentDto): void {
    this.selectedApartment = apartment;
  }

  editApartment(apartment: ApartmentDto): void {
    this.router.navigate([`profile/apartments/${apartment.id}/edit`], { state: { selectedApartment: apartment } });
  }

  deleteApartment(apartmentId: string): void {
    //
  }

  newApartment(): void {
    this.router.navigate([`profile/apartments/new`]);
  }
}
