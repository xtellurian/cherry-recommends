import { current } from "./axiosInstance";
import { getBaseUrl } from "./baseUrl";
import { headers } from "./headers";
import { handleErrorResponse } from "../../utilities/errorHandling";

interface ExecuteFetchRequest {
  method?: "get" | "post" | "put" | "delete" | undefined;
  token?: string;
  apiKey?: string;
  path: string;
  page?: number | null;
  pageSize?: number | null;
  body?: object | string | null;
  query?: object;
  responseType?:
    | "json"
    | "blob"
    | "arraybuffer"
    | "document"
    | "text"
    | "stream"
    | undefined;
}
export const executeFetch = async (
  {
    token,
    apiKey,
    path,
    page,
    pageSize,
    body,
    method,
    query,
    responseType,
  }: ExecuteFetchRequest = { method: "get", path: "/" }
) => {
  const baseUrl = getBaseUrl();
  const client = current({ baseUrl: baseUrl });
  const params = new URLSearchParams();
  for (const [key, value] of Object.entries(query || {})) {
    if (key && value) {
      params.append(key, value);
    }
  }
  if (page) {
    params.append("p.page", `${page}`);
  }
  if (pageSize) {
    params.append("p.pageSize", `${pageSize}`);
  }
  if (apiKey) {
    params.append("apiKey", `${apiKey}`);
  }
  let response;
  try {
    response = await client({
      method,
      url: path,
      params,
      headers: headers(token, null),
      data: body,
      responseType: responseType,
    });
  } catch (ex) {
    // something failed. ther server responded outside of 2xx
    // if we always resolve the promise, then this is always true
    console.error(ex);
    throw ex;
  }
  // happy path
  if (response.status <= 299) {
    return response.data;
  } else {
    console.debug(response);
    return await handleErrorResponse(response);
  }
};
