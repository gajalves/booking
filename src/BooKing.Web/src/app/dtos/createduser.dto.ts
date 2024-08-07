export class ReturnCreatedUserDto {
  Id: string;
  Email: string;
  Name: string;

  constructor(
    id: string,
    email: string,
    name: string
  ) {
  this.Id = id;
  this.Email = email;
  this.Name = name;
  }
}
