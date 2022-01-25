import { executeFetch } from "../../client/apiClient";
export const setArgumentsAsync = async ({ recommenderApiName, token, id, args, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/Arguments`,
        token,
        method: "post",
        body: args,
    });
};