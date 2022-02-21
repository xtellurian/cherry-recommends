import {
  AuthenticatedRequest,
  DeleteRequest,
  EntityRequest,
  EntitySearchRequest,
  PaginatedEntityRequest,
} from "../interfaces";
import { executeFetch } from "./client/apiClientTs";
import { components } from "../model/api";

interface MetricSearchRequest extends EntitySearchRequest {
  scope?: components["schemas"]["MetricScopes"];
}

export const fetchMetricsAsync = async ({
  token,
  page,
  scope,
  searchTerm,
}: MetricSearchRequest): Promise<components["schemas"]["MetricPaginated"]> => {
  return await executeFetch({
    path: "api/Metrics",
    token,
    page,
    query: {
      "q.term": searchTerm,
      "q.scope": scope,
    },
  });
};

export const fetchMetricAsync = async ({
  token,
  id,
}: EntityRequest): Promise<components["schemas"]["Metric"]> => {
  return await executeFetch({
    path: `api/Metrics/${id}`,
    token,
  });
};

export const fetchMetricCustomersAsync = async ({
  token,
  page,
  id,
}: PaginatedEntityRequest) => {
  return await executeFetch({
    path: `api/Metrics/${id}/Customers`,
    token,
    page,
  });
};

export const fetchMetricCustomerMetricsAsync = async ({
  token,
  page,
  id,
}: PaginatedEntityRequest) => {
  return await executeFetch({
    path: `api/Metrics/${id}/CustomerMetrics`,
    token,
    page,
  });
};

interface CreateMetricRequest extends AuthenticatedRequest {
  metric: components["schemas"]["CreateMetric"];
}
export const createMetricAsync = async ({
  token,
  metric,
}: CreateMetricRequest) => {
  return await executeFetch({
    path: "api/Metrics",
    token,
    method: "post",
    body: metric,
  });
};

export const deleteMetricAsync = async ({ token, id }: DeleteRequest) => {
  return await executeFetch({
    path: `api/Metrics/${id}`,
    token,
    method: "delete",
  });
};

export const fetchCustomersMetricsAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await executeFetch({
    path: `api/Customers/${id}/Metrics`,
    token,
  });
};

interface CustomersMetricRequest extends EntityRequest {
  metricId: string | number;
  version?: number | undefined;
}
export const fetchCustomersMetricAsync = async ({
  token,
  id,
  metricId,
  version,
}: CustomersMetricRequest) => {
  return await executeFetch({
    path: `api/Customers/${id}/Metrics/${metricId}`,
    token,
    query: {
      version,
    },
  });
};

export const fetchAggregateMetricValuesNumericAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await executeFetch({
    path: `api/Metrics/${id}/AggregateMetricValuesNumeric`,
    token,
  });
};

export const fetchAggregateMetricValuesStringAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await executeFetch({
    path: `api/Metrics/${id}/AggregateMetricValuesString`,
    token,
  });
};

export const fetchDestinationsAsync = async ({ token, id }: EntityRequest) => {
  return await executeFetch({
    path: `api/Metrics/${id}/Destinations`,
    token,
  });
};

export const fetchExportCustomers = async ({ token, id }: EntityRequest) => {
  return await executeFetch({
    path: `api/Metrics/${id}/ExportCustomers`,
    token,
  });
};

export const fetchMetricBinValuesNumericAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await executeFetch({
    path: `api/Metrics/${id}/NumericMetricBinValues`,
    token,
  });
};

export const fetchMetricBinValuesStringAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await executeFetch({
    path: `api/Metrics/${id}/CategoricalMetricBinValues`,
    token,
  });
};

interface CreateMetricDestinationRequest extends EntityRequest {
  destination: components["schemas"]["CreateDestinationDto"];
}
export const createDestinationAsync = async ({
  token,
  id,
  destination,
}: CreateMetricDestinationRequest) => {
  return await executeFetch({
    path: `api/Metrics/${id}/Destinations`,
    token,
    method: "post",
    body: destination,
  });
};

interface DeleteDestinationRequest extends DeleteRequest {
  destinationId: number;
}
export const deleteDestinationAsync = async ({
  token,
  id,
  destinationId,
}: DeleteDestinationRequest) => {
  return await executeFetch({
    path: `api/Metrics/${id}/Destinations/${destinationId}`,
    token,
    method: "delete",
  });
};

export const fetchGeneratorsAsync = async ({ token, id }: EntityRequest) => {
  return await executeFetch({
    path: `api/Metrics/${id}/Generators`,
    token,
  });
};
