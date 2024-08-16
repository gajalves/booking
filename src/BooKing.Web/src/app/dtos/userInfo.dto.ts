export class UserInfoDto {
  userName: string;
  email: string;
  totalReservations: number;
  apartmentsCreated: number;

  constructor(
    userName: string,
    email: string,
    totalReservations: number,
    apartmentsCreated: number
  ){
    this.userName = userName;
    this.email = email;
    this.totalReservations = totalReservations;
    this.apartmentsCreated = apartmentsCreated;
  }
}
