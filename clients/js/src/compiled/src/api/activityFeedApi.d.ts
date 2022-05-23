import { EntitySearchRequest } from "../interfaces";
import { components } from "../model/api";
declare type ActivityFeedEntities = components["schemas"]["ActivityFeedEntity"][];
export declare const fetchActivityFeedEntitiesAsync: ({ token, page, }: EntitySearchRequest) => Promise<ActivityFeedEntities>;
export {};
