import React from "react";
import { useAnalytics } from "../../analytics/analyticsHooks";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { useAccessToken } from "../../api-hooks/token";
import { useReports } from "../../api-hooks/reportsApi";
import { downloadReportAsync } from "../../api/reportsApi";
import { Title } from "../molecules/layout";
import { Spinner } from "../molecules/Spinner";
import { ErrorBoundary } from "../molecules/ErrorBoundary";

const saveBlob = ({ blob, name }) => {
  var a = document.createElement("a");
  document.body.appendChild(a);
  a.style = "display: none";

  window.URL.createObjectURL(blob);
  a.download = name;
  const url = window.URL.createObjectURL(blob);
  a.href = url;
  a.click();
  window.URL.revokeObjectURL(url);
};

const ReportRow = ({ reportInfo }) => {
  const token = useAccessToken();
  const { analytics } = useAnalytics();
  return (
    <div className="card">
      <div className="card-body text-center">
        <button
          className="btn btn-primary float-right"
          onClick={() => {
            downloadReportAsync({
              token,
              reportName: reportInfo.name,
            })
              .then((blob) => {
                analytics.track("site:report_download_success", {
                  name: reportInfo.name,
                });
                saveBlob({ blob, name: reportInfo.name });
              })
              .catch((e) => {
                analytics.track("site:report_download_failure");
                alert(JSON.stringify(e));
              });
          }}
        >
          Download
        </button>
        <h5>{reportInfo.name}</h5>
      </div>
    </div>
  );
};
const ReportsHome = () => {
  const { result } = useReports();

  if (!result || result.loading) {
    return (
      <div>
        <Title>Reports</Title>
        <hr />
        <Spinner />
      </div>
    );
  }

  return (
    <div>
      <Title>Reports</Title>
      <hr />
      {result.map((r) => (
        <ReportRow key={r.name} reportInfo={r} />
      ))}
    </div>
  );
};

export const ReportsComponent = () => {
  let { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute exact path={`${path}`} component={ReportsHome} />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
