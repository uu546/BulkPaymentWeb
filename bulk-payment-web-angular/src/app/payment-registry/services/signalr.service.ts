import { Injectable } from '@angular/core';
import { ProcessingProgressOutput } from '../models/output/processing-progress-output';
import { BehaviorSubject } from 'rxjs';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';

@Injectable({ providedIn: 'root' })
export class SignalrService {
  private hubConnection: HubConnection | null = null;
  public progress$ = new BehaviorSubject<ProcessingProgressOutput | null>(null);

  constructor() {}

  public async startConnection(registryId: number): Promise<void> {
    // Если уже подключены - ничего не делаем
    console.log(this.hubConnection);

    if (this.hubConnection && this.hubConnection.state !== HubConnectionState.Disconnected) {
      return;
    }

    if (!this.hubConnection) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl('http://localhost:5000/hubs/payments')
        .withAutomaticReconnect()
        .build();

      this.hubConnection.on('UpdateProgress', (data: ProcessingProgressOutput) => {
        console.warn('Данные с метода UpdateProgress', data);
        this.progress$.next(data);
      });
    }

    try {
      this.hubConnection.start().then(() => {
        this.hubConnection?.invoke("JoinRegistryGroupAsync", registryId);
        console.log("Присоединились к группе хаба: ", registryId);
      });
      console.log('✅ SignalR подключен');
    } catch (err) {
      console.error('❌ Ошибка SignalR:', err);
    }
  }

  public stopConnection(): void {
    if (this.hubConnection?.state !== HubConnectionState.Disconnected) {
      this.hubConnection?.stop();
    }
  }
}
