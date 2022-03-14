import { AuthenticatedRequest, DeleteRequest, EntityRequest, EntitySearchRequest, PaginatedEntityRequest, MetricBinRequest } from "../interfaces";
import { components } from "../model/api";
interface MetricSearchRequest extends EntitySearchRequest {
    scope?: components["schemas"]["MetricScopes"];
}
export declare const fetchMetricsAsync: ({ token, page, scope, searchTerm, }: MetricSearchRequest) => Promise<components["schemas"]["MetricPaginated"]>;
export declare const fetchMetricAsync: ({ token, id, }: EntityRequest) => Promise<components["schemas"]["Metric"]>;
export declare const fetchMetricCustomersAsync: ({ token, page, id, }: PaginatedEntityRequest) => Promise<any>;
export declare const fetchMetricCustomerMetricsAsync: ({ token, page, id, }: PaginatedEntityRequest) => Promise<any>;
interface CreateMetricRequest extends AuthenticatedRequest {
    metric: components["schemas"]["CreateMetric"];
}
export declare const createMetricAsync: ({ token, metric, }: CreateMetricRequest) => Promise<any>;
export declare const deleteMetricAsync: ({ token, id }: DeleteRequest) => Promise<any>;
export declare const fetchCustomersMetricsAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface CustomersMetricRequest extends EntityRequest {
    metricId: string | number;
    version?: number | undefined;
}
export declare const fetchCustomersMetricAsync: ({ token, id, metricId, version, }: CustomersMetricRequest) => Promise<any>;
export declare const fetchAggregateMetricValuesNumericAsync: ({ token, id, }: EntityRequest) => Promise<any>;
export declare const fetchAggregateMetricValuesStringAsync: ({ token, id, }: EntityRequest) => Promise<any>;
export declare const fetchDestinationsAsync: ({ token, id }: EntityRequest) => Promise<any>;
export declare const fetchExportCustomers: ({ token, id }: EntityRequest) => Promise<any>;
export declare const fetchMetricBinValuesNumericAsync: ({ token, id, binCount, }: MetricBinRequest) => Promise<any>;
export declare const fetchMetricBinValuesStringAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface CreateMetricDestinationRequest extends EntityRequest {
    destination: components["schemas"]["CreateDestinationDto"];
}
export declare const createDestinationAsync: ({ token, id, destination, }: CreateMetricDestinationRequest) => Promise<any>;
interface DeleteDestinationRequest extends DeleteRequest {
    destinationId: number;
}
export declare const deleteDestinationAsync: ({ token, id, destinationId, }: DeleteDestinationRequest) => Promise<any>;
export declare const fetchGeneratorsAsync: ({ token, id }: EntityRequest) => Promise<any>;
export declare const fetchBusinessMetricsAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface BusinessMetricRequest extends CustomersMetricRequest {
}
export declare const fetchBusinessMetricAsync: ({ token, id, metricId, version, }: BusinessMetricRequest) => Promise<any>;
export {};
