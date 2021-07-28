import { getUrl } from "../../../baseUrl";
import { headers } from "../../headers";

export const updateErrorHandlingAsync = async ({
  recommenderApiName,
  token,
  id,
  errorHandling,
}) => {
  const url = getUrl(
    `api/recommenders/${recommenderApiName}/${id}/ErrorHandling`
  );
  const response = await fetch(url, {
    headers: headers(token),
    method: "post",
    body: JSON.stringify(errorHandling),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
