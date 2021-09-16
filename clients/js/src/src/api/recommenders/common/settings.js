import { getUrl } from "../../../baseUrl";
import { headers } from "../../headers";

export const setSettingsAsync = async ({
  recommenderApiName,
  token,
  id,
  settings,
}) => {
  const url = getUrl(`api/recommenders/${recommenderApiName}/${id}/Settings`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(settings),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const getSettingsAsync = async ({ recommenderApiName, token, id }) => {
  const url = getUrl(`api/recommenders/${recommenderApiName}/${id}/Settings`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
