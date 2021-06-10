import { getUrl } from "../baseUrl";
const defaultHeaders = { "Content-Type": "application/json" };

export const fetchUserEvents = async ({
  success,
  error,
  token,
  commonUserId,
}) => {
  const url = getUrl("api/events");
  let path = `${url}?commonUserId=${commonUserId}`;

  const response = await fetch(path, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(await response.json());
  }
};
