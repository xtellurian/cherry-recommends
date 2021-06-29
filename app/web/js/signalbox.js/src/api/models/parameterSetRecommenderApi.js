import { getUrl } from "../../baseUrl";
const defaultHeaders = { "Content-Type": "application/json" };

export const invokeParameterSetRecommenderModel = async ({
  success,
  error,
  onFinally,
  token,
  id,
  version,
  input,
}) => {
  try {
    const url = getUrl(`api/models/ParameterSetRecommenders/${id}/invoke`);
    const result = await fetch(`${url}?version=${version || "default"}`, {
      headers: !token
        ? defaultHeaders
        : { ...defaultHeaders, Authorization: `Bearer ${token}` },
      method: "post",
      body: JSON.stringify(input),
    });
    if (result.ok) {
      success(await result.json());
    } else {
      error(await result.json());
    }
  } finally {
    if (onFinally) {
      onFinally();
    }
  }
};
