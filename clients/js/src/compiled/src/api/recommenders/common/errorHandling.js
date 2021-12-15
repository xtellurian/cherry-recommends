import { executeFetch } from "../../client/apiClient";
export const updateErrorHandlingAsync = async ({ recommenderApiName, token, id, errorHandling, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ErrorHandling`,
        token,
        method: "post",
        body: errorHandling,
    });
};
