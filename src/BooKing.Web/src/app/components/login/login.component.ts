import { Component, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { IdentityService } from '../../services/identity.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserLoginDto } from '../../dtos/userLogin.dto';
import { ErrorReturnDto } from '../../dtos/errorReturn.dto';
import { CommonModule } from '@angular/common';
import { BtnPrimaryComponent } from '../btn-primary/btn-primary.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, BtnPrimaryComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm!: FormGroup;
  loading = signal(false);

  constructor(
    private identityService: IdentityService,
    private router: Router,
    private toastService: ToastrService
  ){
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)])
    })
  }

  createRegisterObject(): UserLoginDto {
    const { name, email, password } = this.loginForm.value;
    return new UserLoginDto(email, password);
  }

  onSubmit() {
    this.loading.set(true);
    if (this.loginForm.invalid) {
      this.loading.set(false);
      return;
    }

    const loginObject = this.createRegisterObject();
    this.identityService.login(loginObject.email, loginObject.password).subscribe({
      next: (response) => {
        this.loginForm.reset();
        this.toastService.success("Login Successful!");
        setTimeout(() => {
          this.loading.set(false);
          this.router.navigate(['/']);
        }, 300);
      },
      error: (e) => {
        const ret = e.error as ErrorReturnDto;
        this.toastService.error(ret.name);
        this.loading.set(false);
      }
    });
  }
}
