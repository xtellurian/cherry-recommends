import { getUrl } from "../baseUrl";

const defaultHeaders = { "Content-Type": "application/json" };

export const fetchReports = async ({ success, error, token }) => {
  const url = getUrl("api/reports");
  const response = await fetch(url, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const results = await response.json();
    success(results);
  } else {
    error(await response.json());
  }
};

export const downloadReport = async ({ success, error, token, reportName }) => {
  const url = getUrl("api/reports/download");
  let path = `${url}?report=${reportName}`;

  const response = await fetch(path, {
    headers: !token
      ? defaultHeaders
      : { ...defaultHeaders, Authorization: `Bearer ${token}` },
  });
  if (response.ok) {
    const results = await response.blob();
    success(results);
  } else {
    error(await response.json());
  }
};
