import {
  Channel,
  AuthenticatedRequest,
  EntitySearchRequest,
  PaginateResponse,
  EntityRequest,
  DeleteRequest,
} from "../interfaces";
import { executeFetch } from "./client/apiClient";
import { components } from "../model/api";

export const fetchChannelsAsync = async ({
  token,
  page,
}: EntitySearchRequest): Promise<PaginateResponse<Channel>> => {
  return await executeFetch({
    path: "api/Channels",
    token,
    page,
  });
};

interface CreateChannelRequest extends AuthenticatedRequest {
  channel: components["schemas"]["CreateChannelDto"];
}
export const createChannelAsync = async ({
  token,
  channel,
}: CreateChannelRequest) => {
  return await executeFetch({
    path: "api/Channels",
    token,
    method: "post",
    body: channel,
  });
};

export const fetchChannelAsync = async ({
  token,
  id,
}: EntityRequest): Promise<components["schemas"]["ChannelBase"]> => {
  return await executeFetch({
    path: `api/Channels/${id}`,
    token,
  });
};

export const deleteChannelAsync = async ({ token, id }: DeleteRequest) => {
  return await executeFetch({
    path: `api/Channels/${id}`,
    token,
    method: "delete",
  });
};

interface UpdateChannelEnpointRequest extends EntityRequest {
  endpoint: string;
}
export const updateChannelEndpointAsync = async ({
  token,
  id,
  endpoint,
}: UpdateChannelEnpointRequest) => {
  return await executeFetch({
    token,
    path: `api/Channels/${id}/Endpoint`,
    method: "post",
    body: endpoint,
  });
};

interface UpdateChannelPropertiesRequest extends EntityRequest {
  properties: {
    popupAskForEmail: boolean;
    popupDelay: number;
    popupHeader: string;
    popupSubheader: string;
    recommenderId: number;
    customerIdPrefix: string;
    storageKey: string;
  };
}
export const updateChannelPropertiesAsync = async ({
  token,
  id,
  properties,
}: UpdateChannelPropertiesRequest) => {
  return await executeFetch({
    token,
    path: `api/Channels/${id}/WebProperties`,
    method: "post",
    body: properties,
  });
};

interface UpdateEmailChannelTriggerRequest extends EntityRequest {
  listTrigger: {
    listId: string;
    listName: string;
  };
}
export const updateEmailChannelTriggerAsync = async ({
  token,
  id,
  listTrigger,
}: UpdateEmailChannelTriggerRequest) => {
  return await executeFetch({
    token,
    path: `api/Channels/${id}/EmailTrigger`,
    method: "post",
    body: listTrigger,
  });
};
