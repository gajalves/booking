import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { ReturnCreatedUserDto } from "../dtos/createduser.dto";
import { Observable, tap } from "rxjs";
import { UserRegisterDto } from "../dtos/userregister.dto";
import { ReturnLoginUserDto } from "../dtos/returnloginuser.dto";
import { UserLoginDto } from "../dtos/userlogin.dto";
import { ErrorReturnDto } from "../dtos/errorreturn.dto";
import { UserInfoDto } from "../dtos/userInfo.dto";

@Injectable({
  providedIn: 'root'
})
export class IdentityService {
  protected API_URL: string;
  protected http: HttpClient;

  constructor(http: HttpClient) {

    this.API_URL = environment.identity_url;
    this.http = http;
  }

  register(name: string, email: string, password: string): Observable<ReturnCreatedUserDto | ErrorReturnDto> {
    let payload = new UserRegisterDto(name, email, password);
    return this.http.post<ReturnCreatedUserDto>(`${this.API_URL}/User/Register`, payload);
  }

  login(email: string, password: string): Observable<ReturnLoginUserDto | ErrorReturnDto> {
    let payload = new UserLoginDto(email, password);
    return this.http.post<ReturnLoginUserDto>(`${this.API_URL}/User/Login`, payload).pipe(
      tap((response: ReturnLoginUserDto) => {
        localStorage.setItem('accessToken', response.accessToken);
        localStorage.setItem('userName', response.userName);
        localStorage.setItem('userEmail', response.userEmail);
        localStorage.setItem('userId', response.userId);
      })
    );
  }

  userInfo(id: string): Observable<UserInfoDto | ErrorReturnDto> {
    return this.http.get<UserInfoDto>(`${this.API_URL}/User/UserInfo/${id}`);
  }

  logout() {
    localStorage.clear();

  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('accessToken');
  }

  getToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  getUserName(): string | null {
    return localStorage.getItem('userName');
  }

  getUserEmail(): string | null {
    return localStorage.getItem('userEmail');
  }

  getUserId(): string | null {
    return localStorage.getItem('userId');
  }
}
