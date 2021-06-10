import React from "react";
import { Switch, Link, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ApiKeyComponent } from "./apiKeys/ApiKeyComponent";
import { IntegrationsComponent } from "./integrations/IntegrationsComponent";
import { DeploymentInfo } from "./deployment/DeploymentInfo";

const SettingsHome = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <div>
        <Link to={`${path}/api-keys`}>
          <button className="btn btn-outline-primary">API Keys</button>
        </Link>
      </div>
      <div>
        <Link to={`${path}/integrations`}>
          <button className="btn btn-outline-primary">Integrations</button>
        </Link>
      </div>
    </React.Fragment>
  );
};

export const SettingsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={SettingsHome} />
        <AuthorizeRoute path={`${path}/api-keys`} component={ApiKeyComponent} />
        <AuthorizeRoute
          path={`${path}/integrations`}
          component={IntegrationsComponent}
        />
        <AuthorizeRoute
          path={`${path}/integrations`}
          component={IntegrationsComponent}
        />
        <AuthorizeRoute
          path={`${path}/info`}
          component={DeploymentInfo}
        />
      </Switch>
    </React.Fragment>
  );
};
