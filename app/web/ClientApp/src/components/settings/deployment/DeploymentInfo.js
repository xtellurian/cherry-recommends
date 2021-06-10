import React from "react";
import { useDeploymentConfiguration } from "../../../api-hooks/deploymentApi";
import { Title } from "../../molecules/PageHeadings";
import { Spinner } from "../../molecules/Spinner";
export const DeploymentInfo = () => {
  const info = useDeploymentConfiguration();
  if (!info || info.loading) {
    return <Spinner />;
  } else {
    return (
      <React.Fragment>
        <Title>Deployment Information</Title>
        <hr />
        <div className="row">
          <div className="col">Stack</div>
          <div className="col">{info.stack}</div>
        </div>
        <div className="row">
          <div className="col">Project</div>
          <div className="col">{info.project}</div>
        </div>
        <div className="row">
          <div className="col">Environment</div>
          <div className="col">{info.environment}</div>
        </div>
      </React.Fragment>
    );
  }
};
