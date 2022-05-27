import { EntityRequest } from "../../../interfaces";
interface CommonFetchLearningMetricsRequest extends EntityRequest {
    campaignApiName: "ParameterSetRecommenders" | "ItemsRecommenders" | "PromotionsRecommenders" | "ParameterSetCampaigns" | "PromotionsCampaigns";
}
export declare const fetchLearningMetricsAsync: ({ campaignApiName, token, id, useInternalId, }: CommonFetchLearningMetricsRequest) => Promise<any>;
interface CommonSetLearningMetricsRequest extends EntityRequest {
    campaignApiName: "ParameterSetRecommenders" | "ItemsRecommenders" | "PromotionsRecommenders" | "ParameterSetCampaigns" | "PromotionsCampaigns";
    metricIds: string[];
}
export declare const setLearningMetricsAsync: ({ campaignApiName, token, id, useInternalId, metricIds, }: CommonSetLearningMetricsRequest) => Promise<any>;
export {};
