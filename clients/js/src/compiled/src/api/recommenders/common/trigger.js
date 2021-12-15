import { executeFetch } from "../../client/apiClient";
export const setTriggerAsync = async ({ recommenderApiName, token, id, trigger, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/TriggerCollection`,
        token,
        method: "post",
        body: trigger,
    });
};
export const fetchTriggerAsync = async ({ recommenderApiName, token, id }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/${recommenderApiName}/${id}/TriggerCollection`,
    });
};
