import { executeFetch } from "./client/apiClientTs";
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
