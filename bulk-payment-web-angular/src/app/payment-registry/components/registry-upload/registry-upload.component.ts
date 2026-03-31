import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { NgxFileDropEntry, NgxFileDropModule } from 'ngx-file-drop';
import { BehaviorSubject } from 'rxjs';
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

  private progress$ = new BehaviorSubject<ProcessingProgressOutput | null>(null);

  constructor(
    private paymentService: PaymentService,
    private signalRService: SignalrService,
    private cdr: ChangeDetectorRef,
  ) {
    this.progress$ = this.signalRService.progress$;
  }

  ngOnInit() {
    this.signalRService.progress$.subscribe((progress) => {
      this.uploadProgress = progress;
      if (this.progress$.value?.status === 'Completed') {
        this.fetchFinalPayments(this.progress$.value!.registryId);
      }

      this.cdr.detectChanges(); // Принудительно обновляем экран
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

  private fetchFinalPayments(registryId: number): void {
    this.paymentService.getPayments(registryId).subscribe({
      next: (data) => {
        this.payments = data;
        this.cdr.detectChanges();
      },
      error: (err) => console.error('Ошибка загрузки итогов:', err),
    });
  }

  ngOnDestroy(): void {
    if (this.progress$.value?.registryId) {
      this.progress$.unsubscribe();
    }
    this.signalRService.stopConnection();
  }
}
