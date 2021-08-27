import { getUrl } from "../../../baseUrl";
import { headers } from "../../headers";
import { pageQuery } from "../../paging";

export const fetchRecommenderTrackedUserActionsAsync = async ({
  recommenderApiName,
  revenueOnly,
  token,
  id,
  page,
}) => {
  let url = getUrl(
    `api/recommenders/${recommenderApiName}/${id}/TrackedUserActions`
  );

  url = `${url}?${pageQuery(page)}&revenueOnly=${!!revenueOnly}`;
  const response = await fetch(url, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
