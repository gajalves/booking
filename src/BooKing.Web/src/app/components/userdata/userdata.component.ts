import { Component } from '@angular/core';
import { UserInfoDto } from '../../dtos/userInfo.dto';
import { IdentityService } from '../../services/identity.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ErrorReturnDto } from '../../dtos/errorReturn.dto';
import { ApartmentsService } from '../../services/apartments.service';
import { ReserveService } from '../../services/reserve.service';

@Component({
  selector: 'app-userdata',
  standalone: true,
  imports: [],
  templateUrl: './userdata.component.html',
  styleUrl: './userdata.component.css'
})
export class UserdataComponent {
  userProfile: UserInfoDto | null = null;

  constructor(
    private identityService: IdentityService,
    private router: Router,
    private toastService: ToastrService,
    private apartmentsService: ApartmentsService,
    private reserveService: ReserveService
  ) {
    this.getUserInfo();
  }


  getUserInfo() {
    const userId = this.identityService.getUserId();

    if(!userId) {
      this.toastService.error("User Id not found");
      this.router.navigate(['/login']);
    }

    this.identityService.userInfo(userId!).subscribe({
      next: (response) => {
        this.userProfile = response as UserInfoDto;
        this.getCountUserApartmentsCreated();
        this.getCountUserReservations();
      },
      error: (e) => {
        const ret = e.error as ErrorReturnDto;
        this.toastService.error(ret.name);
      }
    });
  }

  getCountUserApartmentsCreated() {
    this.apartmentsService.getCountUserApartmentsCreated().subscribe({
      next: (response) => {
        this.userProfile!.apartmentsCreated = response.value as number;
      },
      error: (e) => {
        const ret = e.error as ErrorReturnDto;
        this.toastService.error(ret.name);
      }
    });
  }

  getCountUserReservations() {
    this.reserveService.getCountUserReservations().subscribe({
      next: (response) => {
        this.userProfile!.totalReservations = response.value as number;
      },
      error: (e) => {
        const ret = e.error as ErrorReturnDto;
        this.toastService.error(ret.name);
      }
    });
  }
}
