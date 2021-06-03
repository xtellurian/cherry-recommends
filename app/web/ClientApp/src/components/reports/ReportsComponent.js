import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { useAccessToken } from "../../api-hooks/token";
import { useReports } from "../../api-hooks/reportsApi";
import { downloadReport } from "../../api/reportsApi";
import { Title } from "../molecules/PageHeadings";
import { Spinner } from "../molecules/Spinner";

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
  return (
    <div className="card">
      <div className="card-body text-center">
        <button
          className="btn btn-primary float-right"
          onClick={() =>
            downloadReport({
              token,
              success: (blob) => saveBlob({ blob, name: reportInfo.name }),
              error: (e) => alert(JSON.stringify(e)),
              reportName: reportInfo.name,
            })
          }
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
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={ReportsHome} />
      </Switch>
    </React.Fragment>
  );
};
