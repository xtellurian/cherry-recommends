import { executeFetch } from "./client/apiClient";
export const fetchDeploymentConfigurationAsync = async ({ token }) => {
    return await executeFetch({
        path: "api/deployment/configuration",
        token,
    });
};
