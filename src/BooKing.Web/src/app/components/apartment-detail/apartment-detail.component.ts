import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApartmentsService } from '../../services/apartments.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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
  totalPrice!: number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apartmentService: ApartmentsService
  ) {
    const apartmentId = this.route.snapshot.paramMap.get('id');
    if(!apartmentId)
      this.router.navigate(['/']);

    this.apartmentService.getApartmentDetail(apartmentId!).subscribe(data => {
      this.apartment = data.body.value;
      console.log(data);
    });
   }

   makeReservation(): void {
    // Lógica para fazer a reserva, pode ser uma navegação para uma página de reserva ou uma chamada para um serviço de reserva
    alert('Reservation functionality is not implemented yet.');
  }

  calculateTotal(): void {
    if (this.checkInDate && this.checkOutDate) {
      const checkIn = new Date(this.checkInDate);
      const checkOut = new Date(this.checkOutDate);
      this.numberOfNights = (checkOut.getTime() - checkIn.getTime()) / (1000 * 3600 * 24);

      this.totalPrice = (this.apartment.price * this.numberOfNights) + this.apartment.cleaningFee + 87;
    } else {
      this.totalPrice = this.apartment.price + this.apartment.cleaningFee + 87;
    }
  }

  goBack(): void {
    this.router.navigate(['/']);
  }
}
