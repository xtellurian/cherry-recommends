import { getUrl } from "../../baseUrl";
const defaultHeaders = { "Content-Type": "application/json" };

export const invokeGenericModel = async ({
  success,
  error,
  onFinally,
  token,
  id,
  input,
}) => {
  try {
    const url = getUrl(`api/models/generic/${id}/invoke`);
    const result = await fetch(url, {
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
