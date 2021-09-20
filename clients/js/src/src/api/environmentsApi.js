import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";
import { headers, setDefaultEnvironmentId as setEnv } from "./headers";

export const fetchEnvironmentsAsync = async ({ token, page }) => {
  const url = getUrl("api/Environments");
  const response = await fetch(`${url}?${pageQuery(page)}`, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw response.json();
  }
};

export const createEnvironmentAsync = async ({ token, environment }) => {
  const url = getUrl("api/Environments");
  const response = await fetch(`${url}?`, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(environment),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw response.json();
  }
};

export const deleteEnvironmentAsync = async ({ token, id }) => {
  const url = getUrl(`api/Environments/${id}`);
  const response = await fetch(`${url}?`, {
    headers: headers(token),
    method: "delete",
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw response.json();
  }
};

export const setDefaultEnvironmentId = setEnv;
