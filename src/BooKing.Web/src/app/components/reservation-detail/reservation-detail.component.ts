import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReservationDto } from '../../dtos/reservation.dto';
import { ReserveService } from '../../services/reserve.service';
import { ErrorReturnDto } from '../../dtos/errorReturn.dto';
import { ToastrService } from 'ngx-toastr';
import { ReservationEventsDto } from '../../dtos/reservationEvents.dto';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reservation-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './reservation-detail.component.html',
  styleUrl: './reservation-detail.component.css'
})
export class ReservationDetailComponent {
  reservationId: string | null;
  reservation!: ReservationDto;

  confirmLoading: boolean = false;
  cancelLoading: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private reservationService: ReserveService,
    private toastService: ToastrService
  ){
    this.reservationId = this.route.snapshot.paramMap.get('id');

    if(!this.reservationId)
      this.router.navigate([`profile/reservations`])

    this.getReservation(this.reservationId!);
  }



  private getReservation(reservationId: string): void {
    this.reservationService.getReservation(reservationId).subscribe(
      {
        next: (response) => {
          this.reservation = response.value as ReservationDto;
          this.getReservationEvents(this.reservationId!);
        },
        error: (e) => {
          const ret = e.error as ErrorReturnDto;
          this.toastService.error(ret.name);
          setTimeout(() => {
            this.router.navigate(['profile/reservations']);
          }, 300);
        }
      }
    )
  }

  private getReservationEvents(reservationId: string): void {
    this.reservationService.getReservationEvents(reservationId).subscribe(
      {
        next: (response) => {
          this.reservation.events = response.value as ReservationEventsDto[];
        },
        error: (e) => {
          const ret = e.error as ErrorReturnDto;
          this.toastService.error(ret.name);
          setTimeout(() => {
            this.router.navigate(['profile/reservations']);
          }, 300);
        }
      }
    )
  }

  confirmReservation(): void {
    this.confirmLoading = true;

    this.reservationService.confirm(this.reservationId!).subscribe(
      {
        next: (response) => {
          this.toastService.success("Successfully Confirmed");
          setTimeout(() => {
            this.confirmLoading = false;
            window.location.reload();
          }, 1000);

        },
        error: (e) => {
          const ret = e.error as ErrorReturnDto;
          this.toastService.error(ret.name);
          setTimeout(() => {
            this.router.navigate(['profile/reservations']);
            this.confirmLoading = false;
          }, 1000);
        }
      })
  }

  cancelReservation(): void {
    this.cancelLoading = true;

    this.reservationService.cancel(this.reservationId!).subscribe(
      {
        next: (response) => {
          this.toastService.success("Successfully Cancelled");
          setTimeout(() => {
            this.cancelLoading = false;
            window.location.reload();
          }, 1000);
        },
        error: (e) => {
          const ret = e.error as ErrorReturnDto;
          this.toastService.error(ret.name);
          setTimeout(() => {
            this.router.navigate(['profile/reservations']);
            this.cancelLoading = false;
          }, 1000);
        }
      })
  }

}
