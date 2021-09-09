import { getUrl } from "../../../baseUrl";
import { headers } from "../../headers";

export const setArgumentsAsync = async ({
  recommenderApiName,
  token,
  id,
  args,
}) => {
  const url = getUrl(`api/recommenders/${recommenderApiName}/${id}/Arguments`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(args),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
