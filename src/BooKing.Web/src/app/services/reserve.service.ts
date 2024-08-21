import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { NewReservationDto } from "../dtos/newreservation.dto";
import { Observable } from "rxjs";
import { ErrorReturnDto } from "../dtos/errorReturn.dto";
import { BaseResultDto } from "../dtos/base.result.dto";
import { Result } from "../dtos/result.dto";
import { ReservationDto } from "../dtos/reservation.dto";
import { ReservationEventsDto } from "../dtos/reservationEvents.dto";

@Injectable({
  providedIn: 'root'
})
export class ReserveService {
  protected API_URL: string;
  protected http: HttpClient;

  constructor(http: HttpClient) {

    this.API_URL = environment.reserve_url;
    this.http = http;
  }

  reserve(apartmentId: string, startDate: string, endDate: string): Observable<BaseResultDto | ErrorReturnDto> {
    const payload = new NewReservationDto(apartmentId, startDate, endDate);

    return this.http.post<BaseResultDto>(`${this.API_URL}/Reservation/Reserve`, payload);
  }

  getUserReservations(userId: string): Observable<Result<ReservationDto[]>> {
    return this.http.get<Result<ReservationDto[]>>(`${this.API_URL}/Reservation/UserReservations/${userId}`);
  }

  getReservation(reservationId: string): Observable<Result<ReservationDto>> {
    return this.http.get<Result<ReservationDto>>(`${this.API_URL}/Reservation/${reservationId}`);
  }

  getReservationEvents(reservationId: string): Observable<Result<ReservationEventsDto[]>> {
    return this.http.get<Result<ReservationEventsDto[]>>(`${this.API_URL}/Reservation/ReservationEvents/${reservationId}`);
  }

  confirm(reservationId: string): Observable<Result<object>> {
    const payload = '"' + reservationId + '"'
    return this.http.post<Result<object>>(`${this.API_URL}/Reservation/Confirm`, payload);
  }

  cancel(reservationId: string): Observable<BaseResultDto | ErrorReturnDto> {
    const payload = '"' + reservationId + '"'
    return this.http.post<BaseResultDto>(`${this.API_URL}/Reservation/Cancel`, payload);
  }
}
