import { Component } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { IdentityService } from '../../services/identity.service';
import { UserDataDto } from '../../dtos/user.dto';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [RouterOutlet, RouterModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  userData: UserDataDto;

  constructor(
    private identityService: IdentityService,
    private router: Router
  ){
    this.userData = this.getUserData();
  }

  logOutUser() {
    this.identityService.logout();
    this.router.navigate(['/']);
  }

  getUserData() : UserDataDto{
    const name = this.identityService.getUserName();
    const email = this.identityService.getUserEmail();
    return new UserDataDto(name!, email!);
  }

}
