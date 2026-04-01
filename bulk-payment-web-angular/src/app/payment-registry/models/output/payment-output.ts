export interface PaymentOutput {
  id: number;
  registryId: number;
  payerInn: string;
  payerAccount: string;
  receiverInn: string;
  receiverAccount: string;
  receiverBik: string;
  amount: number;
  purpose: string;
  isValid: boolean;
  validationError?: string;
}
