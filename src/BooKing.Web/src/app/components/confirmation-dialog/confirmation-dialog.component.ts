import { Component, Injectable, Input, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Injectable()
@Component({
  selector: 'app-confirmation-dialog',
  standalone: true,
  imports: [],
  templateUrl: './confirmation-dialog.component.html',
  styleUrl: './confirmation-dialog.component.css'
})
export class ConfirmationDialogComponent {

  @Input() title: string = 'Please confirm';
  @Input() message: string = 'Do you really want to confirm?';
  @Input() btnOkText: string = 'Ok';
  @Input() btnCancelText: string = 'Cancel';

  constructor(private activeModal: NgbActiveModal) { }

  public decline() {
    this.activeModal.close(false);
  }

  public accept() {
    this.activeModal.close(true);
  }

  public dismiss() {
    this.activeModal.dismiss();
  }
}
