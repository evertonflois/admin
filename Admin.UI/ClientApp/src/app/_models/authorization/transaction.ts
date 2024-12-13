import { TransactionActions } from "./transactionActions";

export class Transaction {
    SubscriberId!: number;
    TransactionCode!: string;
    Description!: string;    
    FlagPermission!: string;
    FlagOriginalPermission!: string;
    Active!: string;
    Actions!: TransactionActions[];   
}
