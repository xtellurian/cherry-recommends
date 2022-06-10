import { AxiosInstance, AxiosResponse } from 'axios';

interface AuthenticatedRequest {
    token: string;
    useInternalId?: boolean | undefined;
}
interface PaginatedRequest extends AuthenticatedRequest {
    page: number;
}
interface PaginateResponse<T> {
    items: T[];
    pagination: {
        pageCount: number;
        totalItemCount: number;
        pageNumber: number;
        hasPreviousPage: boolean;
        hasNextPage: boolean;
        isFirstPage: boolean;
        isLastPage: boolean;
    };
}
interface EntityRequest extends AuthenticatedRequest {
    id: number | string;
    useInternalId: boolean | undefined;
}
interface PaginatedEntityRequest extends EntityRequest {
    page?: number | undefined;
    pageSize?: number | undefined;
}
interface EntitySearchRequest extends PaginatedEntityRequest {
    searchTerm: string | undefined;
}
interface DeleteRequest extends AuthenticatedRequest {
    id: number | string;
}
interface DeleteResponse {
    id: number;
    resourceUrl: string;
    success: boolean;
}
interface SetpropertiesRequest extends EntityRequest {
    properties: any;
}
interface ModelInput {
    commonUserId: string;
    arguments: any | undefined;
}
interface Entity {
    id: number;
}
interface CommonEntity extends Entity {
    commonId: string;
    properties: any;
}
interface RecommendableItem extends CommonEntity {
}
interface Promotion extends RecommendableItem {
}
interface Customer extends CommonEntity {
}
interface ScoredItem {
    itemId: number | undefined;
    itemCommonId: string | undefined;
    commonId: string | undefined;
    item: RecommendableItem;
    score: number;
}
interface ItemsRecommendation {
    created: string;
    correlatorId: null;
    commonUserId: string;
    scoredItems: ScoredItem[];
    customer: Customer;
    trigger: string;
}
interface PromotionsRecommendation extends ItemsRecommendation {
}
interface Business extends CommonEntity {
}
interface MetricBinRequest extends EntityRequest {
    binCount: number;
}
interface PromotionsRequest extends EntitySearchRequest {
    promotionType: string | undefined;
    benefitType: string | undefined;
    weeksAgo: number | undefined;
}
interface Channel$2 extends Entity {
}

