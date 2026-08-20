import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments';
 
import {
  Doctor
} from '../models/doctor.model';
import { doctordashboard } from '../models/doctordashboard.model';

@Injectable({
  providedIn: 'root'
})
export class DoctorApiService {
 private readonly apiUrl =
    `${environment.apiUrl}/doctors`;
   

  constructor(
    private http: HttpClient
  ) {}

  getAll(): Observable<Doctor[]> {
    return this.http.get<Doctor[]>(
      this.apiUrl
    );
  }

  getById(id: number): Observable<Doctor> {
    return this.http.get<Doctor>(
      `${this.apiUrl}/${id}`
    );
  }

  create(doctor: Doctor): Observable<Doctor> {
    return this.http.post<Doctor>(
      this.apiUrl,
      doctor
    );
  }

  update(
    id: number,
    doctor: Doctor
  ): Observable<void> {

    return this.http.put<void>(
      `${this.apiUrl}/${id}`,
      doctor
    );
  }

  delete(id: number): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }

  getDashboard(): Observable<doctordashboard> {

    return this.http.get<doctordashboard>(
      `${this.apiUrl}/dashboard`
    );
  }
}