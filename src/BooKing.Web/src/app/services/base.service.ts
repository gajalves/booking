import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export abstract class BaseService<T> {
  protected abstract API_URL: string;

  constructor(protected http: HttpClient) {}

  getAll(): Observable<T[]> {
    return this.http.get<T[]>(this.API_URL);
  }

  getById(id: string): Observable<T> {
    return this.http.get<T>(`${this.API_URL}/${id}`);
  }

  create(item: T): Observable<T> {
    return this.http.post<T>(this.API_URL, item);
  }

  update(id: string, item: T): Observable<T> {
    return this.http.put<T>(`${this.API_URL}/${id}`, item);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
