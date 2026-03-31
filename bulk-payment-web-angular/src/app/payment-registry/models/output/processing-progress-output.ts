export interface ProcessingProgressOutput {
  registryId: number;
  percent: number;
  processedRows: number;
  totalRows: number;
  errorCount: number;
  status: string;
}
