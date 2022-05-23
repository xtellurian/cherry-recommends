import { executeFetch } from "./client/apiClient";
export const fetchActivityFeedEntitiesAsync = async ({ token, page, }) => {
    return await executeFetch({
        path: "api/ActivityFeed",
        token,
        page,
    });
};
