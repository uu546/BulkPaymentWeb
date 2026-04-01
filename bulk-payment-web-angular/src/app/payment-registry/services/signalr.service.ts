import { Injectable } from '@angular/core';
import { ProcessingProgressOutput } from '../models/output/processing-progress-output';
import { BehaviorSubject, Subject } from 'rxjs';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { PaymentOutput } from '../models/output/payment-output';

@Injectable({ providedIn: 'root' })
export class SignalrService {
  private hubConnection: HubConnection | null = null;
  public progress$ = new BehaviorSubject<ProcessingProgressOutput | null>(null);
  public error$ = new BehaviorSubject<string | null>(null);

  constructor() {}

  public async startConnection(registryId: number): Promise<void> {
    // Если уже подключены - ничего не делаем
    console.log(this.hubConnection);

    // if (this.hubConnection && this.hubConnection.state !== HubConnectionState.Disconnected) {
    //   return;
    // }

    // if (!this.hubConnection) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl('http://localhost:5000/hubs/payments', {
          transport: HttpTransportType.ServerSentEvents,
        })
        .withAutomaticReconnect()
        .build();

      this.hubConnection.on('UpdateProgress', (data: ProcessingProgressOutput) => {
        console.warn('Данные с метода UpdateProgress', data);
        this.progress$.next(data);
      });

      this.hubConnection.on('ErrorMessage', (error) => {
        console.error('Ошибка с метода UpdateProgress:', error);
        this.error$.next(error);
      });
    // }

    try {
      this.hubConnection.start().then(() => {
        this.hubConnection?.invoke('JoinRegistryGroupAsync', registryId);
        console.log('Присоединились к группе хаба: ', registryId);
      });
      console.log('✅ SignalR подключен');
    } catch (err) {
      console.error('❌ Ошибка SignalR:', err);
    }
  }

  // Добавь этот метод в класс SignalrService
  public async getFinalPayments(registryId: number): Promise<PaymentOutput[]> {
    if (this.hubConnection?.state !== HubConnectionState.Connected) {
      throw new Error('SignalR не подключен!');
    }
    const result = await this.hubConnection.invoke<PaymentOutput[]>(
      'GetPaymentsByRegistryIdAsync',
      registryId,
    );

    console.log(`Получен список платежей реестра Id: ${registryId}`, result);
    return result;
  }

  public stopConnection(): void {
    if (this.hubConnection?.state !== HubConnectionState.Disconnected) {
      this.hubConnection?.stop();
    }
  }
}
