import { Component } from '@angular/core';
import { ApartmentDto } from '../../dtos/apartment.dto';
import { ApartmentsService } from '../../services/apartments.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IdentityService } from '../../services/identity.service';
import { ApartmentModalComponent } from '../apartment-modal/apartment-modal.component';
import { CommonModule } from '@angular/common';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
import { ConfirmationDialogService } from '../confirmation-dialog/confirmation-dialog.service';
import { ErrorReturnDto } from '../../dtos/errorReturn.dto';

@Component({
  selector: 'app-user-apartments',
  standalone: true,
  imports: [ApartmentModalComponent, CommonModule, ConfirmationDialogComponent],
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
    private confirmationDialogService: ConfirmationDialogService
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
        const ret = err.error as ErrorReturnDto;
        this.toastService.error(ret.name);
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
    this.apartmentsService.deleteApartment(apartmentId).subscribe({
      next: (result) => {
        if (result.isSuccess) {
          this.toastService.success(result.value);
          setTimeout(() => {
            window.location.reload();
          }, 500);
        }
      },
      error: (err) => {
        const ret = err.error as ErrorReturnDto;
        this.toastService.error(ret.name);
      }
    });
  }

  newApartment(): void {
    this.router.navigate([`profile/apartments/new`]);
  }

  openConfirmationDialog(apartmentId: string) {
    this.confirmationDialogService.confirm('Please confirm', 'Do you really want to delete this apartment ?')
    .then((result) => {
      if(result) {
        this.deleteApartment(apartmentId)
      }
    })
    .catch(((err) => console.log(err)));
  }

  changeIsActive(apartmentId: string, ev: Event) {
    const input = ev.target as HTMLInputElement
    this.apartmentsService.updateIsActive(apartmentId, input.checked).subscribe({
      next: () => {
        this.toastService.success('Apartment status updated');
      },
      error: (err) => {
        const ret = err.error as ErrorReturnDto;
        this.toastService.error(ret.name);
      }
    });
  }
}
