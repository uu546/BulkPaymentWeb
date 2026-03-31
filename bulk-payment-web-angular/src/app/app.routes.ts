import { Routes } from '@angular/router';
import { RegistryUploadComponent } from './payment-registry/components/registry-upload/registry-upload.component';

export const routes: Routes = [
    // Роуты для DocumentShipment
  {
    path: '',
    component: RegistryUploadComponent,
  },
];
