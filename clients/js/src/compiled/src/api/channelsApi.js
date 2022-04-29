import { executeFetch } from "./client/apiClient";
export const fetchChannelsAsync = async ({ token, page, }) => {
    return await executeFetch({
        path: "api/Channels",
        token,
        page,
    });
};
export const createChannelAsync = async ({ token, channel, }) => {
    return await executeFetch({
        path: "api/Channels",
        token,
        method: "post",
        body: channel,
    });
};
export const fetchChannelAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Channels/${id}`,
        token,
    });
};
export const deleteChannelAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Channels/${id}`,
        token,
        method: "delete",
    });
};
export const updateChannelEndpointAsync = async ({ token, id, endpoint, }) => {
    return await executeFetch({
        token,
        path: `api/Channels/${id}/Endpoint`,
        method: "post",
        body: endpoint,
    });
};
export const updateChannelPropertiesAsync = async ({ token, id, properties, }) => {
    return await executeFetch({
        token,
        path: `api/Channels/${id}/WebProperties`,
        method: "post",
        body: properties,
    });
};
export const updateEmailChannelTriggerAsync = async ({ token, id, listTrigger, }) => {
    return await executeFetch({
        token,
        path: `api/Channels/${id}/EmailTrigger`,
        method: "post",
        body: listTrigger,
    });
};
