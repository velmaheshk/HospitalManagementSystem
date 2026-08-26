import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments';
 import { CreateDoctorRequest } from '../models/create-doctor-request.model';
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
    private readonly http: HttpClient
  ) {}

 getDoctors(): Observable<Doctor[]> {
   return this.http.get<Doctor[]>(this.apiUrl);
 }

 getAll(): Observable<Doctor[]> {
   return this.getDoctors();
 }

  getDoctorById(id: number): Observable<Doctor> {
   return this.http.get<Doctor>(`${this.apiUrl}/${id}`);
 }

 getById(id: number): Observable<Doctor> {
   return this.getDoctorById(id);
 }

  createDoctor(request: Doctor): Observable<Doctor> {
   return this.http.post<Doctor>(this.apiUrl, request);
 }

//  create(doctor: Doctor): Observable<Doctor> {
//    return this.createDoctor(doctor);
//  }
create(request: CreateDoctorRequest) {
  return this.http.post<Doctor>(
    this.apiUrl,
    request
  );
}
   updateDoctor(id: number, request: Doctor): Observable<void> {
   return this.http.put<void>(`${this.apiUrl}/${id}`, request);
 }

 update(id: number, doctor: Doctor): Observable<void> {
   return this.updateDoctor(id, doctor);
 }

 deleteDoctor(id: number): Observable<void> {
   return this.http.delete<void>(`${this.apiUrl}/${id}`);
 }

 delete(id: number): Observable<void> {
   return this.deleteDoctor(id);
 }

 getDashboard(): Observable<doctordashboard> {
   return this.http.get<doctordashboard>(`${this.apiUrl}/dashboard`);
 }
}