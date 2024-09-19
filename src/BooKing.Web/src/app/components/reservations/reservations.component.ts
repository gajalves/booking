import { Component } from '@angular/core';
import { ReservationDto } from '../../dtos/reservation.dto';
import { ReserveService } from '../../services/reserve.service';
import { IdentityService } from '../../services/identity.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ErrorReturnDto } from '../../dtos/errorReturn.dto';

@Component({
  selector: 'app-reservations',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './reservations.component.html',
  styleUrl: './reservations.component.css'
})
export class ReservationsComponent {
  reservations: ReservationDto[] = [];

  constructor(
    private reservationService: ReserveService,
    private identityService: IdentityService,
    private toastService: ToastrService,
    private router: Router
  ) {
    this.getUserReservation();
  }


  getUserReservation() {
    const userId = this.identityService.getUserId();
    this.reservationService.getUserReservations(userId!).subscribe(
      {
        next: (response) => {
          this.reservations = response.value as ReservationDto[];
        },
        error: (e) => {
          const ret = e.error as ErrorReturnDto;
          this.toastService.error(ret.name);
        }
      }
    );
  }

  viewApartmentClick(apartmentId: string) {
    this.router.navigate([`/details/${apartmentId}`]);
  }

  navigateToReservationDetails(reservationId: string) {
    this.router.navigate([`profile/reservations/${reservationId}/details`], { state: { reservation: this.reservations.filter(r => r.id == reservationId) } });
  }
}
