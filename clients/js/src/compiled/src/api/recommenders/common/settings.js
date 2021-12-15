import { executeFetch } from "../../client/apiClient";
export const setSettingsAsync = async ({ recommenderApiName, token, id, settings, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/Settings`,
        token,
        method: "post",
        body: settings,
    });
};
export const getSettingsAsync = async ({ recommenderApiName, token, id }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/${recommenderApiName}/${id}/Settings`,
    });
};
