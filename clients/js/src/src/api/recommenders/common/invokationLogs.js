import { getUrl } from "../../../baseUrl";
import { headers } from "../../headers";
import { pageQuery } from "../../paging";

export const fetchRecommenderInvokationLogsAsync = async ({
  recommenderApiName,
  token,
  id,
  page
}) => {
  const url = getUrl(
    `api/recommenders/${recommenderApiName}/${id}/InvokationLogs?${pageQuery(
      page
    )}`
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
