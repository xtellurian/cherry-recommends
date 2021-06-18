import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ListIntegrations } from "./ListIntegrations";
import { CreateIntegration } from "./CreateIntegration";
import { IntegratedSystemDetail } from "./IntegratedSystemDetail";
import { CreateWebhookReceiver } from "./CreateWebhookReceiver";
import { HubspotConnector } from "./hubspot/HubspotConnector";
import { TestHubspotConnection } from "./hubspot/TestHubspotConnection";

export const IntegrationsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={ListIntegrations} />
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
          exact
          path={`${path}/testhubspotconnection/:id`}
          component={TestHubspotConnection}
        />
      </Switch>
    </React.Fragment>
  );
};
