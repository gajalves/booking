import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApartmentsService } from '../../services/apartments.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReserveService } from '../../services/reserve.service';
import { ToastrService } from 'ngx-toastr';
import { ErrorReturnDto } from '../../dtos/errorreturn.dto';

@Component({
  selector: 'app-apartment-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './apartment-detail.component.html',
  styleUrl: './apartment-detail.component.css'
})
export class ApartmentDetailComponent {
  apartment: any;
  checkInDate!: string;
  checkOutDate!: string;
  numberOfNights: number = 1;
  totalPrice: number = 0;

  loading: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apartmentService: ApartmentsService,
    private reserveService: ReserveService,
    private toastService: ToastrService
  ) {
    const apartmentId = this.route.snapshot.paramMap.get('id');
    if(!apartmentId)
      this.router.navigate(['/']);

    this.apartmentService.getApartmentDetail(apartmentId!).subscribe(data => {
      this.apartment = data.body.value;
    });

    this.calculateTotal();
   }

   makeReservation(): void {
    alert('Reservation functionality is not implemented yet.');
  }

  calculateTotal(): void {
    if (this.checkInDate && this.checkOutDate) {
      const checkIn = new Date(this.checkInDate);
      const checkOut = new Date(this.checkOutDate);
      this.numberOfNights = (checkOut.getTime() - checkIn.getTime()) / (1000 * 3600 * 24);

      this.totalPrice = (this.apartment.price * this.numberOfNights) + this.apartment.cleaningFee;
     }
  }

  reserve() {
    this.loading = true;

    const checkIn =  new Date(this.checkInDate).toJSON();
    const checkOut =  new Date(this.checkOutDate).toJSON();

    this.reserveService.reserve(this.apartment.id, checkIn, checkOut).subscribe({
      next: () => {
        this.toastService.success("Reservation created successfully!")
        setTimeout(() =>
        {
            this.router.navigate(['/profile/reservations']);
            this.loading = false;
        },
        3000);
      },
      error: (e) => {
        const ret = e.error as ErrorReturnDto;
        this.toastService.error(ret.name);
        this.loading = false;
      }
    })
  }

  goHome(): void {
    this.router.navigate(['/']);
  }

  goProfile(): void {
    this.router.navigate(['/profile/user']);
  }
}
