import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../../auth0/ProtectedRoute";
import { ListIntegrations } from "./ListIntegrations";
import { CreateIntegration } from "./CreateIntegration";
import { IntegratedSystemDetail } from "./IntegratedSystemDetail";
import { CreateWebhookReceiver } from "./CreateWebhookReceiver";
import { HubspotConnector } from "./hubspot/HubspotConnector";
import { HubspotIntegrationComponent } from "./hubspot/HubspotIntegrationComponent";
import { CustomIntegrationComponent } from "./custom/CustomIntegrationComponent";
import { ShopifyConnector } from "./shopify/ShopifyConnector";
import { useFeatureFlag } from "../../launch-darkly/hooks";

export const IntegrationsComponent = () => {
  const { path } = useRouteMatch();
  const shopifyFlag = useFeatureFlag("shopify", true);
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
          path={`${path}/hubspot-detail/:id`}
          component={HubspotIntegrationComponent}
        />
        {shopifyFlag && (
          <AuthorizeRoute
            exact
            path={`${path}/shopifyconnector`}
            component={ShopifyConnector}
          />
        )}
        <AuthorizeRoute
          path={`${path}/custom/:id`}
          component={CustomIntegrationComponent}
        />
      </Switch>
    </React.Fragment>
  );
};
