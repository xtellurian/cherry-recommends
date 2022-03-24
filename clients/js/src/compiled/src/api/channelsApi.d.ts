import { Channel, AuthenticatedRequest, EntitySearchRequest, PaginateResponse } from "../interfaces";
import { components } from "../model/api";
export declare const fetchChannelsAsync: ({ token, page, }: EntitySearchRequest) => Promise<PaginateResponse<Channel>>;
interface CreateChannelRequest extends AuthenticatedRequest {
    channel: components["schemas"]["CreateChannelDto"];
}
export declare const createChannelAsync: ({ token, channel, }: CreateChannelRequest) => Promise<any>;
export {};
