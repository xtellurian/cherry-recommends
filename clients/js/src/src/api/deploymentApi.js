import { getUrl } from "../baseUrl";
const defaultHeaders = { "Content-Type": "application/json" };

export const fetchDeploymentConfiguration = async ({
  success,
  error,
  token,
}) => {
  const url = getUrl(`api/deployment/configuration`);

  const result = await fetch(url, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (result.ok) {
    success(await result.json());
  } else {
    error(await result.json());
  }
};
