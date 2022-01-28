import {
  AuthenticatedRequest,
  DeleteRequest,
  EntityRequest,
  PaginatedRequest,
} from "../interfaces";
import { executeFetch } from "./client/apiClientTs";
import { components } from "../model/api";

export const fetchMetricGeneratorsAsync = async ({
  page,
  token,
}: PaginatedRequest) => {
  return await executeFetch({
    path: "api/MetricGenerators",
    token,
    page,
  });
};

interface CreateMetricGeneratorRequest extends AuthenticatedRequest {
  generator: components["schemas"]["CreateMetricGenerator"];
}
export const createMetricGeneratorAsync = async ({
  token,
  generator,
}: CreateMetricGeneratorRequest) => {
  return await executeFetch({
    path: "api/MetricGenerators",
    token,
    method: "post",
    body: generator,
  });
};

export const deleteMetricGeneratorAsync = async ({
  token,
  id,
}: DeleteRequest) => {
  return await executeFetch({
    path: `api/MetricGenerators/${id}`,
    token,
    method: "delete",
  });
};

export const manualTriggerMetricGeneratorsAsync = async ({
  token,
  id,
}: EntityRequest) => {
  return await executeFetch({
    token,
    path: `api/MetricGenerators/${id}/Trigger`,
    method: "post",
    body: {},
  });
};
