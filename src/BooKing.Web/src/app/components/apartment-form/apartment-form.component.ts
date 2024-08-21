import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApartmentDto } from '../../dtos/apartment.dto';
import { ApartmentsService } from '../../services/apartments.service';
import { AmenityDto } from '../../dtos/amenity.dto';
import { AmenityService } from '../../services/amenity.service';
import { CommonModule } from '@angular/common';
import { UpdateApartmentDto } from '../../dtos/updateApartment.dto';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NewApartmentDto } from '../../dtos/newApartment.dto';

@Component({
  selector: 'app-apartment-form',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './apartment-form.component.html',
  styleUrl: './apartment-form.component.css'
})
export class ApartmentFormComponent implements OnChanges{
  @Input() apartment!: ApartmentDto;
  apartmentForm: FormGroup;
  amenities: AmenityDto[] = [];
  isEditMode: boolean = false;
  selectedAmenities: string[] = [];
  loading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private apartmentsService: ApartmentsService,
    private amenityService: AmenityService,
    private router: Router,
    private toastService: ToastrService
  ) {
    this.apartmentForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, Validators.required],
      cleaningFee: [0, Validators.required],
      amenities: [[], Validators.required],
      imagePath: ['', Validators.required],
      address: this.fb.group({
        street: ['', Validators.required],
        city: ['', Validators.required],
        state: ['', Validators.required],
        country: ['', Validators.required],
        number: ['', Validators.required],
        zipCode: ['', Validators.required]
      })
    });

    this.loadAmenities();

    if (this.apartment) {
      this.isEditMode = true;
      this.populateForm();
      this.setAmenitiesList();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['apartment'] && this.apartment) {
      this.isEditMode = true;
      this.populateForm();
      this.setAmenitiesList();
    }
  }

  populateForm(): void {
    this.apartmentForm.patchValue({
      name: this.apartment.name,
      description: this.apartment.description,
      price: this.apartment.price,
      cleaningFee: this.apartment.cleaningFee,
      amenities: this.apartment.amenities,
      imagePath: this.apartment.imagePath,
      address: {
        street: this.apartment.address.street,
        city: this.apartment.address.city,
        state: this.apartment.address.state,
        country: this.apartment.address.country,
        number: this.apartment.address.number,
        zipCode: this.apartment.address.zipCode,
      }
    });

    this.setAmenitiesList();
  }

  loadAmenities(): void {
    this.amenityService.getAmenities().subscribe(response => {
      this.amenities = response;
    });
  }

  removeAmenity(amenityId: string): void {
    this.selectedAmenities = this.selectedAmenities.filter(id => id !== amenityId);
  }

  onAmenitySelect(event: Event): void {
    const selectedAmenityId = (event.target as HTMLSelectElement).value;
    if (!this.selectedAmenities.includes(selectedAmenityId)) {
      this.selectedAmenities.push(selectedAmenityId);
    }
  }

  setAmenitiesList(): void {
    this.selectedAmenities = [...this.apartment.amenities.map((a)=> a.id)];
  }

  getAmenityName(amenityId: string): string {
    const amenity = this.amenities.find(a => a.id === amenityId);
    return amenity ? amenity.name : '';
  }

  onSave(): void {
    this.loading = true;
    console.log(this.loading);
    if (!this.apartmentForm.valid)
      console.log('err');

    if(this.apartment){
      this.updateApartment();
      return;
    }

    this.createApartment();
  }

  createApartment() {
    const formValues = this.apartmentForm.value;
    var newApartmentDto = new NewApartmentDto(
      formValues.name,
      formValues.description,
      formValues.address,
      formValues.price,
      formValues.cleaningFee,
      this.selectedAmenities,
      formValues.imagePath,
    );

    this.apartmentsService.createApartment(newApartmentDto).subscribe(res => {
      if(res.isSuccess) {
        this.loading = false;
        this.toastService.success("Apartment Created!")
        this.return();
      }

      this.toastService.error(res.error.name);
      this.loading = false;
      return;
    })

  }

  updateApartment() {
    const formValues = this.apartmentForm.value;
    var updateApartmentDto = new UpdateApartmentDto(
      this.apartment.id,
      formValues.name,
      formValues.description,
      formValues.address,
      formValues.price,
      formValues.cleaningFee,
      this.selectedAmenities,
      formValues.imagePath,
      this.apartment.ownerId
    );

    this.apartmentsService.updateApartment(this.apartment.id, updateApartmentDto).subscribe(res => {
      if(res.isSuccess) {
        this.loading = false;
        this.toastService.success("Apartment updated!")
        return;
      }
      this.toastService.error(res.error.name);
      this.loading = false;
      return;
    })
  }

  return(){
    this.router.navigate(['/profile/apartments']);
  }
}
