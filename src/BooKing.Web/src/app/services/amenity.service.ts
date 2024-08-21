import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Injectable } from "@angular/core";
import { AmenityDto } from "../dtos/amenity.dto";
import { Result } from "../dtos/result.dto";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AmenityService {
  protected API_URL: string;
  protected http: HttpClient;

  constructor(http: HttpClient) {
    this.http = http;

    this.API_URL = environment.apartment_url;
  }

  getAmenities(): Observable<AmenityDto[]> {
    return this.http.get<AmenityDto[]>(`${this.API_URL}/Amenity/GetAll`);
  }
}
