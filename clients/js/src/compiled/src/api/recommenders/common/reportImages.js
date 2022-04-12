import { headers } from "../../client/headers";
import { handleErrorResponse } from "../../../utilities/errorHandling";
import { current } from "../../client/axiosInstance";
export const fetchReportImageBlobUrlAsync = async ({ recommenderApiName, token, id, useInternalId, }) => {
    console.debug("fetching image for recommender");
    console.debug(`api/recommenders/${recommenderApiName}/${id}/ReportImage`);
    const axios = current();
    let response;
    try {
        response = await axios.get(`api/recommenders/${recommenderApiName}/${id}/ReportImage`, {
            headers: headers(token, null),
            responseType: "blob", // required for axios to understand response
        });
    }
    catch (ex) {
        console.error(ex);
        throw ex;
    }
    if (response.status >= 200 && response.status < 300) {
        return URL.createObjectURL(response.data);
    }
    else {
        handleErrorResponse(response);
    }
};
