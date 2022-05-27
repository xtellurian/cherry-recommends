import { executeFetch } from "../../client/apiClient";
export const fetchDestinationsAsync = async ({ campaignApiName, token, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/${campaignApiName}/${id}/Destinations`,
    });
};
export const createDestinationAsync = async ({ campaignApiName, token, id, destination, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/${campaignApiName}/${id}/Destinations`,
        method: "post",
        body: destination,
    });
};
export const removeDestinationAsync = async ({ campaignApiName, token, id, destinationId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/Destinations/${destinationId}`,
        token,
        method: "delete",
    });
};
