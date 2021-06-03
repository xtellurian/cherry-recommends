const defaultHeaders = { "Content-Type": "application/json" };

export const fetchReports = async ({ success, error, token }) => {
  let path = `api/reports`;
  const response = await fetch(path, {
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
  let path = `api/reports/download?report=${reportName}`;

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