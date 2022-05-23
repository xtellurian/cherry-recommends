import { EntitySearchRequest, PaginateResponse } from "../interfaces";
import { executeFetch } from "./client/apiClient";
import { components } from "../model/api";

type ActivityFeedEntities = components["schemas"]["ActivityFeedEntity"][];

export const fetchActivityFeedEntitiesAsync = async ({
  token,
  page,
}: EntitySearchRequest): Promise<ActivityFeedEntities> => {
  return await executeFetch({
    path: "api/ActivityFeed",
    token,
    page,
  });
};
