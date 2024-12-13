import { BaseModel } from "../base";

export class User extends BaseModel {
    SubscriberId!: number;
    Login!: string;
    Name!: string;
    ProfileCode!: string;
    Password!: string;
    Phone!: string;
    Email!: string;
    Active!: string;
    LoginCounter!: number;
    LoginErrorCounter!: number;
}

