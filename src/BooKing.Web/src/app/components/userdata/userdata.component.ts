import { Component } from '@angular/core';
import { UserInfoDto } from '../../dtos/userInfo.dto';
import { IdentityService } from '../../services/identity.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ErrorReturnDto } from '../../dtos/errorReturn.dto';

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
    private toastService: ToastrService
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
      },
      error: (e) => {
        const ret = e.error as ErrorReturnDto;
        this.toastService.error(ret.name);
      }
    });
  }
}
