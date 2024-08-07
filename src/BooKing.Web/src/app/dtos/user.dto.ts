export class UserDataDto {
  name: string;
  email: string;

  constructor(
    name: string,
    email: string
  ){
    this.email = email;
    this.name = name;
  }
}
