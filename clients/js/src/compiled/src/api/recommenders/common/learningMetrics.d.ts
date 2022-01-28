import { EntityRequest } from "../../../interfaces";
interface CommonFetchLearningMetricsRequest extends EntityRequest {
    recommenderApiName: "ParameterSetRecommenders" | "ItemsRecommenders";
}
export declare const fetchLearningMetricsAsync: ({ recommenderApiName, token, id, useInternalId, }: CommonFetchLearningMetricsRequest) => Promise<any>;
interface CommonSetLearningMetricsRequest extends EntityRequest {
    recommenderApiName: "ParameterSetRecommenders" | "ItemsRecommenders";
    metricIds: string[];
}
export declare const setLearningMetricsAsync: ({ recommenderApiName, token, id, useInternalId, metricIds, }: CommonSetLearningMetricsRequest) => Promise<any>;
export {};
