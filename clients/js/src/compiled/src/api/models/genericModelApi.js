import { executeFetch } from "../client/apiClient";
export const invokeGenericModelAsync = async ({ token, id, input }) => {
    return await executeFetch({
        path: `api/models/generic/${id}/invoke`,
        body: input,
        method: "post",
        token,
    });
};
