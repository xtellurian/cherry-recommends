import { headers } from "../../client/headers";
import { handleErrorResponse, handleErrorFetch, } from "../../../utilities/errorHandling";
export const fetchReportImageBlobUrlAsync = async ({ recommenderApiName, token, id, useInternalId, }) => {
    console.log("fetching image for recommender");
    console.log(`api/recommenders/${recommenderApiName}/${id}/ReportImage`);
    let response;
    try {
        response = await fetch(`api/recommenders/${recommenderApiName}/${id}/ReportImage`, {
            headers: headers(token),
            method: "get",
        });
    }
    catch (ex) {
        return handleErrorFetch(ex);
    }
    if (response.ok) {
        const blob = await response.blob();
        return URL.createObjectURL(blob);
    }
    else {
        handleErrorResponse(response);
    }
};
