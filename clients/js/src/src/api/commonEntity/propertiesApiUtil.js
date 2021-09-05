import { getUrl } from "../../baseUrl";
import { headers } from "../headers";

export const getPropertiesAsync = async ({ api, token, id }) => {
  const url = getUrl(`api/${api}/${id}/Properties`);
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const setPropertiesAsync = async ({ api, token, id, properties }) => {
  const url = getUrl(`api/${api}/${id}/Properties`);
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(properties),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
