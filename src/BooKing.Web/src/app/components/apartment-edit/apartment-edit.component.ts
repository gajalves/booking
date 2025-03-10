import { Component } from '@angular/core';
import { ApartmentsService } from '../../services/apartments.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AddressDto, ApartmentDto } from '../../dtos/apartment.dto';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApartmentFormComponent } from '../apartment-form/apartment-form.component';
import { ErrorReturnDto } from '../../dtos/errorReturn.dto';

@Component({
  selector: 'app-apartment-edit',
  standalone: true,
  imports: [FormsModule,ReactiveFormsModule, ApartmentFormComponent, CommonModule],
  templateUrl: './apartment-edit.component.html',
  styleUrl: './apartment-edit.component.css'
})
export class ApartmentEditComponent {
  selectedApartment!: ApartmentDto;
  apartmentForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private apartmentsService: ApartmentsService,
    private route: ActivatedRoute,
    private router: Router,
    private toastService: ToastrService
  ){
    this.selectedApartment = history.state.selectedApartment;

    if(!this.selectedApartment)
      this.getApartment();
  }

  getApartment() {
    const apartmentId = this.route.snapshot.paramMap.get('id');
    if(!apartmentId)
      this.router.navigate(['profile/apartments']);

    this.apartmentsService.getApartmentDetail(apartmentId!).subscribe(
      {
        next: (resp) => {
          this.selectedApartment = resp.body.value as ApartmentDto;
        },
        error: (e) => {
          const ret = e.error as ErrorReturnDto;
          this.toastService.error(ret.name);
        }
      })
  }
}
