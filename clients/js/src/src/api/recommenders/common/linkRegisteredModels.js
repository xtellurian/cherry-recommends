import { getUrl } from "../../../baseUrl";
import { headers } from "../../headers";

export const fetchLinkedRegisteredModelAsync = async ({
  recommenderApiName,
  token,
  id,
}) => {
  const url = getUrl(
    `api/recommenders/${recommenderApiName}/${id}/ModelRegistration`
  );
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const createLinkedRegisteredModelAsync = async ({
  recommenderApiName,
  token,
  id,
  modelId,
}) => {
  const url = getUrl(
    `api/recommenders/${recommenderApiName}/${id}/ModelRegistration`
  );
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify({ modelId }),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
