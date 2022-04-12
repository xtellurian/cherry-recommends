import { headers } from "../../client/headers";
import { handleErrorResponse } from "../../../utilities/errorHandling";
import { current } from "../../client/axiosInstance";

interface ReportImageRequest {
  recommenderApiName:
    | "PromotionsRecommenders"
    | "ParameterSetRecommenders"
    | "ItemsRecommenders";
  token: string;
  id: string | number;
  useInternalId?: boolean;
}

export const fetchReportImageBlobUrlAsync = async ({
  recommenderApiName,
  token,
  id,
  useInternalId,
}: ReportImageRequest) => {
  console.debug("fetching image for recommender");
  console.debug(`api/recommenders/${recommenderApiName}/${id}/ReportImage`);
  const axios = current();
  let response;
  try {
    response = await axios.get(
      `api/recommenders/${recommenderApiName}/${id}/ReportImage`,
      {
        headers: headers(token, null),
        responseType: "blob", // required for axios to understand response
      }
    );
  } catch (ex) {
    console.error(ex);
    throw ex;
  }

  if (response.status >= 200 && response.status < 300) {
    return URL.createObjectURL(response.data);
  } else {
    handleErrorResponse(response);
  }
};
