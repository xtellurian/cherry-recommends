import {
  Channel,
  AuthenticatedRequest,
  EntitySearchRequest,
  PaginateResponse,
} from "../interfaces";
import { executeFetch } from "./client/apiClientTs";
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
