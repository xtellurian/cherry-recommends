import { AxiosInstance } from "axios";
interface InitialiseAxiosConfig {
    baseUrl: string;
    tenant?: string | null;
    timeout?: number;
}
export declare const current: (config?: InitialiseAxiosConfig | undefined) => AxiosInstance;
export {};
