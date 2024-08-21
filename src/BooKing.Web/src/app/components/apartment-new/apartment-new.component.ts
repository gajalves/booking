import { Component } from '@angular/core';
import { ApartmentFormComponent } from '../apartment-form/apartment-form.component';

@Component({
  selector: 'app-apartment-new',
  standalone: true,
  imports: [ApartmentFormComponent],
  templateUrl: './apartment-new.component.html',
  styleUrl: './apartment-new.component.css'
})
export class ApartmentNewComponent {

}
