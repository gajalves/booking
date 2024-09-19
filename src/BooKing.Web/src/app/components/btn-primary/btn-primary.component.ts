import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'btn-primary',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './btn-primary.component.html',
  styleUrl: './btn-primary.component.css'
})
export class BtnPrimaryComponent {
  @Input('btnText') btnText: string = '';
  @Input() disabled: boolean = false;
  @Input() loading: boolean = false;
  @Input() btnClass: string = 'btn-primary';

  @Output('submit') onSubmit = new EventEmitter<void>();

  submit() {
    this.onSubmit.emit();
  }
}
