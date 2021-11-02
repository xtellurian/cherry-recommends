import { executeFetch } from "../../client/apiClient";

export const fetchRecommenderTrackedUserActionsAsync = async ({
  recommenderApiName,
  revenueOnly,
  token,
  id,
  page,
}) => {
  return await executeFetch({
    path: `api/recommenders/${recommenderApiName}/${id}/TrackedUserActions`,
    token,
    page,
    query: {
      revenueOnly,
    },
  });
};
