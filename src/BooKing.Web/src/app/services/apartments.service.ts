import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ApartmentDto } from '../dtos/apartment.dto';
import { BaseService } from './base.service';
import { environment } from '../../environments/environment';
import { BaseResultDto } from '../dtos/base.result.dto';
import { Result } from '../dtos/result.dto';
import { UpdateApartmentDto } from '../dtos/updateApartment.dto';
import { NewApartmentDto } from '../dtos/newApartment.dto';

@Injectable({
  providedIn: 'root'
})
export class ApartmentsService {
  protected API_URL: string;
  protected http: HttpClient;

  constructor(http: HttpClient) {
    this.http = http;

    this.API_URL = environment.apartment_url;
  }

  getPaginatedApartments(pageIndex: number, pageSize: number): Observable<{ status: number, body: BaseResultDto }> {
    return this.http.get<BaseResultDto>(`${this.API_URL}/Apartment?pageIndex=${pageIndex}&pageSize=${pageSize}`, { observe: 'response' })
      .pipe(
        map((response: HttpResponse<BaseResultDto>) => ({
          status: response.status,
          body: response.body as BaseResultDto
        }))
      );
  }

  getApartmentDetail(apartmentId: string): Observable<{ status: number, body: BaseResultDto }> {
    return this.http.get<BaseResultDto>(`${this.API_URL}/Apartment/${apartmentId}`, { observe: 'response' })
      .pipe(
        map((response: HttpResponse<BaseResultDto>) => ({
          status: response.status,
          body: response.body as BaseResultDto
        }))
      );
  }

  getUserApartments(userId: string): Observable<Result<ApartmentDto[]>> {
    return this.http.get<Result<ApartmentDto[]>>(`${this.API_URL}/Apartment/UserApartments/${userId}`)
  }

  createApartment(apartment: NewApartmentDto): Observable<Result<ApartmentDto[]>> {
    return this.http.post<Result<ApartmentDto[]>>(`${this.API_URL}/Apartment/Create`, apartment);
  }

  updateApartment(apartmentId: string, apartment: UpdateApartmentDto): Observable<Result<ApartmentDto[]>> {
    return this.http.put<Result<ApartmentDto[]>>(`${this.API_URL}/Apartment/${apartmentId}`, apartment);
  }
}
