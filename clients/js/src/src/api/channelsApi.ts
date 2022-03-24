import {
  AuthenticatedRequest,
} from "../interfaces";
import { executeFetch } from "./client/apiClientTs";
import { components } from "../model/api";

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
