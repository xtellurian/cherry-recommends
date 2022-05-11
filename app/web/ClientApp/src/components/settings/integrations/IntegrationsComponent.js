import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ListConnections, ListDataSources } from "./ListIntegrations";
import { CreateIntegration } from "./CreateIntegration";
import { IntegratedSystemDetail } from "./IntegratedSystemDetail";
import { CreateWebhookReceiver } from "./CreateWebhookReceiver";
import { HubspotConnector } from "./hubspot/HubspotConnector";
import { HubspotIntegrationComponent } from "./hubspot/HubspotIntegrationComponent";
import { CustomIntegrationComponent } from "./custom/CustomIntegrationComponent";
import { ErrorBoundary } from "../../molecules/ErrorBoundary";

export const IntegrationsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute exact path={`${path}`} component={ListConnections} />
          <AuthorizeRoute
            exact
            path={`${path}/data-sources`}
            component={ListDataSources}
          />
          <AuthorizeRoute
            exact
            path={`${path}/create`}
            component={CreateIntegration}
          />
          <AuthorizeRoute
            exact
            path={`${path}/detail/:id`}
            component={IntegratedSystemDetail}
          />

          <AuthorizeRoute
            exact
            path={`${path}/create-webhook-receiver/:id`}
            component={CreateWebhookReceiver}
          />
          <AuthorizeRoute
            exact
            path={`${path}/hubspotconnector`}
            component={HubspotConnector}
          />
          <AuthorizeRoute
            path={`${path}/hubspot-detail/:id`}
            component={HubspotIntegrationComponent}
          />
          <AuthorizeRoute
            path={`${path}/custom/:id`}
            component={CustomIntegrationComponent}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};
