export class NewReservationDto {
  ApartmentId: string;
  StartDate: string;
  EndDate: string;

  constructor(
    ApartmentId: string,
    StartDate: string,
    EndDate: string
  ){
    this.ApartmentId = ApartmentId;
    this.StartDate = StartDate;
    this.EndDate = EndDate
  }
}
