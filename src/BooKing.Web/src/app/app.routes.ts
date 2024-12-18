import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './services/authguard.service';
import { ProfileComponent } from './components/profile/profile.component';
import { ReservationsComponent } from './components/reservations/reservations.component';
import { UserdataComponent } from './components/userdata/userdata.component';
import { ApartmentDetailComponent } from './components/apartment-detail/apartment-detail.component';
import { ReservationDetailComponent } from './components/reservation-detail/reservation-detail.component';
import { UserApartmentsComponent } from './components/user-apartments/user-apartments.component';
import { ApartmentEditComponent } from './components/apartment-edit/apartment-edit.component';
import { ApartmentNewComponent } from './components/apartment-new/apartment-new.component';

export const routes: Routes = [
  {
    path: "",
    component: HomeComponent
  },
  {
    path: "login",
    component: LoginComponent
  },
  {
    path: "register",
    component: RegisterComponent
  },
  {
    path: "profile",
    component: ProfileComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'user', component: UserdataComponent },
      { path: 'reservations', component: ReservationsComponent },
      { path: 'reservations/:id/details', component: ReservationDetailComponent },
      { path: 'apartments', component: UserApartmentsComponent },
      { path: 'apartments/:id/edit', component: ApartmentEditComponent },
      { path: 'apartments/new', component: ApartmentNewComponent }
    ]
  },
  {
    path: "details/:id",
    component: ApartmentDetailComponent
  }
];