interface components {
    schemas: {
        /** Average Purchase Value report */
        APVReportDto: {
            /** Campaign id. */
            campaignId?: number;
            type?: components["schemas"]["DateTimePeriod"];
            /** Data for the report. */
            data?: components["schemas"]["OfferMeanGrossRevenue"][] | null;
        };
        /** Average Revenue Per Offer report */
        ARPOReportDto: {
            /** Campaign id. */
            campaignId?: number;
            type?: components["schemas"]["DateTimePeriod"];
            /** Data for the ARPO report. */
            data?: components["schemas"]["OfferMeanGrossRevenue"][] | null;
        };
        ActivityFeedEntity: {
            activityKind?: components["schemas"]["ActivityKinds"];
            activityItems?: components["schemas"]["ObjectPaginated"];
        };
        ActivityKinds: "event" | "recommendation";
        AddCampaignChannelDto: {
            id?: number;
        };
        AddMemberDto: {
            commonId: string;
            name?: string | null;
            useInternalId?: boolean | null;
        };
        AddPromotionDto: {
            id?: number | null;
            commonId?: string | null;
        };
        AggregateCustomerMetric: {
            metricId?: number;
            metric?: components["schemas"]["Metric"];
            aggregationType?: components["schemas"]["AggregationTypes"];
            categoricalValue?: string | null;
        };
        AggregateCustomerMetricDto: {
            metricId: number;
            aggregationType: components["schemas"]["AggregationTypes"];
            categoricalValue?: string | null;
        };
        AggregateStep: {
            aggregationType?: components["schemas"]["AggregationTypes"];
        };
        AggregationTypes: "sum" | "mean";
        ApiKeyDto: {
            id?: number;
            name?: string | null;
            lastExchanged?: string | null;
            totalExchanges?: number;
            apiKeyType?: components["schemas"]["ApiKeyTypes"];
        };
        ApiKeyDtoPaginated: {
            items?: components["schemas"]["ApiKeyDto"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        ApiKeyExchangeRequestDto: {
            apiKey: string;
        };
        ApiKeyExchangeResponseDto: {
            access_token?: string | null;
        };
        ApiKeyTypes: "server" | "web";
        ArgumentRule: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            discriminator?: string | null;
            campaignId?: number;
            campaign?: components["schemas"]["CampaignEntityBase"];
            argumentId?: number;
            argument?: components["schemas"]["CampaignArgument"];
        };
        ArgumentTypes: "numerical" | "categorical";
        Audience: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            recommenderId?: number;
            recommender?: components["schemas"]["CampaignEntityBase"];
            segments?: components["schemas"]["Segment"][] | null;
        };
        Auth0ReactConfig: {
            defaultAudience?: string | null;
            audience?: string | null;
            clientId?: string | null;
            clientSecret?: string | null;
            endpoint?: string | null;
            domain?: string | null;
            managementAudience?: string | null;
            scope?: string | null;
        };
        AzureMLClassifierOutput: {
            result?: string | null;
            correlatorId?: number | null;
        };
        AzureMLModelInput: {
            version?: string | null;
            data?: {
                [key: string]: unknown;
            }[] | null;
            customerId?: string | null;
            commonUserId?: string | null;
            businessId?: string | null;
            arguments?: {
                [key: string]: unknown;
            } | null;
            metrics?: {
                [key: string]: unknown;
            } | null;
            features?: {
                [key: string]: unknown;
            } | null;
            parameterBounds?: components["schemas"]["ParameterBounds"][] | null;
        };
        BaselinePromotionDto: {
            itemId?: string | null;
            promotionId?: string | null;
        };
        BatchCreateOrUpdateCustomersDto: {
            users?: components["schemas"]["CreateOrUpdateCustomerDto"][] | null;
            customers?: components["schemas"]["CreateOrUpdateCustomerDto"][] | null;
        };
        Bearer: {
            name?: string | null;
            in?: string | null;
            description?: string | null;
            schema?: components["schemas"]["Schema"];
            type?: string | null;
        };
        BenefitType: "percent" | "fixed";
        BillingAccount: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            tenants?: components["schemas"]["Tenant"][] | null;
            planType?: components["schemas"]["PlanTypes"];
        };
        Business: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            name?: string | null;
            commonId?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
            description?: string | null;
        };
        BusinessMembership: {
            businessId?: number;
            business?: components["schemas"]["Business"];
            customerId?: number;
        };
        BusinessMetricValue: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            metricId?: number;
            feature?: components["schemas"]["Metric"];
            metric?: components["schemas"]["Metric"];
            numericValue?: number | null;
            stringValue?: string | null;
            value?: unknown | null;
            version?: number;
            discriminator?: string | null;
            businessId?: number;
            business?: components["schemas"]["Business"];
        };
        BusinessPaginated: {
            items?: components["schemas"]["Business"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        CampaignArgument: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            campaignId?: number;
            campaign?: components["schemas"]["CampaignEntityBase"];
            commonId?: string | null;
            argumentType?: components["schemas"]["ArgumentTypes"];
            isRequired?: boolean;
        };
        CampaignEntityBase: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            name?: string | null;
            commonId?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
            maxChannelCount?: number;
            errorHandling?: components["schemas"]["CampaignErrorHandling"];
            settings?: components["schemas"]["CampaignSettings"];
            oldArguments?: components["schemas"]["OldRecommenderArgument"][] | null;
            triggerCollection?: components["schemas"]["TriggerCollection"];
            modelRegistrationId?: number | null;
            modelRegistration?: components["schemas"]["ModelRegistration"];
        };
        CampaignErrorHandling: {
            enabled?: boolean;
            throwOnBadInput?: boolean | null;
            requireConsumptionEvent?: boolean | null;
            recommendationCacheTime?: components["schemas"]["TimeSpan"];
            expiryDate?: string | null;
        };
        CampaignSettings: {
            enabled?: boolean;
            throwOnBadInput?: boolean | null;
            requireConsumptionEvent?: boolean | null;
            recommendationCacheTime?: components["schemas"]["TimeSpan"];
            expiryDate?: string | null;
        };
        CampaignSettingsDto: {
            enabled?: boolean | null;
            throwOnBadInput?: boolean | null;
            requireConsumptionEvent?: boolean | null;
            recommendationCacheTime?: components["schemas"]["TimeSpan"];
            /** Date after which no more recommendations will be produced */
            expiryDate?: string | null;
        };
        CampaignStatistics: {
            numberCustomersRecommended?: number;
            numberInvokations?: number;
        };
        CampaignTypes: "product" | "parameterSet" | "items" | "offer";
        CategoricalParameterBounds: {
            categories?: string[] | null;
        };
        CategoricalPredicate: {
            predicateOperator?: components["schemas"]["CategoricalPredicateOperators"];
            compareTo?: string | null;
        };
        CategoricalPredicateOperators: "none" | "equal" | "notEqual";
        ChannelBase: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            name?: string | null;
            channelType?: components["schemas"]["ChannelTypes"];
            linkedIntegratedSystemId?: number;
            linkedIntegratedSystem?: components["schemas"]["IntegratedSystem"];
            discriminator?: string | null;
            lastEnqueued?: string | null;
            lastCompleted?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
            recommenders?: components["schemas"]["CampaignEntityBase"][] | null;
        };
        ChannelBasePaginated: {
            items?: components["schemas"]["ChannelBase"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        ChannelTypes: "webhook" | "email" | "web";
        CheckistItem: {
            complete?: boolean | null;
            current?: boolean | null;
            next?: boolean | null;
            order?: number | null;
            label?: string | null;
            description?: string | null;
            actionTo?: string | null;
            actionLabel?: string | null;
            docsLink?: string | null;
        };
        ChoosePromotionArgumentRule: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            discriminator?: string | null;
            campaignId?: number;
            campaign?: components["schemas"]["CampaignEntityBase"];
            argumentId?: number;
            argument?: components["schemas"]["CampaignArgument"];
            argumentValue?: string | null;
            promotionId?: number;
            promotion?: components["schemas"]["RecommendableItem"];
        };
        ChooseSegmentArgumentRule: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            discriminator?: string | null;
            campaignId?: number;
            campaign?: components["schemas"]["CampaignEntityBase"];
            argumentId?: number;
            argument?: components["schemas"]["CampaignArgument"];
            argumentValue?: string | null;
            segmentId?: number;
            segment?: components["schemas"]["Segment"];
        };
        CreateApiKeyDto: {
            name: string;
            apiKeyType: string;
            scope?: string | null;
        };
        CreateApiKeyResponseDto: {
            name?: string | null;
            apiKey?: string | null;
        };
        CreateBusinessDto: {
            commonId: string;
            name?: string | null;
            description?: string | null;
        };
        CreateChannelDto: {
            name: string;
            channelType: components["schemas"]["ChannelTypes"];
            integratedSystemId: number;
        };
        CreateChoosePromotionArgumentRuleDto: {
            argumentValue: string;
            argumentId: number;
            promotionId: number;
        };
        CreateChooseSegmentArgumentRuleDto: {
            argumentValue: string;
            argumentId: number;
            segmentId: number;
        };
        CreateCustomerMetric: {
            value: unknown;
        };
        CreateDestinationDto: {
            destinationType: string;
            integratedSystemId: number;
            endpoint?: string | null;
            propertyName?: string | null;
        };
        CreateEnvironment: {
            name: string;
        };
        CreateIntegratedSystemDto: {
            name: string;
            systemType: string;
        };
        CreateMetric: {
            commonId: string;
            name?: string | null;
            valueType: components["schemas"]["MetricValueType"];
            scope: components["schemas"]["MetricScopes"];
        };
        CreateMetricEnrolmentRuleDto: {
            metricId: number;
            numericPredicate?: components["schemas"]["NumericPredicate"];
            categoricalPredicate?: components["schemas"]["CategoricalPredicate"];
        };
        CreateMetricGenerator: {
            featureCommonId?: string | null;
            metricCommonId: string;
            generatorType: components["schemas"]["MetricGeneratorTypes"];
            steps?: components["schemas"]["FilterSelectAggregateStepDto"][] | null;
            aggregateCustomerMetric?: components["schemas"]["AggregateCustomerMetricDto"];
            joinTwoMetrics?: components["schemas"]["JoinTwoMetricsDto"];
            timeWindow?: components["schemas"]["MetricGeneratorTimeWindow"];
        };
        CreateOrUpdateCampaignArgument: {
            commonId: string;
            argumentType?: components["schemas"]["ArgumentTypes"];
            isRequired?: boolean;
        };
        CreateOrUpdateCustomerDto: {
            commonUserId?: string | null;
            customerId?: string | null;
            name?: string | null;
            email?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
            integratedSystemReference?: components["schemas"]["IntegratedSystemReference"];
        };
        CreateParameter: {
            commonId: string;
            name?: string | null;
            /** One of Categorical or Numeric */
            parameterType?: string | null;
            description?: string | null;
            defaultValue?: unknown | null;
        };
        CreateParameterSetCampaign: {
            commonId: string;
            name: string;
            cloneFromId?: number | null;
            /** @deprecated */
            throwOnBadInput?: boolean | null;
            /** @deprecated */
            requireConsumptionEvent?: boolean | null;
            settings?: components["schemas"]["CampaignSettingsDto"];
            arguments?: components["schemas"]["CreateOrUpdateCampaignArgument"][] | null;
            targetMetricId?: string | null;
            segmentIds?: number[] | null;
            channelIds?: number[] | null;
            parameters?: string[] | null;
            bounds?: components["schemas"]["ParameterBounds"][] | null;
        };
        CreatePromotionDto: {
            commonId: string;
            name?: string | null;
            directCost: number;
            benefitType: components["schemas"]["BenefitType"];
            benefitValue: number;
            promotionType: components["schemas"]["PromotionType"];
            numberOfRedemptions: number;
            description?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
        };
        CreatePromotionsCampaign: {
            commonId: string;
            name: string;
            cloneFromId?: number | null;
            /** @deprecated */
            throwOnBadInput?: boolean | null;
            /** @deprecated */
            requireConsumptionEvent?: boolean | null;
            settings?: components["schemas"]["CampaignSettingsDto"];
            arguments?: components["schemas"]["CreateOrUpdateCampaignArgument"][] | null;
            targetMetricId?: string | null;
            segmentIds?: number[] | null;
            channelIds?: number[] | null;
            itemIds: string[];
            defaultItemId?: string | null;
            baselineItemId?: string | null;
            baselinePromotionId?: string | null;
            numberOfItemsToRecommend?: number | null;
            useAutoAi?: boolean | null;
            targetType?: components["schemas"]["PromotionCampaignTargetTypes"];
        };
        CreateSegmentDto: {
            name?: string | null;
        };
        CreateTenantMembershipDto: {
            email: string;
        };
        Customer: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            name?: string | null;
            commonId?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
            commonUserId?: string | null;
            customerId?: string | null;
            email?: string | null;
            integratedSystemMaps?: components["schemas"]["TrackedUserSystemMap"][] | null;
            businessMembership?: components["schemas"]["BusinessMembership"];
        };
        CustomerEvent: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            commonUserId?: string | null;
            customerId?: string | null;
            eventId?: string | null;
            timestamp?: string;
            recommendationCorrelatorId?: number | null;
            recommendationCorrelator?: components["schemas"]["RecommendationCorrelator"];
            source?: components["schemas"]["IntegratedSystem"];
            eventKind?: components["schemas"]["EventKinds"];
            eventType?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
            trackedUserId?: number | null;
            trackedUser?: components["schemas"]["Customer"];
            customer?: components["schemas"]["Customer"];
        };
        CustomerEventKindSummary: {
            kind?: components["schemas"]["EventKinds"];
            summary?: components["schemas"]["EventKindSummary"];
        };
        CustomerEventPaginated: {
            items?: components["schemas"]["CustomerEvent"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        CustomerEventSummary: {
            keys?: components["schemas"]["EventKinds"][] | null;
            kinds?: {
                [key: string]: components["schemas"]["EventKindSummary"];
            } | null;
        };
        CustomerMetricWeeklyNumericAggregate: {
            firstOfWeek?: string;
            lastOfWeek?: string;
            metricId?: number;
            weeklyMeanNumericValue?: number;
            weeklyDistinctCustomerCount?: number;
            weeklyDistinctBusinessCount?: number;
        };
        CustomerMetricWeeklyStringAggregate: {
            firstOfWeek?: string;
            lastOfWeek?: string;
            metricId?: number;
            stringValue?: string | null;
            weeklyValueCount?: number;
            weeklyDistinctCustomerCount?: number;
            weeklyDistinctBusinessCount?: number;
        };
        CustomerPaginated: {
            items?: components["schemas"]["Customer"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        Data: {
            type?: string | null;
            items?: components["schemas"]["DataItems"];
        };
        DataItems: {
            type?: string | null;
            required?: string[] | null;
            properties?: components["schemas"]["ItemsProperties"];
        };
        DateTimePeriod: "daily" | "weekly" | "monthly";
        Default: {
            description?: string | null;
            schema?: components["schemas"]["Schema"];
        };
        DefaultArgumentContainer: {
            argumentType?: components["schemas"]["ArgumentTypes"];
            value?: unknown | null;
        };
        DefaultParameterValue: {
            parameterType?: components["schemas"]["ParameterTypes"];
            value?: unknown | null;
        };
        DeferredDelivery: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            sending?: boolean | null;
            channelId?: number;
            channel?: components["schemas"]["ChannelBase"];
            recommendationId?: number;
            recommendation?: components["schemas"]["ItemsRecommendation"];
        };
        Definitions: {
            ServiceInput?: components["schemas"]["ServiceInput"];
            ServiceOutput?: components["schemas"]["ServiceOutput"];
            ErrorResponse?: components["schemas"]["ErrorResponse"];
        };
        DeleteResponse: {
            id?: number;
            resouceUrl?: string | null;
            success?: boolean;
        };
        DestinationTrigger: {
            [key: string]: unknown;
        };
        DiscountCode: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            code?: string | null;
            startsAt?: string | null;
            endsAt?: string | null;
            promotionId?: number;
            generatedAt?: components["schemas"]["IntegratedSystem"][] | null;
        };
        Empty: {
            get?: components["schemas"]["Get"];
        };
        EnrolmentReport: {
            segmentId?: number;
            ruleId?: number;
            rule?: components["schemas"]["EnrolmentRule"];
            customersEnrolled?: number;
        };
        EnrolmentRule: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            discriminator?: string | null;
            segmentId?: number;
            lastEnqueued?: string | null;
            lastCompleted?: string | null;
        };
        Environment: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            name?: string | null;
        };
        EnvironmentPaginated: {
            items?: components["schemas"]["Environment"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        ErrorResponse: {
            type?: string | null;
            properties?: components["schemas"]["ErrorResponseProperties"];
        };
        ErrorResponseProperties: {
            status_code?: components["schemas"]["StatusCodeClass"];
            message?: components["schemas"]["Message"];
        };
        EventCountTimeline: {
            categories?: string[] | null;
            moments?: components["schemas"]["MomentCount"][] | null;
            categoricalMoments?: {
                [key: string]: unknown;
            }[] | null;
        };
        EventDto: {
            commonUserId?: string | null;
            customerId?: string | null;
            eventId: string;
            timestamp?: string | null;
            recommendationCorrelatorId?: number | null;
            sourceSystemId?: number | null;
            kind?: components["schemas"]["EventKinds"];
            eventType: string;
            properties?: {
                [key: string]: unknown;
            } | null;
        };
        EventKindSummary: {
            keys?: string[] | null;
            instanceCount?: number;
            eventTypes?: {
                [key: string]: components["schemas"]["EventStats"];
            } | null;
        };
        EventKinds: "custom" | "propertyUpdate" | "behaviour" | "pageView" | "identify" | "consumeRecommendation" | "addToBusiness" | "purchase" | "usePromotion" | "promotionPresented";
        EventLoggingResponse: {
            eventsProcessed?: number;
            eventsEnqueued?: number;
        };
        EventStats: {
            instances?: number;
            customers?: number;
            trackedUsers?: number;
            fractionOfKind?: number;
        };
        Examples: {
            "application/json"?: string | null;
        };
        FileInformation: {
            name?: string | null;
        };
        FilterSelectAggregateStep: {
            order?: number;
            filter?: components["schemas"]["FilterStep"];
            select?: components["schemas"]["SelectStep"];
            aggregate?: components["schemas"]["AggregateStep"];
        };
        FilterSelectAggregateStepDto: {
            type: string;
            order: number;
            eventTypeMatch?: string | null;
            propertyNameMatch?: string | null;
            aggregationType?: string | null;
        };
        FilterStep: {
            eventTypeMatch?: string | null;
        };
        GeneralSummary: {
            totalCustomers?: number;
            eventCount24Hour?: number;
            recommendationCount24Hour?: number;
        };
        Get: {
            operationId?: string | null;
            description?: string | null;
            responses?: components["schemas"]["GetResponses"];
        };
        GetResponses: {
            "200"?: components["schemas"]["The200"];
            default?: components["schemas"]["Default"];
        };
        GettingStartedChecklist: {
            steps?: {
                [key: string]: components["schemas"]["CheckistItem"];
            } | null;
            allComplete?: boolean | null;
        };
        HistoricCustomerMetric: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            metricId?: number;
            feature?: components["schemas"]["Metric"];
            metric?: components["schemas"]["Metric"];
            numericValue?: number | null;
            stringValue?: string | null;
            value?: unknown | null;
            version?: number;
            discriminator?: string | null;
            customerId?: number;
            trackedUser?: components["schemas"]["Customer"];
            customer?: components["schemas"]["Customer"];
        };
        HistoricCustomerMetricPaginated: {
            items?: components["schemas"]["HistoricCustomerMetric"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        Hosting: {
            multitenant?: boolean;
            canonicalRootDomain?: string | null;
        };
        HostingTypes: "azureMLContainerInstance" | "azurePersonalizer" | "azureFunctions";
        HotjarConfig: {
            siteId?: number;
            snippetVersion?: number;
        };
        Info: {
            title?: string | null;
            description?: string | null;
            version?: string | null;
        };
        IntegratedSystem: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            name?: string | null;
            commonId?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
            systemType?: components["schemas"]["IntegratedSystemTypes"];
            integrationStatus?: components["schemas"]["IntegrationStatuses"];
            tokenResponseUpdated?: string | null;
            discriminator?: string | null;
            isDiscountCodeGenerator?: boolean;
        };
        IntegratedSystemPaginated: {
            items?: components["schemas"]["IntegratedSystem"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        IntegratedSystemReference: {
            integratedSystemId: number;
            userId: string;
        };
        IntegratedSystemTypes: "segment" | "hubspot" | "shopify" | "custom" | "website" | "klaviyo";
        IntegrationStatuses: "notConfigured" | "ok";
        InvokationLogEntry: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            status?: string | null;
            recommenderType?: string | null;
            recommenderId?: number;
            success?: boolean | null;
            modelResponse?: string | null;
            /** @deprecated */
            message?: string | null;
            messages?: string[] | null;
            invokeStarted?: string;
            invokeEnded?: string | null;
            correlatorId?: number | null;
            correlator?: components["schemas"]["RecommendationCorrelator"];
            customerId?: number | null;
            customer?: components["schemas"]["Customer"];
            businessId?: number | null;
            business?: components["schemas"]["Business"];
        };
        InvokationLogEntryPaginated: {
            items?: components["schemas"]["InvokationLogEntry"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        ItemsProperties: {
            Column2?: components["schemas"]["Message"];
            A?: components["schemas"]["StatusCodeClass"];
            B?: components["schemas"]["StatusCodeClass"];
        };
        ItemsRecommendation: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            recommenderType?: components["schemas"]["CampaignTypes"];
            customerId?: number | null;
            trackedUser?: components["schemas"]["Customer"];
            customer?: components["schemas"]["Customer"];
            businessId?: number | null;
            business?: components["schemas"]["Business"];
            targetMetricId?: number | null;
            targetMetric?: components["schemas"]["Metric"];
            trigger?: string | null;
            recommendationCorrelatorId?: number | null;
            modelInput?: string | null;
            modelInputType?: string | null;
            modelOutput?: string | null;
            modelOutputType?: string | null;
            isFromCache?: boolean;
            recommenderId?: number | null;
            recommender?: components["schemas"]["PromotionsCampaign"];
            offer?: components["schemas"]["Offer"];
            maxScoreItemId?: number | null;
            scoredItems?: components["schemas"]["ScoredRecommendableItem"][] | null;
        };
        ItemsRecommendationPaginated: {
            items?: components["schemas"]["ItemsRecommendation"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        ItemsRecommenderPerformanceReport: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            recommenderId?: number;
            recommender?: components["schemas"]["CampaignEntityBase"];
            discriminator?: string | null;
            itemsById?: {
                [key: string]: components["schemas"]["RecommendableItem"];
            } | null;
            itemsByCommonId?: {
                [key: string]: components["schemas"]["RecommendableItem"];
            } | null;
            itemsRecommender?: components["schemas"]["PromotionsCampaign"];
            targetMetric?: components["schemas"]["Metric"];
            performanceByItem?: components["schemas"]["PerformanceByItem"][] | null;
        };
        JoinTwoMetrics: {
            metric1Id?: number;
            metric1?: components["schemas"]["Metric"];
            metric2Id?: number;
            metric2?: components["schemas"]["Metric"];
            joinType?: components["schemas"]["JoinType"];
        };
        JoinTwoMetricsDto: {
            metric1Id: number;
            metric2Id: number;
            joinType: components["schemas"]["JoinType"];
        };
        JoinType: "divide";
        KlaviyoList: {
            list_id?: string | null;
            list_name?: string | null;
        };
        LaunchDarklyConfig: {
            sdkKey?: string | null;
            mobileKey?: string | null;
            clientSideId?: string | null;
        };
        LinkModel: {
            modelId?: number;
        };
        Message: {
            type?: string | null;
        };
        Metric: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            name?: string | null;
            commonId?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
            valueType?: components["schemas"]["MetricValueType"];
            scope?: components["schemas"]["MetricScopes"];
        };
        MetricDailyBinValueNumeric: {
            binFloor?: number;
            binWidth?: number;
            binRange?: string | null;
            customerCount?: number;
            businessCount?: number;
        };
        MetricDailyBinValueString: {
            stringValue?: string | null;
            customerCount?: number;
            businessCount?: number;
        };
        MetricDestinationBase: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            properties?: {
                [key: string]: string;
            } | null;
            destinationType?: string | null;
            metric?: components["schemas"]["Metric"];
            feature?: components["schemas"]["Metric"];
            connectedSystemId?: number;
            connectedSystem?: components["schemas"]["IntegratedSystem"];
            discriminator?: string | null;
        };
        MetricEnrolmentRule: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            discriminator?: string | null;
            segmentId?: number;
            lastEnqueued?: string | null;
            lastCompleted?: string | null;
            metricId?: number | null;
            metric?: components["schemas"]["Metric"];
            numericPredicate?: components["schemas"]["NumericPredicate"];
            categoricalPredicate?: components["schemas"]["CategoricalPredicate"];
        };
        MetricEnrolmentRulePaginated: {
            items?: components["schemas"]["MetricEnrolmentRule"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        MetricGenerator: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            lastEnqueued?: string | null;
            lastCompleted?: string | null;
            metricId?: number;
            metric?: components["schemas"]["Metric"];
            feature?: components["schemas"]["Metric"];
            generatorType?: components["schemas"]["MetricGeneratorTypes"];
            filterSelectAggregateSteps?: components["schemas"]["FilterSelectAggregateStep"][] | null;
            timeWindow?: components["schemas"]["MetricGeneratorTimeWindow"];
            aggregateCustomerMetric?: components["schemas"]["AggregateCustomerMetric"];
            joinTwoMetrics?: components["schemas"]["JoinTwoMetrics"];
        };
        MetricGeneratorPaginated: {
            items?: components["schemas"]["MetricGenerator"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        MetricGeneratorRunSummary: {
            enqueued?: boolean | null;
            totalWrites?: number | null;
            maxSubsetSize?: number | null;
        };
        MetricGeneratorTimeWindow: "allTime" | "sevenDays" | "thirtyDays";
        MetricGeneratorTypes: "monthsSinceEarliestEvent" | "filterSelectAggregate" | "aggregateCustomerMetric" | "joinTwoMetrics";
        MetricPaginated: {
            items?: components["schemas"]["Metric"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        MetricScopes: "customer" | "business" | "global";
        MetricValueType: "numeric" | "categorical";
        MetricsChangedTrigger: {
            name: string;
            featureCommonIds?: string[] | null;
            metricCommonIds?: string[] | null;
        };
        ModelInputDto: {
            customerId?: string | null;
            commonUserId?: string | null;
            businessId?: string | null;
            arguments?: {
                [key: string]: unknown;
            } | null;
            metrics?: {
                [key: string]: unknown;
            } | null;
            features?: {
                [key: string]: unknown;
            } | null;
            parameterBounds?: components["schemas"]["ParameterBounds"][] | null;
        };
        ModelRegistration: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            name?: string | null;
            modelType?: components["schemas"]["ModelTypes"];
            hostingType?: components["schemas"]["HostingTypes"];
            scoringUrl?: string | null;
            swagger?: components["schemas"]["SwaggerDefinition"];
        };
        ModelRegistrationPaginated: {
            items?: components["schemas"]["ModelRegistration"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        ModelTypes: "singleClassClassifier" | "parameterSetRecommenderV1" | "productRecommenderV1" | "itemsRecommenderV1";
        MomentCount: {
            category?: string | null;
            timestamp?: string;
            unixTime?: number;
            count?: number;
        };
        NewTenantDto: {
            name: string;
            termsOfServiceVersion: string;
        };
        NextPageInfo: {
            after?: string | null;
        };
        NumericPredicate: {
            predicateOperator?: components["schemas"]["NumericPredicateOperators"];
            compareTo?: number;
        };
        NumericPredicateOperators: "none" | "equal" | "notEqual" | "greaterThan" | "lessThan" | "greaterThanOrEqualTo" | "lessThanOrEqualTo";
        NumericalParameterBounds: {
            min?: number;
            max?: number;
        };
        ObjectPaginated: {
            items?: unknown[] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        Offer: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            recommendationId?: number;
            redeemedPromotionId?: number | null;
            state?: components["schemas"]["OfferState"];
            redeemedAt?: string | null;
            grossRevenue?: number | null;
            redeemedPromotion?: components["schemas"]["RecommendableItem"];
        };
        OfferConversionRateData: {
            startDate?: string;
            endDate?: string;
            redeemedCount?: number;
            nonBaselineRedeemedCount?: number;
            baselineRedeemedCount?: number;
            totalCount?: number;
            nonBaselineCount?: number;
            baselineCount?: number;
            conversionRate?: number;
            nonBaselineConversionRate?: number;
            baselineConversionRate?: number;
        };
        OfferConversionRateReportDto: {
            /** Campaign id. */
            campaignId?: number;
            type?: components["schemas"]["DateTimePeriod"];
            /** Data for the report. */
            data?: components["schemas"]["OfferConversionRateData"][] | null;
        };
        OfferMeanGrossRevenue: {
            startDate?: string;
            endDate?: string;
            totalGrossRevenue?: number;
            nonBaselineTotalGrossRevenue?: number;
            baselineTotalGrossRevenue?: number;
            meanGrossRevenue?: number;
            nonBaselineMeanGrossRevenue?: number;
            baselineMeanGrossRevenue?: number;
            distinctCustomerCount?: number;
            offerCount?: number;
            nonBaselineOfferCount?: number;
            baselineOfferCount?: number;
        };
        OfferPaginated: {
            items?: components["schemas"]["Offer"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        OfferState: "created" | "presented" | "redeemed" | "expired";
        OldRecommenderArgument: {
            commonId?: string | null;
            argumentType?: components["schemas"]["ArgumentTypes"];
            defaultValue?: components["schemas"]["DefaultArgumentContainer"];
            defaultArgumentValue?: unknown | null;
            isRequired?: boolean;
        };
        OpenApiSecurity: {
            Bearer?: unknown[] | null;
        };
        PaginationInfo: {
            pageCount?: number;
            totalItemCount?: number;
            pageNumber?: number;
            hasPreviousPage?: boolean;
            hasNextPage?: boolean;
            isFirstPage?: boolean;
            isLastPage?: boolean;
            next?: components["schemas"]["NextPageInfo"];
        };
        Parameter: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            name?: string | null;
            commonId?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
            parameterType?: components["schemas"]["ParameterTypes"];
            defaultValue?: components["schemas"]["DefaultParameterValue"];
            default?: unknown | null;
            description?: string | null;
        };
        ParameterBounds: {
            commonId?: string | null;
            numericBounds?: components["schemas"]["NumericalParameterBounds"];
            categoricalBounds?: components["schemas"]["CategoricalParameterBounds"];
        };
        ParameterPaginated: {
            items?: components["schemas"]["Parameter"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        ParameterSetCampaign: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            name?: string | null;
            commonId?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
            maxChannelCount?: number;
            errorHandling?: components["schemas"]["CampaignErrorHandling"];
            settings?: components["schemas"]["CampaignSettings"];
            oldArguments?: components["schemas"]["OldRecommenderArgument"][] | null;
            triggerCollection?: components["schemas"]["TriggerCollection"];
            modelRegistrationId?: number | null;
            modelRegistration?: components["schemas"]["ModelRegistration"];
            parameters?: components["schemas"]["Parameter"][] | null;
            parameterBounds?: components["schemas"]["ParameterBounds"][] | null;
        };
        ParameterSetCampaignPaginated: {
            items?: components["schemas"]["ParameterSetCampaign"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        ParameterSetRecommendation: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            recommenderType?: components["schemas"]["CampaignTypes"];
            customerId?: number | null;
            trackedUser?: components["schemas"]["Customer"];
            customer?: components["schemas"]["Customer"];
            businessId?: number | null;
            business?: components["schemas"]["Business"];
            targetMetricId?: number | null;
            targetMetric?: components["schemas"]["Metric"];
            trigger?: string | null;
            recommendationCorrelatorId?: number | null;
            modelInput?: string | null;
            modelInputType?: string | null;
            modelOutput?: string | null;
            modelOutputType?: string | null;
            isFromCache?: boolean;
            recommenderId?: number | null;
            recommender?: components["schemas"]["ParameterSetCampaign"];
        };
        ParameterSetRecommendationDto: {
            created?: string;
            correlatorId?: number | null;
            recommendedParameters?: {
                [key: string]: unknown;
            } | null;
            commonUserId?: string | null;
            customerId?: string | null;
            customer?: components["schemas"]["Customer"];
            business?: components["schemas"]["Business"];
            businessId?: string | null;
            trigger?: string | null;
        };
        ParameterSetRecommendationPaginated: {
            items?: components["schemas"]["ParameterSetRecommendation"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        ParameterTypes: "numerical" | "categorical";
        Paths: {
            "/"?: components["schemas"]["Empty"];
            "/score"?: components["schemas"]["Score"];
        };
        PerformanceByItem: {
            itemId?: number;
            weekIndex?: number;
            targetMetricSum?: number;
            customerCount?: number | null;
            businessCount?: number | null;
            recommendationCount?: number;
        };
        PerformanceReportDto: {
            /** Campaign id. */
            campaignId?: number;
            type?: components["schemas"]["DateTimePeriod"];
            /** Data for the ARPO report. */
            data?: components["schemas"]["OfferMeanGrossRevenue"][] | null;
        };
        PlanTypes: "none" | "freeTrial" | "usage" | "performance" | "enterprise";
        Post: {
            operationId?: string | null;
            description?: string | null;
            security?: components["schemas"]["OpenApiSecurity"][] | null;
            parameters?: components["schemas"]["Bearer"][] | null;
            responses?: components["schemas"]["PostResponses"];
        };
        PostResponses: {
            "200"?: components["schemas"]["Default"];
            default?: components["schemas"]["Default"];
        };
        ProblemDetails: {
            type?: string | null;
            title?: string | null;
            status?: number | null;
            detail?: string | null;
            instance?: string | null;
        } & {
            [key: string]: unknown;
        };
        PromotionCampaignTargetTypes: "customer" | "business";
        PromotionOptimiser: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            recommenderId?: number;
            weights?: components["schemas"]["PromotionOptimiserWeight"][] | null;
        };
        PromotionOptimiserWeight: {
            id?: number;
            weight?: number;
            segmentId?: number | null;
            promotionId?: number;
            optimiserId?: number;
        };
        PromotionType: "discount" | "gift" | "service" | "upgrade" | "other";
        PromotionsCampaign: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            name?: string | null;
            commonId?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
            maxChannelCount?: number;
            errorHandling?: components["schemas"]["CampaignErrorHandling"];
            settings?: components["schemas"]["CampaignSettings"];
            oldArguments?: components["schemas"]["OldRecommenderArgument"][] | null;
            triggerCollection?: components["schemas"]["TriggerCollection"];
            modelRegistrationId?: number | null;
            modelRegistration?: components["schemas"]["ModelRegistration"];
            baselineItemId?: number | null;
            baselineItem?: components["schemas"]["RecommendableItem"];
            defaultItem?: components["schemas"]["RecommendableItem"];
            numberOfItemsToRecommend?: number | null;
            items?: components["schemas"]["RecommendableItem"][] | null;
            targetMetricId?: number | null;
            targetMetric?: components["schemas"]["Metric"];
            targetType?: components["schemas"]["PromotionCampaignTargetTypes"];
            optimiser?: components["schemas"]["PromotionOptimiser"];
            useOptimiser?: boolean;
        };
        PromotionsCampaignPaginated: {
            items?: components["schemas"]["PromotionsCampaign"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        PromotionsRecommendationDto: {
            created?: string;
            correlatorId?: number | null;
            commonUserId?: string | null;
            scoredItems?: components["schemas"]["ScoredRecommendableItem"][] | null;
            business?: components["schemas"]["Business"];
            businessId?: string | null;
            customer?: components["schemas"]["Customer"];
            customerId?: string | null;
            trigger?: string | null;
        };
        ReactConfig: {
            segment?: components["schemas"]["SegmentConfig"];
            hotjar?: components["schemas"]["HotjarConfig"];
            launchDarkly?: components["schemas"]["LaunchDarklyConfig"];
            auth0?: components["schemas"]["Auth0ReactConfig"];
        };
        RecommendableItem: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            name?: string | null;
            commonId?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
            directCost?: number | null;
            description?: string | null;
            benefitType?: components["schemas"]["BenefitType"];
            benefitValue?: number;
            promotionType?: components["schemas"]["PromotionType"];
            numberOfRedemptions?: number;
            discriminator?: string | null;
        };
        RecommendableItemPaginated: {
            items?: components["schemas"]["RecommendableItem"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        RecommendationCorrelator: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            recommenderId?: number | null;
            recommender?: components["schemas"]["CampaignEntityBase"];
            modelRegistrationId?: number | null;
            modelRegistration?: components["schemas"]["ModelRegistration"];
        };
        RecommendationDestinationBase: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            properties?: {
                [key: string]: string;
            } | null;
            destinationType?: string | null;
            recommender?: components["schemas"]["CampaignEntityBase"];
            trigger?: components["schemas"]["DestinationTrigger"];
            connectedSystemId?: number;
            connectedSystem?: components["schemas"]["IntegratedSystem"];
            discriminator?: string | null;
        };
        RegisterNewModelDto: {
            name: string;
            scoringUrl: string;
            key: string;
            swaggerUrl?: string | null;
            modelType: string;
            hostingType: string;
        };
        Schema: {
            $ref?: string | null;
        };
        Score: {
            post?: components["schemas"]["Post"];
        };
        ScoredRecommendableItem: {
            itemId?: number | null;
            itemCommonId?: string | null;
            commonId?: string | null;
            item?: components["schemas"]["RecommendableItem"];
            discountCodes?: components["schemas"]["DiscountCode"][] | null;
            score?: number | null;
        };
        SecurityDefinitions: {
            Bearer?: components["schemas"]["Bearer"];
        };
        Segment: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            name?: string | null;
        };
        SegmentConfig: {
            writeKey?: string | null;
        };
        SegmentPaginated: {
            items?: components["schemas"]["Segment"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        SelectStep: {
            propertyNameMatch?: string | null;
        };
        ServiceInput: {
            type?: string | null;
            properties?: components["schemas"]["ServiceInputProperties"];
            example?: components["schemas"]["AzureMLModelInput"];
        };
        ServiceInputProperties: {
            data?: components["schemas"]["Data"];
        };
        ServiceOutput: {
            type?: string | null;
            items?: components["schemas"]["StatusCodeClass"];
            example?: number[] | null;
        };
        SetKlaviyoApiKeysDto: {
            publicKey: string;
            privateKey: string;
        };
        SetLearningMetrics: {
            useInternalId?: boolean | null;
            featureIds?: string[] | null;
            metricIds?: string[] | null;
        };
        SetTriggersDto: {
            featuresChanged?: components["schemas"]["MetricsChangedTrigger"];
            metricsChanged?: components["schemas"]["MetricsChangedTrigger"];
        };
        StatusCodeClass: {
            type?: string | null;
            format?: string | null;
        };
        StatusDto: {
            status?: string | null;
        };
        SwaggerDefinition: {
            swagger?: string | null;
            info?: components["schemas"]["Info"];
            schemes?: string[] | null;
            consumes?: string[] | null;
            produces?: string[] | null;
            securityDefinitions?: components["schemas"]["SecurityDefinitions"];
            paths?: components["schemas"]["Paths"];
            definitions?: components["schemas"]["Definitions"];
        };
        Tenant: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            name?: string | null;
            databaseName?: string | null;
            status?: string | null;
        };
        The200: {
            description?: string | null;
            schema?: components["schemas"]["Message"];
            examples?: components["schemas"]["Examples"];
        };
        TimeSpan: {
            ticks?: number;
            days?: number;
            hours?: number;
            milliseconds?: number;
            minutes?: number;
            seconds?: number;
            totalDays?: number;
            totalHours?: number;
            totalMilliseconds?: number;
            totalMinutes?: number;
            totalSeconds?: number;
        };
        TrackedUserSystemMap: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            userId?: string | null;
            integratedSystemId?: number;
        };
        TriggerCollection: {
            featuresChanged?: components["schemas"]["MetricsChangedTrigger"];
            metricsChanged?: components["schemas"]["MetricsChangedTrigger"];
        };
        UpdateChoosePromotionArgumentRuleDto: {
            argumentValue: string;
            promotionId: number;
        };
        UpdateChooseSegmentArgumentRuleDto: {
            argumentValue: string;
            segmentId: number;
        };
        UpdateEmailChannelTriggerDto: {
            /** Id of the List that triggers the Email flow */
            listId?: string | null;
            /** Name of the List that triggers the Email flow */
            listName?: string | null;
        };
        UpdatePromotionDto: {
            name: string;
            directCost: number;
            benefitType: components["schemas"]["BenefitType"];
            benefitValue: number;
            promotionType: components["schemas"]["PromotionType"];
            numberOfRedemptions: number;
            description?: string | null;
            properties?: {
                [key: string]: unknown;
            } | null;
        };
        UpdateWebChannelPropertiesDto: {
            host?: string | null;
            popupAskForEmail?: boolean | null;
            popupDelay?: number | null;
            recommenderId?: number | null;
            popupHeader?: string | null;
            popupSubheader?: string | null;
            customerIdPrefix?: string | null;
        };
        UpdateWeightDto: {
            id?: number;
            weight: number;
        };
        UseOptimiserDto: {
            useOptimiser: boolean;
        };
        UserInfo: {
            email?: string | null;
            emailVerified?: boolean | null;
            userId?: string | null;
            invitationUrl?: string | null;
        };
        UserInfoPaginated: {
            items?: components["schemas"]["UserInfo"][] | null;
            pagination?: components["schemas"]["PaginationInfo"];
        };
        UserMetadata: {
            gettingStartedChecklist?: components["schemas"]["GettingStartedChecklist"];
        };
        WebhookReceiver: {
            id?: number;
            created?: string;
            lastUpdated?: string;
            environmentId?: number | null;
            environment?: components["schemas"]["Environment"];
            endpointId?: string | null;
            sharedSecret?: string | null;
        };
    };
}

declare type ActivityFeedEntities = components["schemas"]["ActivityFeedEntity"][];
declare const fetchActivityFeedEntitiesAsync: ({ token, page, }: EntitySearchRequest) => Promise<ActivityFeedEntities>;

declare const activityFeedApi_d_fetchActivityFeedEntitiesAsync: typeof fetchActivityFeedEntitiesAsync;
declare namespace activityFeedApi_d {
  export {
    activityFeedApi_d_fetchActivityFeedEntitiesAsync as fetchActivityFeedEntitiesAsync,
  };
}

declare const fetchApiKeysAsync: ({ token, page }: PaginatedRequest) => Promise<any>;
declare enum ApiKeyType {
    Server = "Server",
    Web = "Web"
}
interface CreateApiKeyPayload {
    name: string;
    apiKeyType: ApiKeyType;
}
interface CreateApiKeyRequest extends AuthenticatedRequest {
    payload: CreateApiKeyPayload;
}
declare const createApiKeyAsync: ({ token, payload, }: CreateApiKeyRequest) => Promise<any>;
interface ExchangeApiKeyRequest {
    apiKey: string;
}
interface AccessTokenResponse {
    access_token: string;
}
declare const exchangeApiKeyAsync: ({ apiKey, }: ExchangeApiKeyRequest) => Promise<AccessTokenResponse>;
declare const deleteApiKeyAsync: ({ token, id }: DeleteRequest) => Promise<any>;

declare const apiKeyApi_d_fetchApiKeysAsync: typeof fetchApiKeysAsync;
declare const apiKeyApi_d_createApiKeyAsync: typeof createApiKeyAsync;
declare const apiKeyApi_d_exchangeApiKeyAsync: typeof exchangeApiKeyAsync;
declare const apiKeyApi_d_deleteApiKeyAsync: typeof deleteApiKeyAsync;
declare namespace apiKeyApi_d {
  export {
    apiKeyApi_d_fetchApiKeysAsync as fetchApiKeysAsync,
    apiKeyApi_d_createApiKeyAsync as createApiKeyAsync,
    apiKeyApi_d_exchangeApiKeyAsync as exchangeApiKeyAsync,
    apiKeyApi_d_deleteApiKeyAsync as deleteApiKeyAsync,
  };
}

declare const fetchBusinessesAsync: ({ token, page, searchTerm, }: EntitySearchRequest) => Promise<PaginateResponse<Business>>;
declare const fetchBusinessAsync: ({ token, id, }: EntityRequest) => Promise<components["schemas"]["Business"]>;
declare const deleteBusinessAsync: ({ token, id }: DeleteRequest) => Promise<any>;
interface CreateBusinessRequest extends AuthenticatedRequest {
    business: components["schemas"]["CreateBusinessDto"];
}
declare const createBusinessAsync: ({ token, business, }: CreateBusinessRequest) => Promise<any>;
interface UpdateBusinessPropertiesRequest extends EntityRequest {
    properties?: {
        [key: string]: unknown;
    } | null;
}
declare const updateBusinessPropertiesAsync: ({ token, id, properties, }: UpdateBusinessPropertiesRequest) => Promise<any>;
declare const fetchBusinessMembersAsync: ({ token, id, page, searchTerm, }: EntitySearchRequest) => Promise<PaginateResponse<Business>>;
interface DeleteBusinessMemberRequest extends DeleteRequest {
    customerId: number;
}
declare const deleteBusinessMemberAsync: ({ token, id, customerId, }: DeleteBusinessMemberRequest) => Promise<any>;
interface AddBusinessMemberRequest extends EntityRequest {
    customer: components["schemas"]["Customer"];
}
declare const addBusinessMemberAsync: ({ token, id, customer, }: AddBusinessMemberRequest) => Promise<any>;
declare const fetchRecommendationsAsync: ({ token, id, }: EntityRequest) => Promise<components["schemas"]["ItemsRecommendation"]>;

declare const businessesApi_d_fetchBusinessesAsync: typeof fetchBusinessesAsync;
declare const businessesApi_d_fetchBusinessAsync: typeof fetchBusinessAsync;
declare const businessesApi_d_deleteBusinessAsync: typeof deleteBusinessAsync;
declare const businessesApi_d_createBusinessAsync: typeof createBusinessAsync;
declare const businessesApi_d_updateBusinessPropertiesAsync: typeof updateBusinessPropertiesAsync;
declare const businessesApi_d_fetchBusinessMembersAsync: typeof fetchBusinessMembersAsync;
declare const businessesApi_d_deleteBusinessMemberAsync: typeof deleteBusinessMemberAsync;
declare const businessesApi_d_addBusinessMemberAsync: typeof addBusinessMemberAsync;
declare const businessesApi_d_fetchRecommendationsAsync: typeof fetchRecommendationsAsync;
declare namespace businessesApi_d {
  export {
    businessesApi_d_fetchBusinessesAsync as fetchBusinessesAsync,
    businessesApi_d_fetchBusinessAsync as fetchBusinessAsync,
    businessesApi_d_deleteBusinessAsync as deleteBusinessAsync,
    businessesApi_d_createBusinessAsync as createBusinessAsync,
    businessesApi_d_updateBusinessPropertiesAsync as updateBusinessPropertiesAsync,
    businessesApi_d_fetchBusinessMembersAsync as fetchBusinessMembersAsync,
    businessesApi_d_deleteBusinessMemberAsync as deleteBusinessMemberAsync,
    businessesApi_d_addBusinessMemberAsync as addBusinessMemberAsync,
    businessesApi_d_fetchRecommendationsAsync as fetchRecommendationsAsync,
  };
}

declare const fetchChannelsAsync: ({ token, page, }: EntitySearchRequest) => Promise<PaginateResponse<Channel$2>>;
interface CreateChannelRequest extends AuthenticatedRequest {
    channel: components["schemas"]["CreateChannelDto"];
}
declare const createChannelAsync: ({ token, channel, }: CreateChannelRequest) => Promise<any>;
declare const fetchChannelAsync: ({ token, id, }: EntityRequest) => Promise<components["schemas"]["ChannelBase"]>;
declare const deleteChannelAsync: ({ token, id }: DeleteRequest) => Promise<any>;
interface UpdateChannelEnpointRequest extends EntityRequest {
    endpoint: string;
}
declare const updateChannelEndpointAsync: ({ token, id, endpoint, }: UpdateChannelEnpointRequest) => Promise<any>;
interface UpdateChannelPropertiesRequest extends EntityRequest {
    properties: {
        popupAskForEmail: boolean;
        popupDelay: number;
        popupHeader: string;
        popupSubheader: string;
        recommenderId: number;
        customerIdPrefix: string;
    };
}
declare const updateChannelPropertiesAsync: ({ token, id, properties, }: UpdateChannelPropertiesRequest) => Promise<any>;
interface UpdateEmailChannelTriggerRequest extends EntityRequest {
    listTrigger: {
        listId: string;
        listName: string;
    };
}
declare const updateEmailChannelTriggerAsync: ({ token, id, listTrigger, }: UpdateEmailChannelTriggerRequest) => Promise<any>;

declare const channelsApi_d_fetchChannelsAsync: typeof fetchChannelsAsync;
declare const channelsApi_d_createChannelAsync: typeof createChannelAsync;
declare const channelsApi_d_fetchChannelAsync: typeof fetchChannelAsync;
declare const channelsApi_d_deleteChannelAsync: typeof deleteChannelAsync;
declare const channelsApi_d_updateChannelEndpointAsync: typeof updateChannelEndpointAsync;
declare const channelsApi_d_updateChannelPropertiesAsync: typeof updateChannelPropertiesAsync;
declare const channelsApi_d_updateEmailChannelTriggerAsync: typeof updateEmailChannelTriggerAsync;
declare namespace channelsApi_d {
  export {
    channelsApi_d_fetchChannelsAsync as fetchChannelsAsync,
    channelsApi_d_createChannelAsync as createChannelAsync,
    channelsApi_d_fetchChannelAsync as fetchChannelAsync,
    channelsApi_d_deleteChannelAsync as deleteChannelAsync,
    channelsApi_d_updateChannelEndpointAsync as updateChannelEndpointAsync,
    channelsApi_d_updateChannelPropertiesAsync as updateChannelPropertiesAsync,
    channelsApi_d_updateEmailChannelTriggerAsync as updateEmailChannelTriggerAsync,
  };
}

interface InitialiseAxiosConfig {
    baseUrl: string;
    tenant?: string | null;
    timeout?: number;
}
declare const current: (config?: InitialiseAxiosConfig | undefined) => AxiosInstance;

declare const axiosInstance_d_current: typeof current;
declare namespace axiosInstance_d {
  export {
    axiosInstance_d_current as current,
  };
}

declare function fetchCustomersAsync({ token, page, searchTerm }: {
    token: any;
    page: any;
    searchTerm: any;
}): Promise<any>;
declare function updateMergePropertiesAsync$1({ token, id, properties }: {
    token: any;
    id: any;
    properties: any;
}): Promise<any>;
declare function fetchCustomerAsync({ token, id, useInternalId }: {
    token: any;
    id: any;
    useInternalId: any;
}): Promise<any>;
declare function fetchUniqueCustomerActionGroupsAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchLatestRecommendationsAsync$1({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchCustomerActionAsync({ token, id, category, actionName, }: {
    token: any;
    id: any;
    category: any;
    actionName: any;
}): Promise<any>;
declare function uploadUserDataAsync$1({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;
declare function createOrUpdateCustomerAsync({ token, customer, user, }: {
    token: any;
    customer: any;
    user: any;
}): Promise<any>;
declare function fetchCustomersActionsAsync({ token, page, id, revenueOnly, }: {
    token: any;
    page: any;
    id: any;
    revenueOnly: any;
}): Promise<any>;
declare function setCustomerMetricAsync({ token, id, metricId, useInternalId, value, }: {
    token: any;
    id: any;
    metricId: any;
    useInternalId: any;
    value: any;
}): Promise<any>;
declare function deleteCustomerAsync$1({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchCustomerSegmentsAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;

declare const customersApi_d_fetchCustomersAsync: typeof fetchCustomersAsync;
declare const customersApi_d_fetchCustomerAsync: typeof fetchCustomerAsync;
declare const customersApi_d_fetchUniqueCustomerActionGroupsAsync: typeof fetchUniqueCustomerActionGroupsAsync;
declare const customersApi_d_fetchCustomerActionAsync: typeof fetchCustomerActionAsync;
declare const customersApi_d_createOrUpdateCustomerAsync: typeof createOrUpdateCustomerAsync;
declare const customersApi_d_fetchCustomersActionsAsync: typeof fetchCustomersActionsAsync;
declare const customersApi_d_setCustomerMetricAsync: typeof setCustomerMetricAsync;
declare const customersApi_d_fetchCustomerSegmentsAsync: typeof fetchCustomerSegmentsAsync;
declare namespace customersApi_d {
  export {
    customersApi_d_fetchCustomersAsync as fetchCustomersAsync,
    updateMergePropertiesAsync$1 as updateMergePropertiesAsync,
    customersApi_d_fetchCustomerAsync as fetchCustomerAsync,
    customersApi_d_fetchUniqueCustomerActionGroupsAsync as fetchUniqueCustomerActionGroupsAsync,
    fetchLatestRecommendationsAsync$1 as fetchLatestRecommendationsAsync,
    customersApi_d_fetchCustomerActionAsync as fetchCustomerActionAsync,
    uploadUserDataAsync$1 as uploadUserDataAsync,
    customersApi_d_createOrUpdateCustomerAsync as createOrUpdateCustomerAsync,
    customersApi_d_fetchCustomersActionsAsync as fetchCustomersActionsAsync,
    customersApi_d_setCustomerMetricAsync as setCustomerMetricAsync,
    deleteCustomerAsync$1 as deleteCustomerAsync,
    customersApi_d_fetchCustomerSegmentsAsync as fetchCustomerSegmentsAsync,
  };
}

declare function fetchEventSummaryAsync({ token }: {
    token: any;
}): Promise<any>;
declare function fetchEventKindNamesAsync({ token }: {
    token: any;
}): Promise<any>;
declare function fetchEventKindSummaryAsync({ token, kind }: {
    token: any;
    kind: any;
}): Promise<any>;
declare function fetchEventTimelineAsync({ token, kind, eventType }: {
    token: any;
    kind: any;
    eventType: any;
}): Promise<any>;
declare function fetchGeneralSummaryAsync({ token }: {
    token: any;
}): Promise<any>;
declare function fetchLatestActionsAsync({ token }: {
    token: any;
}): Promise<any>;

declare const dataSummaryApi_d_fetchEventSummaryAsync: typeof fetchEventSummaryAsync;
declare const dataSummaryApi_d_fetchEventKindNamesAsync: typeof fetchEventKindNamesAsync;
declare const dataSummaryApi_d_fetchEventKindSummaryAsync: typeof fetchEventKindSummaryAsync;
declare const dataSummaryApi_d_fetchEventTimelineAsync: typeof fetchEventTimelineAsync;
declare const dataSummaryApi_d_fetchGeneralSummaryAsync: typeof fetchGeneralSummaryAsync;
declare const dataSummaryApi_d_fetchLatestActionsAsync: typeof fetchLatestActionsAsync;
declare namespace dataSummaryApi_d {
  export {
    dataSummaryApi_d_fetchEventSummaryAsync as fetchEventSummaryAsync,
    dataSummaryApi_d_fetchEventKindNamesAsync as fetchEventKindNamesAsync,
    dataSummaryApi_d_fetchEventKindSummaryAsync as fetchEventKindSummaryAsync,
    dataSummaryApi_d_fetchEventTimelineAsync as fetchEventTimelineAsync,
    dataSummaryApi_d_fetchGeneralSummaryAsync as fetchGeneralSummaryAsync,
    dataSummaryApi_d_fetchLatestActionsAsync as fetchLatestActionsAsync,
  };
}

declare function fetchDeploymentConfigurationAsync({ token }: {
    token: any;
}): Promise<any>;

declare const deploymentApi_d_fetchDeploymentConfigurationAsync: typeof fetchDeploymentConfigurationAsync;
declare namespace deploymentApi_d {
  export {
    deploymentApi_d_fetchDeploymentConfigurationAsync as fetchDeploymentConfigurationAsync,
  };
}

declare const fetchEventAsync: ({ id, token }: EntityRequest) => Promise<any>;
declare type EventKinds = components["schemas"]["EventKinds"];
interface EventKindConstants {
    custom: EventKinds;
    propertyUpdate: EventKinds;
    behaviour: EventKinds;
    pageView: EventKinds;
    identify: EventKinds;
    consumeRecommendation: EventKinds;
    addToBusiness: EventKinds;
    purchase: EventKinds;
    usePromotion: EventKinds;
    promotionPresented: EventKinds;
}
declare const eventKinds: EventKindConstants;
declare type EventDto = components["schemas"]["EventDto"];
declare type EventLoggingResponse = components["schemas"]["EventLoggingResponse"];
interface CreateEventRequest {
    apiKey?: string | undefined;
    token?: string | undefined;
    events: EventDto[];
}
declare const createEventsAsync: ({ apiKey, token, events, }: CreateEventRequest) => Promise<EventLoggingResponse>;
declare type CustomerEventPaginated = components["schemas"]["CustomerEventPaginated"];
declare const fetchCustomersEventsAsync: ({ token, id, page, pageSize, useInternalId, }: PaginatedEntityRequest) => Promise<CustomerEventPaginated>;
declare const fetchTrackedUsersEventsAsync: ({ token, id, page, pageSize, useInternalId, }: PaginatedEntityRequest) => Promise<CustomerEventPaginated>;
interface CreateRecommendationConsumedRequest {
    token: string;
    commonUserId?: string | undefined;
    customerId: string;
    correlatorId: number;
    properties: {
        [key: string]: unknown;
    } | null | undefined;
}
declare const createRecommendationConsumedEventAsync: ({ token, commonUserId, customerId, correlatorId, properties, }: CreateRecommendationConsumedRequest) => Promise<{
    eventsProcessed?: number | undefined;
    eventsEnqueued?: number | undefined;
}>;
declare const fetchBusinessEventsAsync: ({ token, id, }: EntityRequest) => Promise<any>;

declare const eventsApi_d_fetchEventAsync: typeof fetchEventAsync;
type eventsApi_d_EventKinds = EventKinds;
declare const eventsApi_d_eventKinds: typeof eventKinds;
declare const eventsApi_d_createEventsAsync: typeof createEventsAsync;
declare const eventsApi_d_fetchCustomersEventsAsync: typeof fetchCustomersEventsAsync;
declare const eventsApi_d_fetchTrackedUsersEventsAsync: typeof fetchTrackedUsersEventsAsync;
declare const eventsApi_d_createRecommendationConsumedEventAsync: typeof createRecommendationConsumedEventAsync;
declare const eventsApi_d_fetchBusinessEventsAsync: typeof fetchBusinessEventsAsync;
declare namespace eventsApi_d {
  export {
    eventsApi_d_fetchEventAsync as fetchEventAsync,
    eventsApi_d_EventKinds as EventKinds,
    eventsApi_d_eventKinds as eventKinds,
    eventsApi_d_createEventsAsync as createEventsAsync,
    eventsApi_d_fetchCustomersEventsAsync as fetchCustomersEventsAsync,
    eventsApi_d_fetchTrackedUsersEventsAsync as fetchTrackedUsersEventsAsync,
    eventsApi_d_createRecommendationConsumedEventAsync as createRecommendationConsumedEventAsync,
    eventsApi_d_fetchBusinessEventsAsync as fetchBusinessEventsAsync,
  };
}

declare function fetchEnvironmentsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function createEnvironmentAsync({ token, environment }: {
    token: any;
    environment: any;
}): Promise<any>;
declare function deleteEnvironmentAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare const setDefaultEnvironmentId$1: (e: number) => void;

declare const environmentsApi_d_fetchEnvironmentsAsync: typeof fetchEnvironmentsAsync;
declare const environmentsApi_d_createEnvironmentAsync: typeof createEnvironmentAsync;
declare const environmentsApi_d_deleteEnvironmentAsync: typeof deleteEnvironmentAsync;
declare namespace environmentsApi_d {
  export {
    environmentsApi_d_fetchEnvironmentsAsync as fetchEnvironmentsAsync,
    environmentsApi_d_createEnvironmentAsync as createEnvironmentAsync,
    environmentsApi_d_deleteEnvironmentAsync as deleteEnvironmentAsync,
    setDefaultEnvironmentId$1 as setDefaultEnvironmentId,
  };
}

declare function fetchFeatureGeneratorsAsync({ page, token }: {
    page: any;
    token: any;
}): Promise<any>;
declare function createFeatureGeneratorAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;
declare function deleteFeatureGeneratorAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function manualTriggerFeatureGeneratorsAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;

declare const featureGeneratorsApi_d_fetchFeatureGeneratorsAsync: typeof fetchFeatureGeneratorsAsync;
declare const featureGeneratorsApi_d_createFeatureGeneratorAsync: typeof createFeatureGeneratorAsync;
declare const featureGeneratorsApi_d_deleteFeatureGeneratorAsync: typeof deleteFeatureGeneratorAsync;
declare const featureGeneratorsApi_d_manualTriggerFeatureGeneratorsAsync: typeof manualTriggerFeatureGeneratorsAsync;
declare namespace featureGeneratorsApi_d {
  export {
    featureGeneratorsApi_d_fetchFeatureGeneratorsAsync as fetchFeatureGeneratorsAsync,
    featureGeneratorsApi_d_createFeatureGeneratorAsync as createFeatureGeneratorAsync,
    featureGeneratorsApi_d_deleteFeatureGeneratorAsync as deleteFeatureGeneratorAsync,
    featureGeneratorsApi_d_manualTriggerFeatureGeneratorsAsync as manualTriggerFeatureGeneratorsAsync,
  };
}

declare function fetchFeaturesAsync({ token, page, searchTerm }: {
    token: any;
    page: any;
    searchTerm: any;
}): Promise<any>;
declare function fetchFeatureAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchFeatureTrackedUsersAsync({ token, page, id }: {
    token: any;
    page: any;
    id: any;
}): Promise<any>;
declare function fetchFeatureTrackedUserFeaturesAsync({ token, page, id, }: {
    token: any;
    page: any;
    id: any;
}): Promise<any>;
declare function createFeatureAsync({ token, feature }: {
    token: any;
    feature: any;
}): Promise<any>;
declare function deleteFeatureAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchTrackedUserFeaturesAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchTrackedUserFeatureValuesAsync({ token, id, feature, version, }: {
    token: any;
    id: any;
    feature: any;
    version: any;
}): Promise<any>;
declare function fetchDestinationsAsync$6({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createDestinationAsync$6({ token, id, destination }: {
    token: any;
    id: any;
    destination: any;
}): Promise<any>;
declare function deleteDestinationAsync$1({ token, id, destinationId }: {
    token: any;
    id: any;
    destinationId: any;
}): Promise<any>;
declare function fetchGeneratorsAsync$1({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;

declare const featuresApi_d_fetchFeaturesAsync: typeof fetchFeaturesAsync;
declare const featuresApi_d_fetchFeatureAsync: typeof fetchFeatureAsync;
declare const featuresApi_d_fetchFeatureTrackedUsersAsync: typeof fetchFeatureTrackedUsersAsync;
declare const featuresApi_d_fetchFeatureTrackedUserFeaturesAsync: typeof fetchFeatureTrackedUserFeaturesAsync;
declare const featuresApi_d_createFeatureAsync: typeof createFeatureAsync;
declare const featuresApi_d_deleteFeatureAsync: typeof deleteFeatureAsync;
declare const featuresApi_d_fetchTrackedUserFeaturesAsync: typeof fetchTrackedUserFeaturesAsync;
declare const featuresApi_d_fetchTrackedUserFeatureValuesAsync: typeof fetchTrackedUserFeatureValuesAsync;
declare namespace featuresApi_d {
  export {
    featuresApi_d_fetchFeaturesAsync as fetchFeaturesAsync,
    featuresApi_d_fetchFeatureAsync as fetchFeatureAsync,
    featuresApi_d_fetchFeatureTrackedUsersAsync as fetchFeatureTrackedUsersAsync,
    featuresApi_d_fetchFeatureTrackedUserFeaturesAsync as fetchFeatureTrackedUserFeaturesAsync,
    featuresApi_d_createFeatureAsync as createFeatureAsync,
    featuresApi_d_deleteFeatureAsync as deleteFeatureAsync,
    featuresApi_d_fetchTrackedUserFeaturesAsync as fetchTrackedUserFeaturesAsync,
    featuresApi_d_fetchTrackedUserFeatureValuesAsync as fetchTrackedUserFeatureValuesAsync,
    fetchDestinationsAsync$6 as fetchDestinationsAsync,
    createDestinationAsync$6 as createDestinationAsync,
    deleteDestinationAsync$1 as deleteDestinationAsync,
    fetchGeneratorsAsync$1 as fetchGeneratorsAsync,
  };
}

interface MetricSearchRequest extends EntitySearchRequest {
    scope?: components["schemas"]["MetricScopes"];
}
declare const fetchMetricsAsync: ({ token, page, scope, searchTerm, }: MetricSearchRequest) => Promise<components["schemas"]["MetricPaginated"]>;
declare const fetchMetricAsync: ({ token, id, }: EntityRequest) => Promise<components["schemas"]["Metric"]>;
declare const fetchMetricCustomersAsync: ({ token, page, id, }: PaginatedEntityRequest) => Promise<any>;
declare const fetchMetricCustomerMetricsAsync: ({ token, page, id, }: PaginatedEntityRequest) => Promise<any>;
interface CreateMetricRequest extends AuthenticatedRequest {
    metric: components["schemas"]["CreateMetric"];
}
declare const createMetricAsync: ({ token, metric, }: CreateMetricRequest) => Promise<any>;
declare const deleteMetricAsync: ({ token, id }: DeleteRequest) => Promise<any>;
declare const fetchCustomersMetricsAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface CustomersMetricRequest extends EntityRequest {
    metricId: string | number;
    version?: number | undefined;
}
declare const fetchCustomersMetricAsync: ({ token, id, metricId, version, }: CustomersMetricRequest) => Promise<any>;
declare const fetchAggregateMetricValuesNumericAsync: ({ token, id, }: EntityRequest) => Promise<any>;
declare const fetchAggregateMetricValuesStringAsync: ({ token, id, }: EntityRequest) => Promise<any>;
declare const fetchDestinationsAsync$5: ({ token, id }: EntityRequest) => Promise<any>;
declare const fetchExportCustomers: ({ token, id }: EntityRequest) => Promise<any>;
declare const fetchMetricBinValuesNumericAsync: ({ token, id, binCount, }: MetricBinRequest) => Promise<any>;
declare const fetchMetricBinValuesStringAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface CreateMetricDestinationRequest extends EntityRequest {
    destination: components["schemas"]["CreateDestinationDto"];
}
declare const createDestinationAsync$5: ({ token, id, destination, }: CreateMetricDestinationRequest) => Promise<any>;
interface DeleteDestinationRequest extends DeleteRequest {
    destinationId: number;
}
declare const deleteDestinationAsync: ({ token, id, destinationId, }: DeleteDestinationRequest) => Promise<any>;
declare const fetchGeneratorsAsync: ({ token, id }: EntityRequest) => Promise<any>;
declare const fetchBusinessMetricsAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface BusinessMetricRequest extends CustomersMetricRequest {
}
declare const fetchBusinessMetricAsync: ({ token, id, metricId, version, }: BusinessMetricRequest) => Promise<any>;

declare const metricsApi_d_fetchMetricsAsync: typeof fetchMetricsAsync;
declare const metricsApi_d_fetchMetricAsync: typeof fetchMetricAsync;
declare const metricsApi_d_fetchMetricCustomersAsync: typeof fetchMetricCustomersAsync;
declare const metricsApi_d_fetchMetricCustomerMetricsAsync: typeof fetchMetricCustomerMetricsAsync;
declare const metricsApi_d_createMetricAsync: typeof createMetricAsync;
declare const metricsApi_d_deleteMetricAsync: typeof deleteMetricAsync;
declare const metricsApi_d_fetchCustomersMetricsAsync: typeof fetchCustomersMetricsAsync;
declare const metricsApi_d_fetchCustomersMetricAsync: typeof fetchCustomersMetricAsync;
declare const metricsApi_d_fetchAggregateMetricValuesNumericAsync: typeof fetchAggregateMetricValuesNumericAsync;
declare const metricsApi_d_fetchAggregateMetricValuesStringAsync: typeof fetchAggregateMetricValuesStringAsync;
declare const metricsApi_d_fetchExportCustomers: typeof fetchExportCustomers;
declare const metricsApi_d_fetchMetricBinValuesNumericAsync: typeof fetchMetricBinValuesNumericAsync;
declare const metricsApi_d_fetchMetricBinValuesStringAsync: typeof fetchMetricBinValuesStringAsync;
declare const metricsApi_d_deleteDestinationAsync: typeof deleteDestinationAsync;
declare const metricsApi_d_fetchGeneratorsAsync: typeof fetchGeneratorsAsync;
declare const metricsApi_d_fetchBusinessMetricsAsync: typeof fetchBusinessMetricsAsync;
declare const metricsApi_d_fetchBusinessMetricAsync: typeof fetchBusinessMetricAsync;
declare namespace metricsApi_d {
  export {
    metricsApi_d_fetchMetricsAsync as fetchMetricsAsync,
    metricsApi_d_fetchMetricAsync as fetchMetricAsync,
    metricsApi_d_fetchMetricCustomersAsync as fetchMetricCustomersAsync,
    metricsApi_d_fetchMetricCustomerMetricsAsync as fetchMetricCustomerMetricsAsync,
    metricsApi_d_createMetricAsync as createMetricAsync,
    metricsApi_d_deleteMetricAsync as deleteMetricAsync,
    metricsApi_d_fetchCustomersMetricsAsync as fetchCustomersMetricsAsync,
    metricsApi_d_fetchCustomersMetricAsync as fetchCustomersMetricAsync,
    metricsApi_d_fetchAggregateMetricValuesNumericAsync as fetchAggregateMetricValuesNumericAsync,
    metricsApi_d_fetchAggregateMetricValuesStringAsync as fetchAggregateMetricValuesStringAsync,
    fetchDestinationsAsync$5 as fetchDestinationsAsync,
    metricsApi_d_fetchExportCustomers as fetchExportCustomers,
    metricsApi_d_fetchMetricBinValuesNumericAsync as fetchMetricBinValuesNumericAsync,
    metricsApi_d_fetchMetricBinValuesStringAsync as fetchMetricBinValuesStringAsync,
    createDestinationAsync$5 as createDestinationAsync,
    metricsApi_d_deleteDestinationAsync as deleteDestinationAsync,
    metricsApi_d_fetchGeneratorsAsync as fetchGeneratorsAsync,
    metricsApi_d_fetchBusinessMetricsAsync as fetchBusinessMetricsAsync,
    metricsApi_d_fetchBusinessMetricAsync as fetchBusinessMetricAsync,
  };
}

declare const fetchMetricGeneratorsAsync: ({ page, token, }: PaginatedRequest) => Promise<any>;
interface CreateMetricGeneratorRequest extends AuthenticatedRequest {
    generator: components["schemas"]["CreateMetricGenerator"];
}
declare const createMetricGeneratorAsync: ({ token, generator, }: CreateMetricGeneratorRequest) => Promise<any>;
declare const deleteMetricGeneratorAsync: ({ token, id, }: DeleteRequest) => Promise<any>;
declare const manualTriggerMetricGeneratorsAsync: ({ token, id, }: EntityRequest) => Promise<any>;

declare const metricGeneratorsApi_d_fetchMetricGeneratorsAsync: typeof fetchMetricGeneratorsAsync;
declare const metricGeneratorsApi_d_createMetricGeneratorAsync: typeof createMetricGeneratorAsync;
declare const metricGeneratorsApi_d_deleteMetricGeneratorAsync: typeof deleteMetricGeneratorAsync;
declare const metricGeneratorsApi_d_manualTriggerMetricGeneratorsAsync: typeof manualTriggerMetricGeneratorsAsync;
declare namespace metricGeneratorsApi_d {
  export {
    metricGeneratorsApi_d_fetchMetricGeneratorsAsync as fetchMetricGeneratorsAsync,
    metricGeneratorsApi_d_createMetricGeneratorAsync as createMetricGeneratorAsync,
    metricGeneratorsApi_d_deleteMetricGeneratorAsync as deleteMetricGeneratorAsync,
    metricGeneratorsApi_d_manualTriggerMetricGeneratorsAsync as manualTriggerMetricGeneratorsAsync,
  };
}

declare function fetchIntegratedSystemsAsync({ token, page, systemType, }: {
    token: any;
    page: any;
    systemType: any;
}): Promise<any>;
declare function fetchIntegratedSystemAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function renameAsync({ token, id, name }: {
    token: any;
    id: any;
    name: any;
}): Promise<any>;
declare function createIntegratedSystemAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;
declare function deleteIntegratedSystemAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchWebhookReceiversAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createWebhookReceiverAsync({ token, id, useSharedSecret, }: {
    token: any;
    id: any;
    useSharedSecret: any;
}): Promise<any>;
declare function setIsDCGeneratorAsync({ token, id, value }: {
    token: any;
    id: any;
    value: any;
}): Promise<any>;

declare const integratedSystemsApi_d_fetchIntegratedSystemsAsync: typeof fetchIntegratedSystemsAsync;
declare const integratedSystemsApi_d_fetchIntegratedSystemAsync: typeof fetchIntegratedSystemAsync;
declare const integratedSystemsApi_d_renameAsync: typeof renameAsync;
declare const integratedSystemsApi_d_createIntegratedSystemAsync: typeof createIntegratedSystemAsync;
declare const integratedSystemsApi_d_deleteIntegratedSystemAsync: typeof deleteIntegratedSystemAsync;
declare const integratedSystemsApi_d_fetchWebhookReceiversAsync: typeof fetchWebhookReceiversAsync;
declare const integratedSystemsApi_d_createWebhookReceiverAsync: typeof createWebhookReceiverAsync;
declare const integratedSystemsApi_d_setIsDCGeneratorAsync: typeof setIsDCGeneratorAsync;
declare namespace integratedSystemsApi_d {
  export {
    integratedSystemsApi_d_fetchIntegratedSystemsAsync as fetchIntegratedSystemsAsync,
    integratedSystemsApi_d_fetchIntegratedSystemAsync as fetchIntegratedSystemAsync,
    integratedSystemsApi_d_renameAsync as renameAsync,
    integratedSystemsApi_d_createIntegratedSystemAsync as createIntegratedSystemAsync,
    integratedSystemsApi_d_deleteIntegratedSystemAsync as deleteIntegratedSystemAsync,
    integratedSystemsApi_d_fetchWebhookReceiversAsync as fetchWebhookReceiversAsync,
    integratedSystemsApi_d_createWebhookReceiverAsync as createWebhookReceiverAsync,
    integratedSystemsApi_d_setIsDCGeneratorAsync as setIsDCGeneratorAsync,
  };
}

declare function fetchModelRegistrationsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function fetchModelRegistrationAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function deleteModelRegistrationAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createModelRegistrationAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;
declare function invokeModelAsync({ token, modelId, metrics }: {
    token: any;
    modelId: any;
    metrics: any;
}): Promise<any>;

declare const modelRegistrationsApi_d_fetchModelRegistrationsAsync: typeof fetchModelRegistrationsAsync;
declare const modelRegistrationsApi_d_fetchModelRegistrationAsync: typeof fetchModelRegistrationAsync;
declare const modelRegistrationsApi_d_deleteModelRegistrationAsync: typeof deleteModelRegistrationAsync;
declare const modelRegistrationsApi_d_createModelRegistrationAsync: typeof createModelRegistrationAsync;
declare const modelRegistrationsApi_d_invokeModelAsync: typeof invokeModelAsync;
declare namespace modelRegistrationsApi_d {
  export {
    modelRegistrationsApi_d_fetchModelRegistrationsAsync as fetchModelRegistrationsAsync,
    modelRegistrationsApi_d_fetchModelRegistrationAsync as fetchModelRegistrationAsync,
    modelRegistrationsApi_d_deleteModelRegistrationAsync as deleteModelRegistrationAsync,
    modelRegistrationsApi_d_createModelRegistrationAsync as createModelRegistrationAsync,
    modelRegistrationsApi_d_invokeModelAsync as invokeModelAsync,
  };
}

declare function invokeGenericModelAsync({ token, id, input }: {
    token: any;
    id: any;
    input: any;
}): Promise<any>;

declare const index_d_invokeGenericModelAsync: typeof invokeGenericModelAsync;
declare namespace index_d {
  export {
    index_d_invokeGenericModelAsync as invokeGenericModelAsync,
  };
}

declare function fetchParametersAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function fetchParameterAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function deleteParameterAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createParameterAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;

declare const parametersApi_d_fetchParametersAsync: typeof fetchParametersAsync;
declare const parametersApi_d_fetchParameterAsync: typeof fetchParameterAsync;
declare const parametersApi_d_deleteParameterAsync: typeof deleteParameterAsync;
declare const parametersApi_d_createParameterAsync: typeof createParameterAsync;
declare namespace parametersApi_d {
  export {
    parametersApi_d_fetchParametersAsync as fetchParametersAsync,
    parametersApi_d_fetchParameterAsync as fetchParameterAsync,
    parametersApi_d_deleteParameterAsync as deleteParameterAsync,
    parametersApi_d_createParameterAsync as createParameterAsync,
  };
}

declare const fetchParameterSetRecommendersAsync: ({ token, page, }: PaginatedRequest) => Promise<any>;
declare const fetchParameterSetRecommenderAsync: ({ token, id, searchTerm, }: EntitySearchRequest) => Promise<any>;
interface CreateParameterSetRecommenderRequest extends AuthenticatedRequest {
    payload: {
        name: string;
        commonId: string;
        settings: components["schemas"]["CampaignSettingsDto"];
        parameters: string[];
        bounds: components["schemas"]["ParameterBounds"][];
        arguments: components["schemas"]["CreateOrUpdateCampaignArgument"][];
    };
}
declare const createParameterSetRecommenderAsync: ({ token, payload, }: CreateParameterSetRecommenderRequest) => Promise<any>;
declare const deleteParameterSetRecommenderAsync: ({ token, id, }: EntityRequest) => Promise<any>;
declare const fetchParameterSetRecommendationsAsync$1: ({ token, page, pageSize, id, }: PaginatedEntityRequest) => Promise<any>;
declare type LinkRegisteredModelRequest$4 = EntityRequest & components["schemas"]["LinkModel"];
declare const createLinkRegisteredModelAsync$4: ({ token, id, modelId, }: LinkRegisteredModelRequest$4) => Promise<any>;
declare const fetchLinkedRegisteredModelAsync$4: ({ token, id, }: EntityRequest) => Promise<any>;
interface InvokeParameterSetRecommenderRequest extends EntityRequest {
    input: components["schemas"]["ModelInputDto"];
}
declare const invokeParameterSetRecommenderAsync: ({ token, id, input, }: InvokeParameterSetRecommenderRequest) => Promise<any>;
declare const fetchInvokationLogsAsync$4: ({ id, token, page, pageSize, }: PaginatedEntityRequest) => Promise<any>;
declare const fetchTargetVariablesAsync$4: ({ id, token, name }: any) => Promise<any>;
declare const createTargetVariableAsync$4: ({ id, token, targetVariableValue, }: any) => Promise<any>;
interface SetSettingsRequest$4 extends EntityRequest {
    settings: components["schemas"]["CampaignSettingsDto"];
}
declare const setSettingsAsync$4: ({ id, token, settings, }: SetSettingsRequest$4) => Promise<any>;
declare type CreateArgument$3 = components["schemas"]["CreateOrUpdateCampaignArgument"];
declare type Argument$4 = components["schemas"]["CampaignArgument"];
interface SetArgumentsRequest$4 extends EntityRequest {
    args: CreateArgument$3[];
}
declare const fetchArgumentsAsync$3: ({ id, token }: EntityRequest) => Promise<any>;
declare const setArgumentsAsync$4: ({ id, token, args, }: SetArgumentsRequest$4) => Promise<Argument$4[]>;
declare const fetchDestinationsAsync$4: ({ id, token }: EntityRequest) => Promise<any>;
interface CreateDestinationRequest$4 extends EntityRequest {
    destination: components["schemas"]["CreateDestinationDto"];
}
declare const createDestinationAsync$4: ({ id, token, destination, }: CreateDestinationRequest$4) => Promise<any>;
interface RemoveDestinationRequest$4 extends EntityRequest {
    destinationId: string | number;
}
declare const removeDestinationAsync$4: ({ id, token, destinationId, }: RemoveDestinationRequest$4) => Promise<any>;
declare const fetchTriggerAsync$4: ({ id, token }: EntityRequest) => Promise<any>;
interface SetTriggerRequest$4 extends EntityRequest {
    trigger: components["schemas"]["SetTriggersDto"];
}
declare const setTriggerAsync$4: ({ id, token, trigger, }: SetTriggerRequest$4) => Promise<any>;
declare const fetchLearningFeaturesAsync$4: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningFeaturesRequest$4 extends EntityRequest {
    featureIds: string[] | number[];
}
declare const setLearningFeaturesAsync$4: ({ id, token, featureIds, useInternalId, }: SetLearningFeaturesRequest$4) => Promise<any>;
declare const fetchLearningMetricsAsync$4: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningMetricsRequest$4 extends EntityRequest {
    metricIds: string[];
}
declare const setLearningMetricsAsync$4: ({ id, token, metricIds, useInternalId, }: SetLearningMetricsRequest$4) => Promise<any>;
declare type RecommenderStatistics$2 = components["schemas"]["CampaignStatistics"];
declare const fetchStatisticsAsync$4: ({ id, token, }: EntityRequest) => Promise<RecommenderStatistics$2>;
declare const fetchReportImageBlobUrlAsync$4: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;

declare const parameterSetRecommendersApi_d_fetchParameterSetRecommendersAsync: typeof fetchParameterSetRecommendersAsync;
declare const parameterSetRecommendersApi_d_fetchParameterSetRecommenderAsync: typeof fetchParameterSetRecommenderAsync;
declare const parameterSetRecommendersApi_d_createParameterSetRecommenderAsync: typeof createParameterSetRecommenderAsync;
declare const parameterSetRecommendersApi_d_deleteParameterSetRecommenderAsync: typeof deleteParameterSetRecommenderAsync;
declare const parameterSetRecommendersApi_d_invokeParameterSetRecommenderAsync: typeof invokeParameterSetRecommenderAsync;
declare namespace parameterSetRecommendersApi_d {
  export {
    parameterSetRecommendersApi_d_fetchParameterSetRecommendersAsync as fetchParameterSetRecommendersAsync,
    parameterSetRecommendersApi_d_fetchParameterSetRecommenderAsync as fetchParameterSetRecommenderAsync,
    parameterSetRecommendersApi_d_createParameterSetRecommenderAsync as createParameterSetRecommenderAsync,
    parameterSetRecommendersApi_d_deleteParameterSetRecommenderAsync as deleteParameterSetRecommenderAsync,
    fetchParameterSetRecommendationsAsync$1 as fetchParameterSetRecommendationsAsync,
    createLinkRegisteredModelAsync$4 as createLinkRegisteredModelAsync,
    fetchLinkedRegisteredModelAsync$4 as fetchLinkedRegisteredModelAsync,
    parameterSetRecommendersApi_d_invokeParameterSetRecommenderAsync as invokeParameterSetRecommenderAsync,
    fetchInvokationLogsAsync$4 as fetchInvokationLogsAsync,
    fetchTargetVariablesAsync$4 as fetchTargetVariablesAsync,
    createTargetVariableAsync$4 as createTargetVariableAsync,
    setSettingsAsync$4 as setSettingsAsync,
    fetchArgumentsAsync$3 as fetchArgumentsAsync,
    setArgumentsAsync$4 as setArgumentsAsync,
    fetchDestinationsAsync$4 as fetchDestinationsAsync,
    createDestinationAsync$4 as createDestinationAsync,
    removeDestinationAsync$4 as removeDestinationAsync,
    fetchTriggerAsync$4 as fetchTriggerAsync,
    setTriggerAsync$4 as setTriggerAsync,
    fetchLearningFeaturesAsync$4 as fetchLearningFeaturesAsync,
    setLearningFeaturesAsync$4 as setLearningFeaturesAsync,
    fetchLearningMetricsAsync$4 as fetchLearningMetricsAsync,
    setLearningMetricsAsync$4 as setLearningMetricsAsync,
    fetchStatisticsAsync$4 as fetchStatisticsAsync,
    fetchReportImageBlobUrlAsync$4 as fetchReportImageBlobUrlAsync,
  };
}

declare const fetchParameterSetCampaignsAsync: ({ token, page, }: PaginatedRequest) => Promise<any>;
declare const fetchParameterSetCampaignAsync: ({ token, id, searchTerm, }: EntitySearchRequest) => Promise<any>;
interface CreateParameterSetCampaignRequest extends AuthenticatedRequest {
    payload: {
        name: string;
        commonId: string;
        settings: components["schemas"]["CampaignSettingsDto"];
        parameters: string[];
        bounds: components["schemas"]["ParameterBounds"][];
        arguments: components["schemas"]["CreateOrUpdateCampaignArgument"][];
    };
}
declare const createParameterSetCampaignAsync: ({ token, payload, }: CreateParameterSetCampaignRequest) => Promise<any>;
declare const deleteParameterSetCampaignAsync: ({ token, id, }: EntityRequest) => Promise<any>;
declare const fetchParameterSetRecommendationsAsync: ({ token, page, pageSize, id, }: PaginatedEntityRequest) => Promise<any>;
declare type LinkRegisteredModelRequest$3 = EntityRequest & components["schemas"]["LinkModel"];
declare const createLinkRegisteredModelAsync$3: ({ token, id, modelId, }: LinkRegisteredModelRequest$3) => Promise<any>;
declare const fetchLinkedRegisteredModelAsync$3: ({ token, id, }: EntityRequest) => Promise<any>;
interface InvokeParameterSetCampaignRequest extends EntityRequest {
    input: components["schemas"]["ModelInputDto"];
}
declare const invokeParameterSetCampaignAsync: ({ token, id, input, }: InvokeParameterSetCampaignRequest) => Promise<any>;
declare const fetchInvokationLogsAsync$3: ({ id, token, page, pageSize, }: PaginatedEntityRequest) => Promise<any>;
declare const fetchTargetVariablesAsync$3: ({ id, token, name }: any) => Promise<any>;
declare const createTargetVariableAsync$3: ({ id, token, targetVariableValue, }: any) => Promise<any>;
interface SetSettingsRequest$3 extends EntityRequest {
    settings: components["schemas"]["CampaignSettingsDto"];
}
declare const setSettingsAsync$3: ({ id, token, settings, }: SetSettingsRequest$3) => Promise<any>;
declare type CreateArgument$2 = components["schemas"]["CreateOrUpdateCampaignArgument"];
declare type Argument$3 = components["schemas"]["CampaignArgument"];
interface SetArgumentsRequest$3 extends EntityRequest {
    args: CreateArgument$2[];
}
declare const fetchArgumentsAsync$2: ({ id, token }: EntityRequest) => Promise<any>;
declare const setArgumentsAsync$3: ({ id, token, args, }: SetArgumentsRequest$3) => Promise<Argument$3[]>;
declare const fetchDestinationsAsync$3: ({ id, token }: EntityRequest) => Promise<any>;
interface CreateDestinationRequest$3 extends EntityRequest {
    destination: components["schemas"]["CreateDestinationDto"];
}
declare const createDestinationAsync$3: ({ id, token, destination, }: CreateDestinationRequest$3) => Promise<any>;
interface RemoveDestinationRequest$3 extends EntityRequest {
    destinationId: string | number;
}
declare const removeDestinationAsync$3: ({ id, token, destinationId, }: RemoveDestinationRequest$3) => Promise<any>;
declare const fetchTriggerAsync$3: ({ id, token }: EntityRequest) => Promise<any>;
interface SetTriggerRequest$3 extends EntityRequest {
    trigger: components["schemas"]["SetTriggersDto"];
}
declare const setTriggerAsync$3: ({ id, token, trigger, }: SetTriggerRequest$3) => Promise<any>;
declare const fetchLearningFeaturesAsync$3: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningFeaturesRequest$3 extends EntityRequest {
    featureIds: string[] | number[];
}
declare const setLearningFeaturesAsync$3: ({ id, token, featureIds, useInternalId, }: SetLearningFeaturesRequest$3) => Promise<any>;
declare const fetchLearningMetricsAsync$3: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningMetricsRequest$3 extends EntityRequest {
    metricIds: string[];
}
declare const setLearningMetricsAsync$3: ({ id, token, metricIds, useInternalId, }: SetLearningMetricsRequest$3) => Promise<any>;
declare type CampaignStatistics$1 = components["schemas"]["CampaignStatistics"];
declare const fetchStatisticsAsync$3: ({ id, token, }: EntityRequest) => Promise<CampaignStatistics$1>;
declare const fetchReportImageBlobUrlAsync$3: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;

declare const parameterSetCampaignsApi_d_fetchParameterSetCampaignsAsync: typeof fetchParameterSetCampaignsAsync;
declare const parameterSetCampaignsApi_d_fetchParameterSetCampaignAsync: typeof fetchParameterSetCampaignAsync;
declare const parameterSetCampaignsApi_d_createParameterSetCampaignAsync: typeof createParameterSetCampaignAsync;
declare const parameterSetCampaignsApi_d_deleteParameterSetCampaignAsync: typeof deleteParameterSetCampaignAsync;
declare const parameterSetCampaignsApi_d_fetchParameterSetRecommendationsAsync: typeof fetchParameterSetRecommendationsAsync;
declare const parameterSetCampaignsApi_d_invokeParameterSetCampaignAsync: typeof invokeParameterSetCampaignAsync;
declare namespace parameterSetCampaignsApi_d {
  export {
    parameterSetCampaignsApi_d_fetchParameterSetCampaignsAsync as fetchParameterSetCampaignsAsync,
    parameterSetCampaignsApi_d_fetchParameterSetCampaignAsync as fetchParameterSetCampaignAsync,
    parameterSetCampaignsApi_d_createParameterSetCampaignAsync as createParameterSetCampaignAsync,
    parameterSetCampaignsApi_d_deleteParameterSetCampaignAsync as deleteParameterSetCampaignAsync,
    parameterSetCampaignsApi_d_fetchParameterSetRecommendationsAsync as fetchParameterSetRecommendationsAsync,
    createLinkRegisteredModelAsync$3 as createLinkRegisteredModelAsync,
    fetchLinkedRegisteredModelAsync$3 as fetchLinkedRegisteredModelAsync,
    parameterSetCampaignsApi_d_invokeParameterSetCampaignAsync as invokeParameterSetCampaignAsync,
    fetchInvokationLogsAsync$3 as fetchInvokationLogsAsync,
    fetchTargetVariablesAsync$3 as fetchTargetVariablesAsync,
    createTargetVariableAsync$3 as createTargetVariableAsync,
    setSettingsAsync$3 as setSettingsAsync,
    fetchArgumentsAsync$2 as fetchArgumentsAsync,
    setArgumentsAsync$3 as setArgumentsAsync,
    fetchDestinationsAsync$3 as fetchDestinationsAsync,
    createDestinationAsync$3 as createDestinationAsync,
    removeDestinationAsync$3 as removeDestinationAsync,
    fetchTriggerAsync$3 as fetchTriggerAsync,
    setTriggerAsync$3 as setTriggerAsync,
    fetchLearningFeaturesAsync$3 as fetchLearningFeaturesAsync,
    setLearningFeaturesAsync$3 as setLearningFeaturesAsync,
    fetchLearningMetricsAsync$3 as fetchLearningMetricsAsync,
    setLearningMetricsAsync$3 as setLearningMetricsAsync,
    fetchStatisticsAsync$3 as fetchStatisticsAsync,
    fetchReportImageBlobUrlAsync$3 as fetchReportImageBlobUrlAsync,
  };
}

declare function setMetadataAsync({ token, metadata }: {
    token: any;
    metadata: any;
}): Promise<any>;
declare function getMetadataAsync({ token }: {
    token: any;
}): Promise<any>;

declare const profileApi_d_setMetadataAsync: typeof setMetadataAsync;
declare const profileApi_d_getMetadataAsync: typeof getMetadataAsync;
declare namespace profileApi_d {
  export {
    profileApi_d_setMetadataAsync as setMetadataAsync,
    profileApi_d_getMetadataAsync as getMetadataAsync,
  };
}

declare const fetchItemsRecommendersAsync: ({ token, page, }: PaginatedRequest) => Promise<any>;
declare const fetchItemsRecommenderAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface ItemsRecommendationsRequest extends PaginatedEntityRequest {
    page: number;
}
declare const fetchItemsRecommendationsAsync: ({ token, page, pageSize, id, }: ItemsRecommendationsRequest) => Promise<any>;
declare const deleteItemsRecommenderAsync: ({ token, id, }: DeleteRequest) => Promise<any>;
interface CreateItemsRecommenderRequest extends AuthenticatedRequest {
    payload: components["schemas"]["CreatePromotionsCampaign"];
}
declare const createItemsRecommenderAsync: ({ token, payload, useInternalId, }: CreateItemsRecommenderRequest) => Promise<any>;
declare const fetchItemsAsync$1: ({ token, id }: EntityRequest) => Promise<any>;
interface AddItemPayload {
    id: number | undefined;
    commonId: string | undefined;
}
interface AddItemRequest extends EntityRequest {
    item: AddItemPayload;
}
declare const addItemAsync: ({ token, id, item }: AddItemRequest) => Promise<any>;
interface RemoveItemRequest extends EntityRequest {
    itemId: string | number;
}
declare const removeItemAsync: ({ token, id, itemId, }: RemoveItemRequest) => Promise<any>;
interface SetBaselineItemRequest extends EntityRequest {
    itemId: string | number;
}
declare const setBaselineItemAsync: ({ token, id, itemId, }: SetBaselineItemRequest) => Promise<any>;
declare const setDefaultItemAsync: ({ token, id, itemId, }: SetBaselineItemRequest) => Promise<any>;
declare const getBaselineItemAsync: ({ token, id }: EntityRequest) => Promise<any>;
declare const getDefaultItemAsync: ({ token, id }: EntityRequest) => Promise<any>;
declare type LinkRegisteredModelRequest$2 = EntityRequest & components["schemas"]["LinkModel"];
declare const createLinkRegisteredModelAsync$2: ({ token, id, modelId, }: LinkRegisteredModelRequest$2) => Promise<any>;
declare const fetchLinkedRegisteredModelAsync$2: ({ token, id, }: EntityRequest) => Promise<any>;
interface InvokeItemRecommenderRequest extends EntityRequest {
    input: ModelInput;
}
declare const invokeItemsRecommenderAsync: ({ token, id, input, }: InvokeItemRecommenderRequest) => Promise<ItemsRecommendation>;
declare const fetchInvokationLogsAsync$2: ({ id, token, page, pageSize, }: PaginatedEntityRequest) => Promise<any>;
declare const fetchTargetVariablesAsync$2: ({ id, token, name }: any) => Promise<any>;
declare const createTargetVariableAsync$2: ({ id, token, targetVariableValue, }: any) => Promise<any>;
interface RecommenderSettings$1 {
    requireConsumptionEvent: boolean;
    throwOnBadInput: boolean;
    recommendationCacheTime: string;
}
interface SetSettingsRequest$2 extends EntityRequest {
    settings: RecommenderSettings$1;
}
declare const setSettingsAsync$2: ({ id, token, settings, }: SetSettingsRequest$2) => Promise<any>;
interface Argument$2 {
    commonId: string;
    argumentType: "Numerical" | "Categorical";
    defaultValue: string | number;
    isRequired: boolean;
}
interface SetArgumentsRequest$2 extends EntityRequest {
    args: Argument$2[];
}
declare const setArgumentsAsync$2: ({ id, token, args, }: SetArgumentsRequest$2) => Promise<any>;
declare const fetchDestinationsAsync$2: ({ id, token }: EntityRequest) => Promise<any>;
interface Destination$2 {
    destinationType: "Webhook" | "SegmentSourceFunction" | "HubspotContactProperty";
    endpoint: string;
    integratedSystemId: number;
}
interface CreateDestinationRequest$2 extends EntityRequest {
    destination: Destination$2;
}
declare const createDestinationAsync$2: ({ id, token, destination, }: CreateDestinationRequest$2) => Promise<any>;
interface RemoveDestinationRequest$2 extends EntityRequest {
    destinationId: number;
}
declare const removeDestinationAsync$2: ({ id, token, destinationId, }: RemoveDestinationRequest$2) => Promise<any>;
declare const fetchTriggerAsync$2: ({ id, token }: EntityRequest) => Promise<any>;
interface Trigger$2 {
    featuresChanged: any;
}
interface SetTriggerRequest$2 extends EntityRequest {
    trigger: Trigger$2;
}
declare const setTriggerAsync$2: ({ id, token, trigger, }: SetTriggerRequest$2) => Promise<any>;
declare const fetchLearningFeaturesAsync$2: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningFeaturesRequest$2 extends EntityRequest {
    featureIds: string[];
}
declare const setLearningFeaturesAsync$2: ({ id, token, featureIds, useInternalId, }: SetLearningFeaturesRequest$2) => Promise<any>;
declare const fetchLearningMetricsAsync$2: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningMetricsRequest$2 extends EntityRequest {
    metricIds: string[];
}
declare const setLearningMetricsAsync$2: ({ id, token, metricIds, useInternalId, }: SetLearningMetricsRequest$2) => Promise<any>;
declare type RecommenderStatistics$1 = components["schemas"]["CampaignStatistics"];
declare const fetchStatisticsAsync$2: ({ id, token, }: EntityRequest) => Promise<RecommenderStatistics$1>;
declare const fetchReportImageBlobUrlAsync$2: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
declare type PerformanceResponse$2 = components["schemas"]["ItemsRecommenderPerformanceReport"];
interface PerformanceRequest$2 extends EntityRequest {
    reportId?: string | number | undefined;
}
declare const fetchPerformanceAsync$2: ({ token, id, reportId, }: PerformanceRequest$2) => Promise<PerformanceResponse$2>;

declare const itemsRecommendersApi_d_fetchItemsRecommendersAsync: typeof fetchItemsRecommendersAsync;
declare const itemsRecommendersApi_d_fetchItemsRecommenderAsync: typeof fetchItemsRecommenderAsync;
declare const itemsRecommendersApi_d_fetchItemsRecommendationsAsync: typeof fetchItemsRecommendationsAsync;
declare const itemsRecommendersApi_d_deleteItemsRecommenderAsync: typeof deleteItemsRecommenderAsync;
declare const itemsRecommendersApi_d_createItemsRecommenderAsync: typeof createItemsRecommenderAsync;
declare const itemsRecommendersApi_d_addItemAsync: typeof addItemAsync;
declare const itemsRecommendersApi_d_removeItemAsync: typeof removeItemAsync;
declare const itemsRecommendersApi_d_setBaselineItemAsync: typeof setBaselineItemAsync;
declare const itemsRecommendersApi_d_setDefaultItemAsync: typeof setDefaultItemAsync;
declare const itemsRecommendersApi_d_getBaselineItemAsync: typeof getBaselineItemAsync;
declare const itemsRecommendersApi_d_getDefaultItemAsync: typeof getDefaultItemAsync;
declare const itemsRecommendersApi_d_invokeItemsRecommenderAsync: typeof invokeItemsRecommenderAsync;
declare namespace itemsRecommendersApi_d {
  export {
    itemsRecommendersApi_d_fetchItemsRecommendersAsync as fetchItemsRecommendersAsync,
    itemsRecommendersApi_d_fetchItemsRecommenderAsync as fetchItemsRecommenderAsync,
    itemsRecommendersApi_d_fetchItemsRecommendationsAsync as fetchItemsRecommendationsAsync,
    itemsRecommendersApi_d_deleteItemsRecommenderAsync as deleteItemsRecommenderAsync,
    itemsRecommendersApi_d_createItemsRecommenderAsync as createItemsRecommenderAsync,
    fetchItemsAsync$1 as fetchItemsAsync,
    itemsRecommendersApi_d_addItemAsync as addItemAsync,
    itemsRecommendersApi_d_removeItemAsync as removeItemAsync,
    itemsRecommendersApi_d_setBaselineItemAsync as setBaselineItemAsync,
    itemsRecommendersApi_d_setDefaultItemAsync as setDefaultItemAsync,
    itemsRecommendersApi_d_getBaselineItemAsync as getBaselineItemAsync,
    itemsRecommendersApi_d_getDefaultItemAsync as getDefaultItemAsync,
    createLinkRegisteredModelAsync$2 as createLinkRegisteredModelAsync,
    fetchLinkedRegisteredModelAsync$2 as fetchLinkedRegisteredModelAsync,
    itemsRecommendersApi_d_invokeItemsRecommenderAsync as invokeItemsRecommenderAsync,
    fetchInvokationLogsAsync$2 as fetchInvokationLogsAsync,
    fetchTargetVariablesAsync$2 as fetchTargetVariablesAsync,
    createTargetVariableAsync$2 as createTargetVariableAsync,
    setSettingsAsync$2 as setSettingsAsync,
    setArgumentsAsync$2 as setArgumentsAsync,
    fetchDestinationsAsync$2 as fetchDestinationsAsync,
    createDestinationAsync$2 as createDestinationAsync,
    removeDestinationAsync$2 as removeDestinationAsync,
    fetchTriggerAsync$2 as fetchTriggerAsync,
    setTriggerAsync$2 as setTriggerAsync,
    fetchLearningFeaturesAsync$2 as fetchLearningFeaturesAsync,
    setLearningFeaturesAsync$2 as setLearningFeaturesAsync,
    fetchLearningMetricsAsync$2 as fetchLearningMetricsAsync,
    setLearningMetricsAsync$2 as setLearningMetricsAsync,
    fetchStatisticsAsync$2 as fetchStatisticsAsync,
    fetchReportImageBlobUrlAsync$2 as fetchReportImageBlobUrlAsync,
    fetchPerformanceAsync$2 as fetchPerformanceAsync,
  };
}

declare const fetchPromotionsRecommendersAsync: ({ token, page, }: PaginatedRequest) => Promise<any>;
declare const fetchPromotionsRecommenderAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface PromotionsRecommendationsRequest$1 extends PaginatedEntityRequest {
    page: number;
}
declare const fetchPromotionsRecommendationsAsync$1: ({ token, page, pageSize, id, }: PromotionsRecommendationsRequest$1) => Promise<any>;
declare const deletePromotionsRecommenderAsync: ({ token, id, }: DeleteRequest) => Promise<any>;
interface CreatePromotionsRecommenderRequest extends AuthenticatedRequest {
    payload: components["schemas"]["CreatePromotionsCampaign"];
}
declare const createPromotionsRecommenderAsync: ({ token, payload, useInternalId, }: CreatePromotionsRecommenderRequest) => Promise<any>;
declare const fetchPromotionsAsync$2: ({ token, id }: EntityRequest) => Promise<any>;
declare type Audience$1 = components["schemas"]["Audience"];
declare const fetchAudienceAsync$1: ({ token, id, }: EntityRequest) => Promise<Audience$1>;
interface AddPromotionPayload$1 {
    id: number | undefined;
    commonId: string | undefined;
}
interface AddPromotionRequest$1 extends EntityRequest {
    promotion: AddPromotionPayload$1;
}
declare const addPromotionAsync$1: ({ token, id, promotion, }: AddPromotionRequest$1) => Promise<any>;
interface RemovePromotionRequest$1 extends EntityRequest {
    promotionId: string | number;
}
declare const removePromotionAsync$1: ({ token, id, promotionId, }: RemovePromotionRequest$1) => Promise<any>;
interface SetBaselinePromotionRequest$1 extends EntityRequest {
    promotionId: string | number;
}
declare const setBaselinePromotionAsync$1: ({ token, id, promotionId, }: SetBaselinePromotionRequest$1) => Promise<any>;
declare const getBaselinePromotionAsync$1: ({ token, id, }: EntityRequest) => Promise<any>;
declare type LinkRegisteredModelRequest$1 = EntityRequest & components["schemas"]["LinkModel"];
declare const createLinkRegisteredModelAsync$1: ({ token, id, modelId, }: LinkRegisteredModelRequest$1) => Promise<any>;
declare const fetchLinkedRegisteredModelAsync$1: ({ token, id, }: EntityRequest) => Promise<any>;
interface InvokePromotionRecommenderRequest extends EntityRequest {
    input: ModelInput;
}
declare const invokePromotionsRecommenderAsync: ({ token, id, input, }: InvokePromotionRecommenderRequest) => Promise<PromotionsRecommendation>;
declare const fetchInvokationLogsAsync$1: ({ id, token, page, pageSize, }: PaginatedEntityRequest) => Promise<any>;
declare const fetchTargetVariablesAsync$1: ({ id, token, name }: any) => Promise<any>;
declare const createTargetVariableAsync$1: ({ id, token, targetVariableValue, }: any) => Promise<any>;
interface RecommenderSettings {
    requireConsumptionEvent: boolean;
    throwOnBadInput: boolean;
    recommendationCacheTime: string;
}
interface SetSettingsRequest$1 extends EntityRequest {
    settings: RecommenderSettings;
}
declare const setSettingsAsync$1: ({ id, token, settings, }: SetSettingsRequest$1) => Promise<any>;
declare type CreateArgument$1 = components["schemas"]["CreateOrUpdateCampaignArgument"];
declare type Argument$1 = components["schemas"]["CampaignArgument"];
interface SetArgumentsRequest$1 extends EntityRequest {
    args: CreateArgument$1[];
}
declare const fetchArgumentsAsync$1: ({ id, token }: EntityRequest) => Promise<any>;
declare const setArgumentsAsync$1: ({ id, token, args, }: SetArgumentsRequest$1) => Promise<Argument$1[]>;
interface CreateChoosePromotionArgumentRule$1 extends EntityRequest {
    rule: components["schemas"]["CreateChoosePromotionArgumentRuleDto"];
}
declare const createChoosePromotionArgumentRuleAsync$1: ({ id, useInternalId, token, rule, }: CreateChoosePromotionArgumentRule$1) => Promise<any>;
interface UpdateChoosePromotionArgumentRule$1 extends EntityRequest {
    rule: components["schemas"]["UpdateChoosePromotionArgumentRuleDto"];
    ruleId: number;
}
declare const updateChoosePromotionArgumentRuleAsync$1: ({ id, useInternalId, token, rule, ruleId, }: UpdateChoosePromotionArgumentRule$1) => Promise<any>;
declare const fetchChoosePromotionArgumentRulesAsync$1: ({ id, useInternalId, token, }: CreateChoosePromotionArgumentRule$1) => Promise<any>;
interface CreateChooseSegmentArgumentRule$1 extends EntityRequest {
    rule: components["schemas"]["CreateChooseSegmentArgumentRuleDto"];
}
declare const createChooseSegmentArgumentRuleAsync$1: ({ id, useInternalId, token, rule, }: CreateChooseSegmentArgumentRule$1) => Promise<any>;
interface UpdateChooseSegmentArgumentRule$1 extends EntityRequest {
    rule: components["schemas"]["UpdateChooseSegmentArgumentRuleDto"];
    ruleId: number;
}
declare const updateChooseSegmentArgumentRuleAsync$1: ({ id, useInternalId, token, rule, ruleId, }: UpdateChooseSegmentArgumentRule$1) => Promise<any>;
declare const fetchChooseSegmentArgumentRulesAsync$1: ({ id, useInternalId, token, }: CreateChoosePromotionArgumentRule$1) => Promise<any>;
interface DeleteArgumentRule$1 extends EntityRequest {
    ruleId: number;
}
declare const deleteArgumentRuleAsync$1: ({ id, useInternalId, token, ruleId, }: DeleteArgumentRule$1) => Promise<any>;
declare const fetchDestinationsAsync$1: ({ id, token }: EntityRequest) => Promise<any>;
interface Destination$1 {
    destinationType: "Webhook" | "SegmentSourceFunction" | "HubspotContactProperty";
    endpoint: string;
    integratedSystemId: number;
}
interface CreateDestinationRequest$1 extends EntityRequest {
    destination: Destination$1;
}
declare const createDestinationAsync$1: ({ id, token, destination, }: CreateDestinationRequest$1) => Promise<any>;
interface RemoveDestinationRequest$1 extends EntityRequest {
    destinationId: number;
}
declare const removeDestinationAsync$1: ({ id, token, destinationId, }: RemoveDestinationRequest$1) => Promise<any>;
declare const fetchTriggerAsync$1: ({ id, token }: EntityRequest) => Promise<any>;
interface Trigger$1 {
    featuresChanged: any;
}
interface SetTriggerRequest$1 extends EntityRequest {
    trigger: Trigger$1;
}
declare const setTriggerAsync$1: ({ id, token, trigger, }: SetTriggerRequest$1) => Promise<any>;
declare const fetchLearningFeaturesAsync$1: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningFeaturesRequest$1 extends EntityRequest {
    featureIds: string[];
}
declare const setLearningFeaturesAsync$1: ({ id, token, featureIds, useInternalId, }: SetLearningFeaturesRequest$1) => Promise<any>;
declare const fetchLearningMetricsAsync$1: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningMetricsRequest$1 extends EntityRequest {
    metricIds: string[];
}
declare const setLearningMetricsAsync$1: ({ id, token, metricIds, useInternalId, }: SetLearningMetricsRequest$1) => Promise<any>;
declare type RecommenderStatistics = components["schemas"]["CampaignStatistics"];
declare const fetchStatisticsAsync$1: ({ id, token, }: EntityRequest) => Promise<RecommenderStatistics>;
declare const fetchReportImageBlobUrlAsync$1: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
declare type PerformanceResponse$1 = components["schemas"]["ItemsRecommenderPerformanceReport"];
interface PerformanceRequest$1 extends EntityRequest {
    reportId?: string | number | undefined;
}
declare const fetchPerformanceAsync$1: ({ token, id, reportId, }: PerformanceRequest$1) => Promise<PerformanceResponse$1>;
declare type PromotionOptimiser$1 = components["schemas"]["PromotionOptimiser"];
declare const fetchPromotionOptimiserAsync$1: ({ token, useInternalId, id, }: EntityRequest) => Promise<PromotionOptimiser$1>;
interface SetAllPromotionOptimiserWeightsRequest$1 extends EntityRequest {
    weights: components["schemas"]["UpdateWeightDto"][];
}
declare const setAllPromotionOptimiserWeightsAsync$1: ({ token, useInternalId, id, weights, }: SetAllPromotionOptimiserWeightsRequest$1) => Promise<PromotionOptimiser$1>;
interface SetPromotionOptimiserWeightRequest$1 extends EntityRequest {
    weightId: number;
    weight: number;
}
declare const setPromotionOptimiserWeightAsync$1: ({ token, useInternalId, id, weightId, weight, }: SetPromotionOptimiserWeightRequest$1) => Promise<PromotionOptimiser$1>;
interface SetUseOptimiserRequest$1 extends EntityRequest {
    useOptimiser: boolean;
}
declare const setUseOptimiserAsync$1: ({ token, useInternalId, id, useOptimiser, }: SetUseOptimiserRequest$1) => Promise<PromotionOptimiser$1>;
declare const fetchRecommenderChannelsAsync: ({ id, token, }: EntityRequest) => Promise<any>;
declare type Channel$1 = components["schemas"]["ChannelBase"];
interface AddRecommenderChannelRequest extends EntityRequest {
    channel: components["schemas"]["AddCampaignChannelDto"];
}
declare const addRecommenderChannelAsync: ({ token, id, channel, }: AddRecommenderChannelRequest) => Promise<Channel$1>;
declare type PromotionsRecommenders = components["schemas"]["PromotionsCampaign"];
interface RemoveRecommenderChannelRequest extends EntityRequest {
    channelId: number;
}
declare const removeRecommenderChannelAsync: ({ id, token, channelId, }: RemoveRecommenderChannelRequest) => Promise<PromotionsRecommenders>;
interface RecommendationRequest$1 extends EntityRequest {
    recommendationId: number;
}
declare const fetchPromotionsRecommendationAsync$1: ({ token, recommendationId, }: RecommendationRequest$1) => Promise<any>;
interface OffersRequest$1 extends PaginatedEntityRequest {
    page: number;
    offerState?: string;
}
declare const fetchOffersAsync$1: ({ token, page, pageSize, id, offerState, }: OffersRequest$1) => Promise<any>;

declare const promotionsRecommendersApi_d_fetchPromotionsRecommendersAsync: typeof fetchPromotionsRecommendersAsync;
declare const promotionsRecommendersApi_d_fetchPromotionsRecommenderAsync: typeof fetchPromotionsRecommenderAsync;
declare const promotionsRecommendersApi_d_deletePromotionsRecommenderAsync: typeof deletePromotionsRecommenderAsync;
declare const promotionsRecommendersApi_d_createPromotionsRecommenderAsync: typeof createPromotionsRecommenderAsync;
declare const promotionsRecommendersApi_d_invokePromotionsRecommenderAsync: typeof invokePromotionsRecommenderAsync;
declare const promotionsRecommendersApi_d_fetchRecommenderChannelsAsync: typeof fetchRecommenderChannelsAsync;
declare const promotionsRecommendersApi_d_addRecommenderChannelAsync: typeof addRecommenderChannelAsync;
declare const promotionsRecommendersApi_d_removeRecommenderChannelAsync: typeof removeRecommenderChannelAsync;
declare namespace promotionsRecommendersApi_d {
  export {
    promotionsRecommendersApi_d_fetchPromotionsRecommendersAsync as fetchPromotionsRecommendersAsync,
    promotionsRecommendersApi_d_fetchPromotionsRecommenderAsync as fetchPromotionsRecommenderAsync,
    fetchPromotionsRecommendationsAsync$1 as fetchPromotionsRecommendationsAsync,
    promotionsRecommendersApi_d_deletePromotionsRecommenderAsync as deletePromotionsRecommenderAsync,
    promotionsRecommendersApi_d_createPromotionsRecommenderAsync as createPromotionsRecommenderAsync,
    fetchPromotionsAsync$2 as fetchPromotionsAsync,
    fetchAudienceAsync$1 as fetchAudienceAsync,
    addPromotionAsync$1 as addPromotionAsync,
    removePromotionAsync$1 as removePromotionAsync,
    setBaselinePromotionAsync$1 as setBaselinePromotionAsync,
    getBaselinePromotionAsync$1 as getBaselinePromotionAsync,
    createLinkRegisteredModelAsync$1 as createLinkRegisteredModelAsync,
    fetchLinkedRegisteredModelAsync$1 as fetchLinkedRegisteredModelAsync,
    promotionsRecommendersApi_d_invokePromotionsRecommenderAsync as invokePromotionsRecommenderAsync,
    fetchInvokationLogsAsync$1 as fetchInvokationLogsAsync,
    fetchTargetVariablesAsync$1 as fetchTargetVariablesAsync,
    createTargetVariableAsync$1 as createTargetVariableAsync,
    setSettingsAsync$1 as setSettingsAsync,
    fetchArgumentsAsync$1 as fetchArgumentsAsync,
    setArgumentsAsync$1 as setArgumentsAsync,
    createChoosePromotionArgumentRuleAsync$1 as createChoosePromotionArgumentRuleAsync,
    updateChoosePromotionArgumentRuleAsync$1 as updateChoosePromotionArgumentRuleAsync,
    fetchChoosePromotionArgumentRulesAsync$1 as fetchChoosePromotionArgumentRulesAsync,
    createChooseSegmentArgumentRuleAsync$1 as createChooseSegmentArgumentRuleAsync,
    updateChooseSegmentArgumentRuleAsync$1 as updateChooseSegmentArgumentRuleAsync,
    fetchChooseSegmentArgumentRulesAsync$1 as fetchChooseSegmentArgumentRulesAsync,
    deleteArgumentRuleAsync$1 as deleteArgumentRuleAsync,
    fetchDestinationsAsync$1 as fetchDestinationsAsync,
    createDestinationAsync$1 as createDestinationAsync,
    removeDestinationAsync$1 as removeDestinationAsync,
    fetchTriggerAsync$1 as fetchTriggerAsync,
    setTriggerAsync$1 as setTriggerAsync,
    fetchLearningFeaturesAsync$1 as fetchLearningFeaturesAsync,
    setLearningFeaturesAsync$1 as setLearningFeaturesAsync,
    fetchLearningMetricsAsync$1 as fetchLearningMetricsAsync,
    setLearningMetricsAsync$1 as setLearningMetricsAsync,
    fetchStatisticsAsync$1 as fetchStatisticsAsync,
    fetchReportImageBlobUrlAsync$1 as fetchReportImageBlobUrlAsync,
    fetchPerformanceAsync$1 as fetchPerformanceAsync,
    fetchPromotionOptimiserAsync$1 as fetchPromotionOptimiserAsync,
    setAllPromotionOptimiserWeightsAsync$1 as setAllPromotionOptimiserWeightsAsync,
    setPromotionOptimiserWeightAsync$1 as setPromotionOptimiserWeightAsync,
    setUseOptimiserAsync$1 as setUseOptimiserAsync,
    promotionsRecommendersApi_d_fetchRecommenderChannelsAsync as fetchRecommenderChannelsAsync,
    promotionsRecommendersApi_d_addRecommenderChannelAsync as addRecommenderChannelAsync,
    promotionsRecommendersApi_d_removeRecommenderChannelAsync as removeRecommenderChannelAsync,
    fetchPromotionsRecommendationAsync$1 as fetchPromotionsRecommendationAsync,
    fetchOffersAsync$1 as fetchOffersAsync,
  };
}

declare const fetchPromotionsCampaignsAsync: ({ token, page, }: PaginatedRequest) => Promise<any>;
declare const fetchPromotionsCampaignAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface PromotionsRecommendationsRequest extends PaginatedEntityRequest {
    page: number;
}
declare const fetchPromotionsRecommendationsAsync: ({ token, page, pageSize, id, }: PromotionsRecommendationsRequest) => Promise<any>;
declare const deletePromotionsCampaignAsync: ({ token, id, }: DeleteRequest) => Promise<any>;
interface CreatePromotionsCampaignRequest extends AuthenticatedRequest {
    payload: components["schemas"]["CreatePromotionsCampaign"];
}
declare const createPromotionsCampaignAsync: ({ token, payload, useInternalId, }: CreatePromotionsCampaignRequest) => Promise<any>;
declare const fetchPromotionsAsync$1: ({ token, id }: EntityRequest) => Promise<any>;
declare type Audience = components["schemas"]["Audience"];
declare const fetchAudienceAsync: ({ token, id, }: EntityRequest) => Promise<Audience>;
interface AddPromotionPayload {
    id: number | undefined;
    commonId: string | undefined;
}
interface AddPromotionRequest extends EntityRequest {
    promotion: AddPromotionPayload;
}
declare const addPromotionAsync: ({ token, id, promotion, }: AddPromotionRequest) => Promise<any>;
interface RemovePromotionRequest extends EntityRequest {
    promotionId: string | number;
}
declare const removePromotionAsync: ({ token, id, promotionId, }: RemovePromotionRequest) => Promise<any>;
interface SetBaselinePromotionRequest extends EntityRequest {
    promotionId: string | number;
}
declare const setBaselinePromotionAsync: ({ token, id, promotionId, }: SetBaselinePromotionRequest) => Promise<any>;
declare const getBaselinePromotionAsync: ({ token, id, }: EntityRequest) => Promise<any>;
declare type LinkRegisteredModelRequest = EntityRequest & components["schemas"]["LinkModel"];
declare const createLinkRegisteredModelAsync: ({ token, id, modelId, }: LinkRegisteredModelRequest) => Promise<any>;
declare const fetchLinkedRegisteredModelAsync: ({ token, id, }: EntityRequest) => Promise<any>;
interface InvokePromotionCampaignRequest extends EntityRequest {
    input: ModelInput;
}
declare const invokePromotionsCampaignAsync: ({ token, id, input, }: InvokePromotionCampaignRequest) => Promise<PromotionsRecommendation>;
declare const fetchInvokationLogsAsync: ({ id, token, page, pageSize, }: PaginatedEntityRequest) => Promise<any>;
declare const fetchTargetVariablesAsync: ({ id, token, name }: any) => Promise<any>;
declare const createTargetVariableAsync: ({ id, token, targetVariableValue, }: any) => Promise<any>;
interface CampaignSettings {
    requireConsumptionEvent: boolean;
    throwOnBadInput: boolean;
    recommendationCacheTime: string;
}
interface SetSettingsRequest extends EntityRequest {
    settings: CampaignSettings;
}
declare const setSettingsAsync: ({ id, token, settings, }: SetSettingsRequest) => Promise<any>;
declare type CreateArgument = components["schemas"]["CreateOrUpdateCampaignArgument"];
declare type Argument = components["schemas"]["CampaignArgument"];
interface SetArgumentsRequest extends EntityRequest {
    args: CreateArgument[];
}
declare const fetchArgumentsAsync: ({ id, token }: EntityRequest) => Promise<any>;
declare const setArgumentsAsync: ({ id, token, args, }: SetArgumentsRequest) => Promise<Argument[]>;
interface CreateChoosePromotionArgumentRule extends EntityRequest {
    rule: components["schemas"]["CreateChoosePromotionArgumentRuleDto"];
}
declare const createChoosePromotionArgumentRuleAsync: ({ id, useInternalId, token, rule, }: CreateChoosePromotionArgumentRule) => Promise<any>;
interface UpdateChoosePromotionArgumentRule extends EntityRequest {
    rule: components["schemas"]["UpdateChoosePromotionArgumentRuleDto"];
    ruleId: number;
}
declare const updateChoosePromotionArgumentRuleAsync: ({ id, useInternalId, token, rule, ruleId, }: UpdateChoosePromotionArgumentRule) => Promise<any>;
declare const fetchChoosePromotionArgumentRulesAsync: ({ id, useInternalId, token, }: CreateChoosePromotionArgumentRule) => Promise<any>;
interface CreateChooseSegmentArgumentRule extends EntityRequest {
    rule: components["schemas"]["CreateChooseSegmentArgumentRuleDto"];
}
declare const createChooseSegmentArgumentRuleAsync: ({ id, useInternalId, token, rule, }: CreateChooseSegmentArgumentRule) => Promise<any>;
interface UpdateChooseSegmentArgumentRule extends EntityRequest {
    rule: components["schemas"]["UpdateChooseSegmentArgumentRuleDto"];
    ruleId: number;
}
declare const updateChooseSegmentArgumentRuleAsync: ({ id, useInternalId, token, rule, ruleId, }: UpdateChooseSegmentArgumentRule) => Promise<any>;
declare const fetchChooseSegmentArgumentRulesAsync: ({ id, useInternalId, token, }: CreateChoosePromotionArgumentRule) => Promise<any>;
interface DeleteArgumentRule extends EntityRequest {
    ruleId: number;
}
declare const deleteArgumentRuleAsync: ({ id, useInternalId, token, ruleId, }: DeleteArgumentRule) => Promise<any>;
declare const fetchDestinationsAsync: ({ id, token }: EntityRequest) => Promise<any>;
interface Destination {
    destinationType: "Webhook" | "SegmentSourceFunction" | "HubspotContactProperty";
    endpoint: string;
    integratedSystemId: number;
}
interface CreateDestinationRequest extends EntityRequest {
    destination: Destination;
}
declare const createDestinationAsync: ({ id, token, destination, }: CreateDestinationRequest) => Promise<any>;
interface RemoveDestinationRequest extends EntityRequest {
    destinationId: number;
}
declare const removeDestinationAsync: ({ id, token, destinationId, }: RemoveDestinationRequest) => Promise<any>;
declare const fetchTriggerAsync: ({ id, token }: EntityRequest) => Promise<any>;
interface Trigger {
    featuresChanged: any;
}
interface SetTriggerRequest extends EntityRequest {
    trigger: Trigger;
}
declare const setTriggerAsync: ({ id, token, trigger, }: SetTriggerRequest) => Promise<any>;
declare const fetchLearningFeaturesAsync: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningFeaturesRequest extends EntityRequest {
    featureIds: string[];
}
declare const setLearningFeaturesAsync: ({ id, token, featureIds, useInternalId, }: SetLearningFeaturesRequest) => Promise<any>;
declare const fetchLearningMetricsAsync: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
interface SetLearningMetricsRequest extends EntityRequest {
    metricIds: string[];
}
declare const setLearningMetricsAsync: ({ id, token, metricIds, useInternalId, }: SetLearningMetricsRequest) => Promise<any>;
declare type CampaignStatistics = components["schemas"]["CampaignStatistics"];
declare const fetchStatisticsAsync: ({ id, token, }: EntityRequest) => Promise<CampaignStatistics>;
declare const fetchReportImageBlobUrlAsync: ({ id, token, useInternalId, }: EntityRequest) => Promise<any>;
declare type PerformanceResponse = components["schemas"]["ItemsRecommenderPerformanceReport"];
interface PerformanceRequest extends EntityRequest {
    reportId?: string | number | undefined;
}
declare const fetchPerformanceAsync: ({ token, id, reportId, }: PerformanceRequest) => Promise<PerformanceResponse>;
declare type PromotionOptimiser = components["schemas"]["PromotionOptimiser"];
declare const fetchPromotionOptimiserAsync: ({ token, useInternalId, id, }: EntityRequest) => Promise<PromotionOptimiser>;
interface SetAllPromotionOptimiserWeightsRequest extends EntityRequest {
    weights: components["schemas"]["UpdateWeightDto"][];
}
declare const setAllPromotionOptimiserWeightsAsync: ({ token, useInternalId, id, weights, }: SetAllPromotionOptimiserWeightsRequest) => Promise<PromotionOptimiser>;
interface SetPromotionOptimiserWeightRequest extends EntityRequest {
    weightId: number;
    weight: number;
}
declare const setPromotionOptimiserWeightAsync: ({ token, useInternalId, id, weightId, weight, }: SetPromotionOptimiserWeightRequest) => Promise<PromotionOptimiser>;
interface SetUseOptimiserRequest extends EntityRequest {
    useOptimiser: boolean;
}
declare const setUseOptimiserAsync: ({ token, useInternalId, id, useOptimiser, }: SetUseOptimiserRequest) => Promise<PromotionOptimiser>;
declare const fetchCampaignChannelsAsync: ({ id, token, }: EntityRequest) => Promise<any>;
declare type Channel = components["schemas"]["ChannelBase"];
interface AddCampaignChannelRequest extends EntityRequest {
    channel: components["schemas"]["AddCampaignChannelDto"];
}
declare const addCampaignChannelAsync: ({ token, id, channel, }: AddCampaignChannelRequest) => Promise<Channel>;
declare type PromotionsCampaigns = components["schemas"]["PromotionsCampaign"];
interface RemoveCampaignChannelRequest extends EntityRequest {
    channelId: number;
}
declare const removeCampaignChannelAsync: ({ id, token, channelId, }: RemoveCampaignChannelRequest) => Promise<PromotionsCampaigns>;
interface RecommendationRequest extends EntityRequest {
    recommendationId: number;
}
declare const fetchPromotionsRecommendationAsync: ({ token, recommendationId, }: RecommendationRequest) => Promise<any>;
interface OffersRequest extends PaginatedEntityRequest {
    page: number;
    offerState?: string;
}
declare const fetchOffersAsync: ({ token, page, pageSize, id, offerState, }: OffersRequest) => Promise<any>;
declare type ARPOReport = components["schemas"]["ARPOReportDto"];
declare const fetchARPOReportAsync: ({ token, id, }: EntityRequest) => Promise<ARPOReport>;
declare type APVReport = components["schemas"]["APVReportDto"];
declare const fetchAPVReportAsync: ({ token, id, }: EntityRequest) => Promise<APVReport>;
declare type OfferConversionRateReport = components["schemas"]["OfferConversionRateReportDto"];
declare const fetchOfferConversionRateReportAsync: ({ token, id, }: EntityRequest) => Promise<OfferConversionRateReport>;
declare type PerformanceReport = components["schemas"]["PerformanceReportDto"];
declare const fetchPerformanceReportAsync: ({ token, id, }: EntityRequest) => Promise<PerformanceReport>;

declare const promotionsCampaignsApi_d_fetchPromotionsCampaignsAsync: typeof fetchPromotionsCampaignsAsync;
declare const promotionsCampaignsApi_d_fetchPromotionsCampaignAsync: typeof fetchPromotionsCampaignAsync;
declare const promotionsCampaignsApi_d_fetchPromotionsRecommendationsAsync: typeof fetchPromotionsRecommendationsAsync;
declare const promotionsCampaignsApi_d_deletePromotionsCampaignAsync: typeof deletePromotionsCampaignAsync;
declare const promotionsCampaignsApi_d_createPromotionsCampaignAsync: typeof createPromotionsCampaignAsync;
declare const promotionsCampaignsApi_d_fetchAudienceAsync: typeof fetchAudienceAsync;
declare const promotionsCampaignsApi_d_addPromotionAsync: typeof addPromotionAsync;
declare const promotionsCampaignsApi_d_removePromotionAsync: typeof removePromotionAsync;
declare const promotionsCampaignsApi_d_setBaselinePromotionAsync: typeof setBaselinePromotionAsync;
declare const promotionsCampaignsApi_d_getBaselinePromotionAsync: typeof getBaselinePromotionAsync;
declare const promotionsCampaignsApi_d_createLinkRegisteredModelAsync: typeof createLinkRegisteredModelAsync;
declare const promotionsCampaignsApi_d_fetchLinkedRegisteredModelAsync: typeof fetchLinkedRegisteredModelAsync;
declare const promotionsCampaignsApi_d_invokePromotionsCampaignAsync: typeof invokePromotionsCampaignAsync;
declare const promotionsCampaignsApi_d_fetchInvokationLogsAsync: typeof fetchInvokationLogsAsync;
declare const promotionsCampaignsApi_d_fetchTargetVariablesAsync: typeof fetchTargetVariablesAsync;
declare const promotionsCampaignsApi_d_createTargetVariableAsync: typeof createTargetVariableAsync;
declare const promotionsCampaignsApi_d_setSettingsAsync: typeof setSettingsAsync;
declare const promotionsCampaignsApi_d_fetchArgumentsAsync: typeof fetchArgumentsAsync;
declare const promotionsCampaignsApi_d_setArgumentsAsync: typeof setArgumentsAsync;
declare const promotionsCampaignsApi_d_createChoosePromotionArgumentRuleAsync: typeof createChoosePromotionArgumentRuleAsync;
declare const promotionsCampaignsApi_d_updateChoosePromotionArgumentRuleAsync: typeof updateChoosePromotionArgumentRuleAsync;
declare const promotionsCampaignsApi_d_fetchChoosePromotionArgumentRulesAsync: typeof fetchChoosePromotionArgumentRulesAsync;
declare const promotionsCampaignsApi_d_createChooseSegmentArgumentRuleAsync: typeof createChooseSegmentArgumentRuleAsync;
declare const promotionsCampaignsApi_d_updateChooseSegmentArgumentRuleAsync: typeof updateChooseSegmentArgumentRuleAsync;
declare const promotionsCampaignsApi_d_fetchChooseSegmentArgumentRulesAsync: typeof fetchChooseSegmentArgumentRulesAsync;
declare const promotionsCampaignsApi_d_deleteArgumentRuleAsync: typeof deleteArgumentRuleAsync;
declare const promotionsCampaignsApi_d_fetchDestinationsAsync: typeof fetchDestinationsAsync;
declare const promotionsCampaignsApi_d_createDestinationAsync: typeof createDestinationAsync;
declare const promotionsCampaignsApi_d_removeDestinationAsync: typeof removeDestinationAsync;
declare const promotionsCampaignsApi_d_fetchTriggerAsync: typeof fetchTriggerAsync;
declare const promotionsCampaignsApi_d_setTriggerAsync: typeof setTriggerAsync;
declare const promotionsCampaignsApi_d_fetchLearningFeaturesAsync: typeof fetchLearningFeaturesAsync;
declare const promotionsCampaignsApi_d_setLearningFeaturesAsync: typeof setLearningFeaturesAsync;
declare const promotionsCampaignsApi_d_fetchLearningMetricsAsync: typeof fetchLearningMetricsAsync;
declare const promotionsCampaignsApi_d_setLearningMetricsAsync: typeof setLearningMetricsAsync;
declare const promotionsCampaignsApi_d_fetchStatisticsAsync: typeof fetchStatisticsAsync;
declare const promotionsCampaignsApi_d_fetchReportImageBlobUrlAsync: typeof fetchReportImageBlobUrlAsync;
declare const promotionsCampaignsApi_d_fetchPerformanceAsync: typeof fetchPerformanceAsync;
declare const promotionsCampaignsApi_d_fetchPromotionOptimiserAsync: typeof fetchPromotionOptimiserAsync;
declare const promotionsCampaignsApi_d_setAllPromotionOptimiserWeightsAsync: typeof setAllPromotionOptimiserWeightsAsync;
declare const promotionsCampaignsApi_d_setPromotionOptimiserWeightAsync: typeof setPromotionOptimiserWeightAsync;
declare const promotionsCampaignsApi_d_setUseOptimiserAsync: typeof setUseOptimiserAsync;
declare const promotionsCampaignsApi_d_fetchCampaignChannelsAsync: typeof fetchCampaignChannelsAsync;
declare const promotionsCampaignsApi_d_addCampaignChannelAsync: typeof addCampaignChannelAsync;
declare const promotionsCampaignsApi_d_removeCampaignChannelAsync: typeof removeCampaignChannelAsync;
declare const promotionsCampaignsApi_d_fetchPromotionsRecommendationAsync: typeof fetchPromotionsRecommendationAsync;
declare const promotionsCampaignsApi_d_fetchOffersAsync: typeof fetchOffersAsync;
declare const promotionsCampaignsApi_d_fetchARPOReportAsync: typeof fetchARPOReportAsync;
declare const promotionsCampaignsApi_d_fetchAPVReportAsync: typeof fetchAPVReportAsync;
declare const promotionsCampaignsApi_d_fetchOfferConversionRateReportAsync: typeof fetchOfferConversionRateReportAsync;
declare const promotionsCampaignsApi_d_fetchPerformanceReportAsync: typeof fetchPerformanceReportAsync;
declare namespace promotionsCampaignsApi_d {
  export {
    promotionsCampaignsApi_d_fetchPromotionsCampaignsAsync as fetchPromotionsCampaignsAsync,
    promotionsCampaignsApi_d_fetchPromotionsCampaignAsync as fetchPromotionsCampaignAsync,
    promotionsCampaignsApi_d_fetchPromotionsRecommendationsAsync as fetchPromotionsRecommendationsAsync,
    promotionsCampaignsApi_d_deletePromotionsCampaignAsync as deletePromotionsCampaignAsync,
    promotionsCampaignsApi_d_createPromotionsCampaignAsync as createPromotionsCampaignAsync,
    fetchPromotionsAsync$1 as fetchPromotionsAsync,
    promotionsCampaignsApi_d_fetchAudienceAsync as fetchAudienceAsync,
    promotionsCampaignsApi_d_addPromotionAsync as addPromotionAsync,
    promotionsCampaignsApi_d_removePromotionAsync as removePromotionAsync,
    promotionsCampaignsApi_d_setBaselinePromotionAsync as setBaselinePromotionAsync,
    promotionsCampaignsApi_d_getBaselinePromotionAsync as getBaselinePromotionAsync,
    promotionsCampaignsApi_d_createLinkRegisteredModelAsync as createLinkRegisteredModelAsync,
    promotionsCampaignsApi_d_fetchLinkedRegisteredModelAsync as fetchLinkedRegisteredModelAsync,
    promotionsCampaignsApi_d_invokePromotionsCampaignAsync as invokePromotionsCampaignAsync,
    promotionsCampaignsApi_d_fetchInvokationLogsAsync as fetchInvokationLogsAsync,
    promotionsCampaignsApi_d_fetchTargetVariablesAsync as fetchTargetVariablesAsync,
    promotionsCampaignsApi_d_createTargetVariableAsync as createTargetVariableAsync,
    promotionsCampaignsApi_d_setSettingsAsync as setSettingsAsync,
    promotionsCampaignsApi_d_fetchArgumentsAsync as fetchArgumentsAsync,
    promotionsCampaignsApi_d_setArgumentsAsync as setArgumentsAsync,
    promotionsCampaignsApi_d_createChoosePromotionArgumentRuleAsync as createChoosePromotionArgumentRuleAsync,
    promotionsCampaignsApi_d_updateChoosePromotionArgumentRuleAsync as updateChoosePromotionArgumentRuleAsync,
    promotionsCampaignsApi_d_fetchChoosePromotionArgumentRulesAsync as fetchChoosePromotionArgumentRulesAsync,
    promotionsCampaignsApi_d_createChooseSegmentArgumentRuleAsync as createChooseSegmentArgumentRuleAsync,
    promotionsCampaignsApi_d_updateChooseSegmentArgumentRuleAsync as updateChooseSegmentArgumentRuleAsync,
    promotionsCampaignsApi_d_fetchChooseSegmentArgumentRulesAsync as fetchChooseSegmentArgumentRulesAsync,
    promotionsCampaignsApi_d_deleteArgumentRuleAsync as deleteArgumentRuleAsync,
    promotionsCampaignsApi_d_fetchDestinationsAsync as fetchDestinationsAsync,
    promotionsCampaignsApi_d_createDestinationAsync as createDestinationAsync,
    promotionsCampaignsApi_d_removeDestinationAsync as removeDestinationAsync,
    promotionsCampaignsApi_d_fetchTriggerAsync as fetchTriggerAsync,
    promotionsCampaignsApi_d_setTriggerAsync as setTriggerAsync,
    promotionsCampaignsApi_d_fetchLearningFeaturesAsync as fetchLearningFeaturesAsync,
    promotionsCampaignsApi_d_setLearningFeaturesAsync as setLearningFeaturesAsync,
    promotionsCampaignsApi_d_fetchLearningMetricsAsync as fetchLearningMetricsAsync,
    promotionsCampaignsApi_d_setLearningMetricsAsync as setLearningMetricsAsync,
    promotionsCampaignsApi_d_fetchStatisticsAsync as fetchStatisticsAsync,
    promotionsCampaignsApi_d_fetchReportImageBlobUrlAsync as fetchReportImageBlobUrlAsync,
    promotionsCampaignsApi_d_fetchPerformanceAsync as fetchPerformanceAsync,
    promotionsCampaignsApi_d_fetchPromotionOptimiserAsync as fetchPromotionOptimiserAsync,
    promotionsCampaignsApi_d_setAllPromotionOptimiserWeightsAsync as setAllPromotionOptimiserWeightsAsync,
    promotionsCampaignsApi_d_setPromotionOptimiserWeightAsync as setPromotionOptimiserWeightAsync,
    promotionsCampaignsApi_d_setUseOptimiserAsync as setUseOptimiserAsync,
    promotionsCampaignsApi_d_fetchCampaignChannelsAsync as fetchCampaignChannelsAsync,
    promotionsCampaignsApi_d_addCampaignChannelAsync as addCampaignChannelAsync,
    promotionsCampaignsApi_d_removeCampaignChannelAsync as removeCampaignChannelAsync,
    promotionsCampaignsApi_d_fetchPromotionsRecommendationAsync as fetchPromotionsRecommendationAsync,
    promotionsCampaignsApi_d_fetchOffersAsync as fetchOffersAsync,
    promotionsCampaignsApi_d_fetchARPOReportAsync as fetchARPOReportAsync,
    promotionsCampaignsApi_d_fetchAPVReportAsync as fetchAPVReportAsync,
    promotionsCampaignsApi_d_fetchOfferConversionRateReportAsync as fetchOfferConversionRateReportAsync,
    promotionsCampaignsApi_d_fetchPerformanceReportAsync as fetchPerformanceReportAsync,
  };
}

declare type Auth0ReactConfig = components["schemas"]["Auth0ReactConfig"] | undefined;
declare const fetchAuth0ConfigurationAsync: () => Promise<Auth0ReactConfig>;
declare type ReactConfig = components["schemas"]["ReactConfig"] | undefined;
declare const fetchConfigurationAsync: () => Promise<ReactConfig>;
declare type Hosting$1 = components["schemas"]["Hosting"];
declare const fetchHostingAsync$1: () => Promise<Hosting$1>;

declare const reactConfigApi_d_fetchAuth0ConfigurationAsync: typeof fetchAuth0ConfigurationAsync;
declare const reactConfigApi_d_fetchConfigurationAsync: typeof fetchConfigurationAsync;
declare namespace reactConfigApi_d {
  export {
    reactConfigApi_d_fetchAuth0ConfigurationAsync as fetchAuth0ConfigurationAsync,
    reactConfigApi_d_fetchConfigurationAsync as fetchConfigurationAsync,
    fetchHostingAsync$1 as fetchHostingAsync,
  };
}

declare const fetchItemsAsync: ({ token, page, searchTerm, }: EntitySearchRequest) => Promise<PaginateResponse<RecommendableItem>>;
declare const fetchItemAsync: ({ token, id }: EntityRequest) => Promise<any>;
interface CreateItemRequest extends AuthenticatedRequest {
    item: components["schemas"]["CreatePromotionDto"];
}
declare const createItemAsync: ({ token, item, }: CreateItemRequest) => Promise<RecommendableItem>;
interface UpdateItemRequest extends EntityRequest {
    item: {
        name: string;
        description: string;
        properties: any;
    };
}
declare const updateItemAsync: ({ token, id, item, }: UpdateItemRequest) => Promise<RecommendableItem>;
declare const deleteItemAsync: ({ token, id, }: DeleteRequest) => Promise<DeleteResponse>;
declare const getPropertiesAsync$1: ({ token, id }: EntityRequest) => Promise<any>;
declare const setPropertiesAsync$1: ({ token, id, properties, }: SetpropertiesRequest) => Promise<any>;

declare const recommendableItemsApi_d_fetchItemsAsync: typeof fetchItemsAsync;
declare const recommendableItemsApi_d_fetchItemAsync: typeof fetchItemAsync;
declare const recommendableItemsApi_d_createItemAsync: typeof createItemAsync;
declare const recommendableItemsApi_d_updateItemAsync: typeof updateItemAsync;
declare const recommendableItemsApi_d_deleteItemAsync: typeof deleteItemAsync;
declare namespace recommendableItemsApi_d {
  export {
    recommendableItemsApi_d_fetchItemsAsync as fetchItemsAsync,
    recommendableItemsApi_d_fetchItemAsync as fetchItemAsync,
    recommendableItemsApi_d_createItemAsync as createItemAsync,
    recommendableItemsApi_d_updateItemAsync as updateItemAsync,
    recommendableItemsApi_d_deleteItemAsync as deleteItemAsync,
    getPropertiesAsync$1 as getPropertiesAsync,
    setPropertiesAsync$1 as setPropertiesAsync,
  };
}

declare const fetchPromotionsAsync: ({ token, page, searchTerm, promotionType, benefitType, weeksAgo, }: PromotionsRequest) => Promise<PaginateResponse<Promotion>>;
declare const fetchPromotionAsync: ({ token, id }: EntityRequest) => Promise<any>;
interface CreatePromotionRequest extends AuthenticatedRequest {
    promotion: components["schemas"]["CreatePromotionDto"];
}
declare const createPromotionAsync: ({ token, promotion, }: CreatePromotionRequest) => Promise<Promotion>;
interface UpdatePromotionRequest extends EntityRequest {
    promotion: {
        name: string;
        description: string;
        properties: any;
    };
}
declare const updatePromotionAsync: ({ token, id, promotion, }: UpdatePromotionRequest) => Promise<Promotion>;
declare const deletePromotionAsync: ({ token, id, }: DeleteRequest) => Promise<DeleteResponse>;
declare const getPropertiesAsync: ({ token, id }: EntityRequest) => Promise<any>;
declare const setPropertiesAsync: ({ token, id, properties, }: SetpropertiesRequest) => Promise<any>;

declare const promotionsApi_d_fetchPromotionsAsync: typeof fetchPromotionsAsync;
declare const promotionsApi_d_fetchPromotionAsync: typeof fetchPromotionAsync;
declare const promotionsApi_d_createPromotionAsync: typeof createPromotionAsync;
declare const promotionsApi_d_updatePromotionAsync: typeof updatePromotionAsync;
declare const promotionsApi_d_deletePromotionAsync: typeof deletePromotionAsync;
declare const promotionsApi_d_getPropertiesAsync: typeof getPropertiesAsync;
declare const promotionsApi_d_setPropertiesAsync: typeof setPropertiesAsync;
declare namespace promotionsApi_d {
  export {
    promotionsApi_d_fetchPromotionsAsync as fetchPromotionsAsync,
    promotionsApi_d_fetchPromotionAsync as fetchPromotionAsync,
    promotionsApi_d_createPromotionAsync as createPromotionAsync,
    promotionsApi_d_updatePromotionAsync as updatePromotionAsync,
    promotionsApi_d_deletePromotionAsync as deletePromotionAsync,
    promotionsApi_d_getPropertiesAsync as getPropertiesAsync,
    promotionsApi_d_setPropertiesAsync as setPropertiesAsync,
  };
}

declare function fetchReportsAsync({ token }: {
    token: any;
}): Promise<any>;
declare function downloadReportAsync({ token, reportName }: {
    token: any;
    reportName: any;
}): Promise<any>;

declare const reportsApi_d_fetchReportsAsync: typeof fetchReportsAsync;
declare const reportsApi_d_downloadReportAsync: typeof downloadReportAsync;
declare namespace reportsApi_d {
  export {
    reportsApi_d_fetchReportsAsync as fetchReportsAsync,
    reportsApi_d_downloadReportAsync as downloadReportAsync,
  };
}

declare function fetchSegmentsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function fetchSegmentAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createSegmentAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;
declare function deleteSegmentAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function addCustomerAsync({ token, id, customerId }: {
    token: any;
    id: any;
    customerId: any;
}): Promise<any>;
declare function removeCustomerAsync({ token, id, customerId }: {
    token: any;
    id: any;
    customerId: any;
}): Promise<any>;
declare function fetchSegmentCustomersAsync({ token, page, id, searchTerm, weeksAgo, }: {
    token: any;
    page: any;
    id: any;
    searchTerm: any;
    weeksAgo: any;
}): Promise<any>;
declare function fetchSegmentEnrolmentRulesAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function addSegmentEnrolmentRuleAsync({ token, id, payload }: {
    token: any;
    id: any;
    payload: any;
}): Promise<any>;
declare function removeSegmentEnrolmentRuleAsync({ token, id, ruleId, }: {
    token: any;
    id: any;
    ruleId: any;
}): Promise<any>;

declare const segmentsApi_d_fetchSegmentsAsync: typeof fetchSegmentsAsync;
declare const segmentsApi_d_fetchSegmentAsync: typeof fetchSegmentAsync;
declare const segmentsApi_d_createSegmentAsync: typeof createSegmentAsync;
declare const segmentsApi_d_deleteSegmentAsync: typeof deleteSegmentAsync;
declare const segmentsApi_d_addCustomerAsync: typeof addCustomerAsync;
declare const segmentsApi_d_removeCustomerAsync: typeof removeCustomerAsync;
declare const segmentsApi_d_fetchSegmentCustomersAsync: typeof fetchSegmentCustomersAsync;
declare const segmentsApi_d_fetchSegmentEnrolmentRulesAsync: typeof fetchSegmentEnrolmentRulesAsync;
declare const segmentsApi_d_addSegmentEnrolmentRuleAsync: typeof addSegmentEnrolmentRuleAsync;
declare const segmentsApi_d_removeSegmentEnrolmentRuleAsync: typeof removeSegmentEnrolmentRuleAsync;
declare namespace segmentsApi_d {
  export {
    segmentsApi_d_fetchSegmentsAsync as fetchSegmentsAsync,
    segmentsApi_d_fetchSegmentAsync as fetchSegmentAsync,
    segmentsApi_d_createSegmentAsync as createSegmentAsync,
    segmentsApi_d_deleteSegmentAsync as deleteSegmentAsync,
    segmentsApi_d_addCustomerAsync as addCustomerAsync,
    segmentsApi_d_removeCustomerAsync as removeCustomerAsync,
    segmentsApi_d_fetchSegmentCustomersAsync as fetchSegmentCustomersAsync,
    segmentsApi_d_fetchSegmentEnrolmentRulesAsync as fetchSegmentEnrolmentRulesAsync,
    segmentsApi_d_addSegmentEnrolmentRuleAsync as addSegmentEnrolmentRuleAsync,
    segmentsApi_d_removeSegmentEnrolmentRuleAsync as removeSegmentEnrolmentRuleAsync,
  };
}

declare function fetchTouchpointsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function fetchTouchpointAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createTouchpointMetadataAsync({ token, payload }: {
    token: any;
    payload: any;
}): Promise<any>;
declare function fetchTrackedUserTouchpointsAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function fetchTrackedUsersInTouchpointAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createTrackedUserTouchpointAsync({ token, id, touchpointCommonId, payload, }: {
    token: any;
    id: any;
    touchpointCommonId: any;
    payload: any;
}): Promise<any>;
declare function fetchTrackedUserTouchpointValuesAsync({ token, id, touchpointCommonId, }: {
    token: any;
    id: any;
    touchpointCommonId: any;
}): Promise<any>;

declare const touchpointsApi_d_fetchTouchpointsAsync: typeof fetchTouchpointsAsync;
declare const touchpointsApi_d_fetchTouchpointAsync: typeof fetchTouchpointAsync;
declare const touchpointsApi_d_createTouchpointMetadataAsync: typeof createTouchpointMetadataAsync;
declare const touchpointsApi_d_fetchTrackedUserTouchpointsAsync: typeof fetchTrackedUserTouchpointsAsync;
declare const touchpointsApi_d_fetchTrackedUsersInTouchpointAsync: typeof fetchTrackedUsersInTouchpointAsync;
declare const touchpointsApi_d_createTrackedUserTouchpointAsync: typeof createTrackedUserTouchpointAsync;
declare const touchpointsApi_d_fetchTrackedUserTouchpointValuesAsync: typeof fetchTrackedUserTouchpointValuesAsync;
declare namespace touchpointsApi_d {
  export {
    touchpointsApi_d_fetchTouchpointsAsync as fetchTouchpointsAsync,
    touchpointsApi_d_fetchTouchpointAsync as fetchTouchpointAsync,
    touchpointsApi_d_createTouchpointMetadataAsync as createTouchpointMetadataAsync,
    touchpointsApi_d_fetchTrackedUserTouchpointsAsync as fetchTrackedUserTouchpointsAsync,
    touchpointsApi_d_fetchTrackedUsersInTouchpointAsync as fetchTrackedUsersInTouchpointAsync,
    touchpointsApi_d_createTrackedUserTouchpointAsync as createTrackedUserTouchpointAsync,
    touchpointsApi_d_fetchTrackedUserTouchpointValuesAsync as fetchTrackedUserTouchpointValuesAsync,
  };
}

declare const fetchTrackedUsersAsync: ({ token, page, searchTerm }: {
    token: any;
    page: any;
    searchTerm: any;
}) => Promise<any>;
declare const updateMergePropertiesAsync: ({ token, id, properties }: {
    token: any;
    id: any;
    properties: any;
}) => Promise<any>;
declare const fetchTrackedUserAsync: ({ token, id, useInternalId }: {
    token: any;
    id: any;
    useInternalId: any;
}) => Promise<any>;
declare const fetchUniqueTrackedUserActionGroupsAsync: ({ token, id }: {
    token: any;
    id: any;
}) => Promise<any>;
declare const fetchLatestRecommendationsAsync: ({ token, id }: {
    token: any;
    id: any;
}) => Promise<any>;
declare const fetchTrackedUserActionAsync: ({ token, id, category, actionName, }: {
    token: any;
    id: any;
    category: any;
    actionName: any;
}) => Promise<any>;
declare const uploadUserDataAsync: ({ token, payload }: {
    token: any;
    payload: any;
}) => Promise<any>;
declare const createOrUpdateTrackedUserAsync: ({ token, customer, user, }: {
    token: any;
    customer: any;
    user: any;
}) => Promise<any>;
declare const fetchTrackedUsersActionsAsync: ({ token, page, id, revenueOnly, }: {
    token: any;
    page: any;
    id: any;
    revenueOnly: any;
}) => Promise<any>;
declare const deleteCustomerAsync: ({ token, id }: {
    token: any;
    id: any;
}) => Promise<any>;

declare const trackedUsersApi_d_fetchTrackedUsersAsync: typeof fetchTrackedUsersAsync;
declare const trackedUsersApi_d_updateMergePropertiesAsync: typeof updateMergePropertiesAsync;
declare const trackedUsersApi_d_fetchTrackedUserAsync: typeof fetchTrackedUserAsync;
declare const trackedUsersApi_d_fetchUniqueTrackedUserActionGroupsAsync: typeof fetchUniqueTrackedUserActionGroupsAsync;
declare const trackedUsersApi_d_fetchLatestRecommendationsAsync: typeof fetchLatestRecommendationsAsync;
declare const trackedUsersApi_d_fetchTrackedUserActionAsync: typeof fetchTrackedUserActionAsync;
declare const trackedUsersApi_d_uploadUserDataAsync: typeof uploadUserDataAsync;
declare const trackedUsersApi_d_createOrUpdateTrackedUserAsync: typeof createOrUpdateTrackedUserAsync;
declare const trackedUsersApi_d_fetchTrackedUsersActionsAsync: typeof fetchTrackedUsersActionsAsync;
declare const trackedUsersApi_d_deleteCustomerAsync: typeof deleteCustomerAsync;
declare namespace trackedUsersApi_d {
  export {
    trackedUsersApi_d_fetchTrackedUsersAsync as fetchTrackedUsersAsync,
    trackedUsersApi_d_updateMergePropertiesAsync as updateMergePropertiesAsync,
    trackedUsersApi_d_fetchTrackedUserAsync as fetchTrackedUserAsync,
    trackedUsersApi_d_fetchUniqueTrackedUserActionGroupsAsync as fetchUniqueTrackedUserActionGroupsAsync,
    trackedUsersApi_d_fetchLatestRecommendationsAsync as fetchLatestRecommendationsAsync,
    trackedUsersApi_d_fetchTrackedUserActionAsync as fetchTrackedUserActionAsync,
    trackedUsersApi_d_uploadUserDataAsync as uploadUserDataAsync,
    trackedUsersApi_d_createOrUpdateTrackedUserAsync as createOrUpdateTrackedUserAsync,
    trackedUsersApi_d_fetchTrackedUsersActionsAsync as fetchTrackedUsersActionsAsync,
    trackedUsersApi_d_deleteCustomerAsync as deleteCustomerAsync,
  };
}

declare type Tenant = components["schemas"]["Tenant"];
declare type BillingAccount = components["schemas"]["BillingAccount"];
declare type PaginatedMemberships = components["schemas"]["UserInfoPaginated"];
declare type Hosting = components["schemas"]["Hosting"];
declare const fetchCurrentTenantAsync: ({ token, }: AuthenticatedRequest) => Promise<Tenant>;
declare const fetchAccountAsync: ({ token, id, }: EntityRequest) => Promise<BillingAccount>;
declare const fetchHostingAsync: ({ token, }: AuthenticatedRequest) => Promise<Hosting>;
declare const fetchCurrentTenantMembershipsAsync: ({ token, }: AuthenticatedRequest) => Promise<PaginatedMemberships>;
interface CreateTenantMembershipRequest extends AuthenticatedRequest {
    email: string;
}
declare const createTenantMembershipAsync: ({ token, email, }: CreateTenantMembershipRequest) => Promise<any>;

declare const tenantsApi_d_fetchCurrentTenantAsync: typeof fetchCurrentTenantAsync;
declare const tenantsApi_d_fetchAccountAsync: typeof fetchAccountAsync;
declare const tenantsApi_d_fetchHostingAsync: typeof fetchHostingAsync;
declare const tenantsApi_d_fetchCurrentTenantMembershipsAsync: typeof fetchCurrentTenantMembershipsAsync;
declare const tenantsApi_d_createTenantMembershipAsync: typeof createTenantMembershipAsync;
declare namespace tenantsApi_d {
  export {
    tenantsApi_d_fetchCurrentTenantAsync as fetchCurrentTenantAsync,
    tenantsApi_d_fetchAccountAsync as fetchAccountAsync,
    tenantsApi_d_fetchHostingAsync as fetchHostingAsync,
    tenantsApi_d_fetchCurrentTenantMembershipsAsync as fetchCurrentTenantMembershipsAsync,
    tenantsApi_d_createTenantMembershipAsync as createTenantMembershipAsync,
  };
}

declare function fetchRewardSelectorsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
declare function fetchRewardSelectorAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function deleteRewardSelectorAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
declare function createRewardSelectorAsync({ token, entity }: {
    token: any;
    entity: any;
}): Promise<any>;

declare const rewardSelectorsApi_d_fetchRewardSelectorsAsync: typeof fetchRewardSelectorsAsync;
declare const rewardSelectorsApi_d_fetchRewardSelectorAsync: typeof fetchRewardSelectorAsync;
declare const rewardSelectorsApi_d_deleteRewardSelectorAsync: typeof deleteRewardSelectorAsync;
declare const rewardSelectorsApi_d_createRewardSelectorAsync: typeof createRewardSelectorAsync;
declare namespace rewardSelectorsApi_d {
  export {
    rewardSelectorsApi_d_fetchRewardSelectorsAsync as fetchRewardSelectorsAsync,
    rewardSelectorsApi_d_fetchRewardSelectorAsync as fetchRewardSelectorAsync,
    rewardSelectorsApi_d_deleteRewardSelectorAsync as deleteRewardSelectorAsync,
    rewardSelectorsApi_d_createRewardSelectorAsync as createRewardSelectorAsync,
  };
}

declare const setBaseUrl: (baseUrl: string) => void;

declare const setTenant: (tenant: string) => void;
declare const setDefaultEnvironmentId: (e: number) => void;
declare const setDefaultApiKey: (k: string) => void;

declare type ErrorHandler = (response: AxiosResponse) => Promise<any>;
declare const setErrorResponseHandler: (errorHandler: ErrorHandler) => void;
declare const handleErrorResponse: ErrorHandler;

declare const errorHandling_d_setErrorResponseHandler: typeof setErrorResponseHandler;
declare const errorHandling_d_handleErrorResponse: typeof handleErrorResponse;
declare namespace errorHandling_d {
  export {
    errorHandling_d_setErrorResponseHandler as setErrorResponseHandler,
    errorHandling_d_handleErrorResponse as handleErrorResponse,
  };
}

export { activityFeedApi_d as activityFeed, apiKeyApi_d as apiKeys, axiosInstance_d as axiosInstance, businessesApi_d as businesses, channelsApi_d as channels, components, customersApi_d as customers, dataSummaryApi_d as dataSummary, deploymentApi_d as deployment, environmentsApi_d as environments, errorHandling_d as errorHandling, eventsApi_d as events, featureGeneratorsApi_d as featureGenerators, featuresApi_d as features, integratedSystemsApi_d as integratedSystems, itemsRecommendersApi_d as itemsRecommenders, metricGeneratorsApi_d as metricGenerators, metricsApi_d as metrics, modelRegistrationsApi_d as modelRegistrations, index_d as models, parameterSetCampaignsApi_d as parameterSetCampaigns, parameterSetRecommendersApi_d as parameterSetRecommenders, parametersApi_d as parameters, profileApi_d as profile, promotionsApi_d as promotions, promotionsCampaignsApi_d as promotionsCampaigns, promotionsRecommendersApi_d as promotionsRecommenders, reactConfigApi_d as reactConfig, recommendableItemsApi_d as recommendableItems, reportsApi_d as reports, rewardSelectorsApi_d as rewardSelectors, segmentsApi_d as segments, setBaseUrl, setDefaultApiKey, setDefaultEnvironmentId, setTenant, tenantsApi_d as tenants, touchpointsApi_d as touchpoints, trackedUsersApi_d as trackedUsers };
