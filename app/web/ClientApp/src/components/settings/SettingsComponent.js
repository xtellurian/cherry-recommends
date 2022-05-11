import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ApiKeyComponent } from "./apiKeys/ApiKeyComponent";
import { IntegrationsComponent } from "./integrations/IntegrationsComponent";
import { DeploymentInfo } from "./deployment/DeploymentInfo";
import { EnvironmentsComponent } from "./environments/EnvironmentsComponent";
import { Navigation } from "../molecules";
import { ErrorBoundary } from "../molecules/ErrorBoundary";

const SettingsHome = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <div>
        <Navigation to={`${path}/api-keys`}>
          <button className="btn btn-outline-primary">API Keys</button>
        </Navigation>
      </div>
      <div>
        <Navigation to={`${path}/integrations`}>
          <button className="btn btn-outline-primary">Integrations</button>
        </Navigation>
      </div>
      <div>
        <Navigation to={`${path}/environments`}>
          <button className="btn btn-outline-primary">Environments</button>
        </Navigation>
      </div>
    </React.Fragment>
  );
};

export const SettingsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute exact path={`${path}`} component={SettingsHome} />
          <AuthorizeRoute
            path={`${path}/api-keys`}
            component={ApiKeyComponent}
          />
          <AuthorizeRoute
            path={`${path}/integrations`}
            component={IntegrationsComponent}
          />
          <AuthorizeRoute path={`${path}/info`} component={DeploymentInfo} />
          <AuthorizeRoute
            path={`${path}/environments`}
            component={EnvironmentsComponent}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
