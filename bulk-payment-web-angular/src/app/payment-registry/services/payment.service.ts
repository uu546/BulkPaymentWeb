import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  private apiUrl = 'http://localhost:5000/api/payment';

  constructor(private http: HttpClient) {}

  public upload(file: File): Observable<{ registryId: number; message: string }> {
    const formData = new FormData();

    formData.append('file', file);

    return this.http.post<{ registryId: number; message: string }>(
      `${this.apiUrl}/upload`,
      formData,
    );
  }
}
