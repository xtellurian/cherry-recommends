import { executeFetch } from "../../client/apiClient";
export const fetchDestinationsAsync = async ({ recommenderApiName, token, id, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/${recommenderApiName}/${id}/Destinations`,
    });
};
export const createDestinationAsync = async ({ recommenderApiName, token, id, destination, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/${recommenderApiName}/${id}/Destinations`,
        method: "post",
        body: destination,
    });
};
export const removeDestinationAsync = async ({ recommenderApiName, token, id, destinationId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/Destinations/${destinationId}`,
        token,
        method: "delete",
    });
};
