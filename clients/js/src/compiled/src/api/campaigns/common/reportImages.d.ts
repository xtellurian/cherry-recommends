interface ReportImageRequest {
    campaignApiName: "PromotionsRecommenders" | "ParameterSetRecommenders" | "ItemsRecommenders" | "ParameterSetCampaigns" | "PromotionsCampaigns";
    token: string;
    id: string | number;
    useInternalId?: boolean;
}
export declare const fetchReportImageBlobUrlAsync: ({ campaignApiName, token, id, useInternalId, }: ReportImageRequest) => Promise<string | undefined>;
export {};
