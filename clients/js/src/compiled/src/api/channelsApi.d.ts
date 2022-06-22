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
        customerIdPrefix: string;
        storageKey: string;
        conditionalAction: string;
        conditions: {
            id: string;
            parameter: string;
            operator: string;
            value: string;
        }[];
    };
}
export declare const updateChannelPropertiesAsync: ({ token, id, properties, }: UpdateChannelPropertiesRequest) => Promise<any>;
interface UpdateEmailChannelTriggerRequest extends EntityRequest {
    listTrigger: {
        listId: string;
        listName: string;
    };
}
export declare const updateEmailChannelTriggerAsync: ({ token, id, listTrigger, }: UpdateEmailChannelTriggerRequest) => Promise<any>;
export declare type ConditionalActions = components["schemas"]["PopupConditionalActions"];
interface ConditionalActionsConstants {
    none: ConditionalActions;
    allow: ConditionalActions;
    block: ConditionalActions;
}
export declare const conditionalActions: ConditionalActionsConstants;
export {};
