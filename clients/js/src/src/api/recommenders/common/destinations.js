import { getUrl } from "../../../baseUrl";
import { headers } from "../../headers";

export const fetchDestinationsAsync = async ({
  recommenderApiName,
  token,
  id,
}) => {
  const url = getUrl(
    `api/recommenders/${recommenderApiName}/${id}/Destinations`
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

export const createDestinationAsync = async ({
  recommenderApiName,
  token,
  id,
  destination,
}) => {
  const url = getUrl(
    `api/recommenders/${recommenderApiName}/${id}/Destinations`
  );
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(destination),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const removeDestinationAsync = async ({
  recommenderApiName,
  token,
  id,
  destinationId,
}) => {
  const url = getUrl(
    `api/recommenders/${recommenderApiName}/${id}/Destinations/${destinationId}`
  );
  const response = await fetch(url, {
    headers: headers(token),
    method: "delete",
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
