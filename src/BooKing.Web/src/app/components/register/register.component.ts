import { Component, signal } from '@angular/core';
import { IdentityService } from '../../services/identity.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserRegisterDto } from '../../dtos/userRegister.dto';
import { ToastrService } from 'ngx-toastr';
import { ErrorReturnDto } from '../../dtos/errorReturn.dto';
import { BtnPrimaryComponent } from '../btn-primary/btn-primary.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, BtnPrimaryComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})

export class RegisterComponent {
  registerForm!: FormGroup;
  loading = signal(false);

  constructor(
    private identityService: IdentityService,
    private router: Router,
    private toastService: ToastrService
  ) {
    this.registerForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)])
    })
  }

  createRegisterObject(): UserRegisterDto {
    const { name, email, password } = this.registerForm.value;
    return new UserRegisterDto(name, email, password);
  }

  onSubmit() {
    this.loading.set(true);
    if (this.registerForm.invalid) {
      this.loading.set(false);
      return;
    }

    const registerObject = this.createRegisterObject();
    this.identityService.register(registerObject.name, registerObject.email, registerObject.password).subscribe({
      next: (response) => {
        this.registerForm.reset();
        this.toastService.success("Your account has been created!")
        setTimeout(() =>
          {
              this.loading.set(false);
              this.router.navigate(['/login']);
          },
          5000);
      },
      error: (e) => {
        const ret = e.error as ErrorReturnDto;
        this.toastService.error(ret.name);
        this.loading.set(false);
      }
    });
  }
}
