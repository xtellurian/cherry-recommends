interface ReportImageRequest {
    recommenderApiName: "PromotionsRecommenders" | "ParameterSetRecommenders" | "ItemsRecommenders";
    token: string;
    id: string | number;
    useInternalId?: boolean;
}
export declare const fetchReportImageBlobUrlAsync: ({ recommenderApiName, token, id, useInternalId, }: ReportImageRequest) => Promise<string | undefined>;
export {};
