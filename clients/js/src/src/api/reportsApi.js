import { executeFetch } from "./client/apiClient";

export const fetchReportsAsync = async ({ token }) => {
  return await executeFetch({
    path: "api/Reports",
    token,
  });
};

export const downloadReportAsync = async ({ token, reportName }) => {
  return await executeFetch({
    path: "api/reports/download",
    token,
    query: {
      report: reportName,
    },
  });
};
