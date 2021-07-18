import { getUrl } from "../../../baseUrl";
import { headers } from "../../headers";

export const fetchRecommenderTargetVariableValuesAsync = async ({
  recommenderApiName,
  name,
  token,
  id,
}) => {
  const url = getUrl(
    `api/recommenders/${recommenderApiName}/${id}/TargetVariableValues?name=${
      name || ""
    }`
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

export const createRecommenderTargetVariableValueAsync = async ({
  recommenderApiName,
  targetVariableValue,
  token,
  id,
}) => {
  const url = getUrl(
    `api/recommenders/${recommenderApiName}/${id}/TargetVariableValues`
  );
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(targetVariableValue),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
