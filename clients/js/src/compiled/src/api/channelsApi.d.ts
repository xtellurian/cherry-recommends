import { Channel, AuthenticatedRequest, EntitySearchRequest, PaginateResponse, EntityRequest, DeleteRequest } from "../interfaces";
import { components } from "../model/api";
export declare const fetchChannelsAsync: ({ token, page, }: EntitySearchRequest) => Promise<PaginateResponse<Channel>>;
interface CreateChannelRequest extends AuthenticatedRequest {
    channel: components["schemas"]["CreateChannelDto"];
}
export declare const createChannelAsync: ({ token, channel, }: CreateChannelRequest) => Promise<any>;
export declare const fetchChannelAsync: ({ token, id, }: EntityRequest) => Promise<components["schemas"]["ChannelBase"]>;
export declare const deleteChannelAsync: ({ token, id }: DeleteRequest) => Promise<any>;
interface UpdateChannelEnpointRequest extends EntityRequest {
    endpoint: string;
}
export declare const updateChannelEndpointAsync: ({ token, id, endpoint, }: UpdateChannelEnpointRequest) => Promise<any>;
interface UpdateChannelPropertiesRequest extends EntityRequest {
    properties: {
        popupAskForEmail: boolean;
        popupDelay: number;
        popupHeader: string;
        popupSubheader: string;
        recommenderId: number;
    };
}
export declare const updateChannelPropertiesAsync: ({ token, id, properties, }: UpdateChannelPropertiesRequest) => Promise<any>;
export {};
