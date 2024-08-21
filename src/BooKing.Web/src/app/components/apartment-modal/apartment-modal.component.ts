import { Component, Input } from '@angular/core';
import { ApartmentDto } from '../../dtos/apartment.dto';

@Component({
  selector: 'app-apartment-modal',
  standalone: true,
  imports: [],
  templateUrl: './apartment-modal.component.html',
  styleUrl: './apartment-modal.component.css'
})
export class ApartmentModalComponent {
  @Input() apartment?: ApartmentDto;
}
