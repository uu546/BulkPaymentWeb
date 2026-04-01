import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { NgxFileDropEntry, NgxFileDropModule } from 'ngx-file-drop';
import { ProcessingProgressOutput } from '../../models/output/processing-progress-output';
import { PaymentService } from '../../services/payment.service';
import { SignalrService } from '../../services/signalr.service';

@Component({
  selector: 'app-registry-upload.component',
  imports: [CommonModule, NgxFileDropModule],
  templateUrl: './registry-upload.component.html',
  styleUrl: './registry-upload.component.css',
})
export class RegistryUploadComponent implements OnInit, OnDestroy {
  public selectedFile: File | null = null;
  public uploadProgress: ProcessingProgressOutput | null = null;
  public payments: any[] = [];
  public globalError: string | null = null;

  constructor(
    private paymentService: PaymentService,
    private signalRService: SignalrService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.signalRService.progress$.subscribe((progress) => {
      this.uploadProgress = progress;
      if (this.uploadProgress?.status === 'Completed') {
        this.getFinalPayments(this.uploadProgress!.registryId);
      }

      this.cdr.detectChanges(); // Принудительно обновляем экран
    });

    this.signalRService.error$.subscribe((errMessage) => {
      this.globalError = errMessage;
      this.uploadProgress = null;
      this.cdr.detectChanges();
    });
  }

  // Метод, который вызывает библиотека при сбросе файла
  public dropped(files: NgxFileDropEntry[]) {
    // Берем только первый файл
    const droppedFile = files[0];

    // Проверяем, что это действительно файл, а не папка
    if (droppedFile.fileEntry.isFile) {
      const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
      fileEntry.file((file: File) => {
        // Извлекли настоящий File, передаем его в наш обработчик
        this.handleFile(file);
      });
    }
  }

  private handleFile(file: File): void {
    if (!file) return;
    const ext = file.name.split('.').pop()?.toLowerCase();

    if (ext !== 'xls' && ext !== 'xlsx') {
      alert('Пожалуйста, загрузите файл формата Excel.');
      return;
    }
    this.globalError = null;
    this.selectedFile = file;
    this.uploadProgress = null;
    this.payments = [];
  }

  public uploadFile(): void {
    if (!this.selectedFile) return;

    this.paymentService.upload(this.selectedFile).subscribe({
      next: (response) => {
        this.signalRService.startConnection(response.registryId);
        console.log('Job стартовал', response);
      },
      error: () => alert('Ошибка загрузки'),
    });
  }

  private async getFinalPayments(registryId: number): Promise<void> {
    try {
      console.log('Запрашиваем итоги через SignalR Hub...');

      const data = await this.signalRService.getFinalPayments(registryId);

      console.log('Данные получены через хаб:', data);

      this.payments = data;

      this.cdr.detectChanges();
    } catch (err) {
      console.error('Ошибка загрузки итогов через хаб:', err);
    }
  }

  ngOnDestroy(): void {
    this.signalRService.stopConnection();
  }
}
