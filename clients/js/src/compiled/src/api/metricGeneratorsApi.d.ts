import { AuthenticatedRequest, DeleteRequest, EntityRequest, PaginatedRequest } from "../interfaces";
import { components } from "../model/api";
export declare const fetchMetricGeneratorsAsync: ({ page, token, }: PaginatedRequest) => Promise<any>;
interface CreateMetricGeneratorRequest extends AuthenticatedRequest {
    generator: components["schemas"]["CreateMetricGenerator"];
}
export declare const createMetricGeneratorAsync: ({ token, generator, }: CreateMetricGeneratorRequest) => Promise<any>;
export declare const deleteMetricGeneratorAsync: ({ token, id, }: DeleteRequest) => Promise<any>;
export declare const manualTriggerMetricGeneratorsAsync: ({ token, id, }: EntityRequest) => Promise<any>;
export {};
