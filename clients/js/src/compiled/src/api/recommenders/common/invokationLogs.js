import { executeFetch } from "../../client/apiClient";
export const fetchRecommenderInvokationLogsAsync = async ({ recommenderApiName, token, id, page, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/InvokationLogs`,
        page,
        token,
    });
};
