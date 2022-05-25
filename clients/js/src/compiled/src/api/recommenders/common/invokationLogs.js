import { executeFetch } from "../../client/apiClient";
export const fetchRecommenderInvokationLogsAsync = async ({ recommenderApiName, token, id, page, pageSize, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/InvokationLogs`,
        page,
        pageSize,
        token,
    });
};
