export class ReturnLoginUserDto {
  accessToken: string;
  expiresIn: number;
  userId: string;
  userEmail: string;
  userName: string;

  constructor(
    accessToken: string,
    expiresIn: number,
    userId: string,
    userEmail: string,
    userName: string
  ) {
    this.accessToken = accessToken;
    this.expiresIn = expiresIn;
    this.userId = userId;
    this.userEmail = userEmail;
    this.userName =  userName;
  }
}
