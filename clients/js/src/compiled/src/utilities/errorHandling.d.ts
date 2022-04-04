import { AxiosResponse } from "axios";
declare type ErrorHandler = (response: AxiosResponse) => Promise<any>;
export declare const setErrorResponseHandler: (errorHandler: ErrorHandler) => void;
export declare const handleErrorResponse: ErrorHandler;
export {};
