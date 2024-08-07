import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ApartmentDto } from '../dtos/apartment.dto';
import { BaseService } from './base.service';
import { environment } from '../../environments/environment';
import { BaseResultDto } from '../dtos/base.result.dto';

@Injectable({
  providedIn: 'root'
})
export class ApartmentsService extends BaseService<ApartmentDto> {
  protected API_URL: string;

  constructor(http: HttpClient) {
    super(http);

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
}
