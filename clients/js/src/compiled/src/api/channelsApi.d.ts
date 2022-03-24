import { AuthenticatedRequest } from "../interfaces";
import { components } from "../model/api";
interface CreateChannelRequest extends AuthenticatedRequest {
    channel: components["schemas"]["CreateChannelDto"];
}
export declare const createChannelAsync: ({ token, channel, }: CreateChannelRequest) => Promise<any>;
export {};
