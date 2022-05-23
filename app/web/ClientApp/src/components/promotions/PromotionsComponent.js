import React from "react";
import { Redirect, Route } from "react-router";
import { Switch, useRouteMatch } from "react-router-dom";

import AuthorizeRoute from "../auth0/ProtectedRoute";
import { RecommendableItemsSummary } from "./PromotionsSummary";
import { CreateItem } from "./CreatePromotion";
import { ItemDetail } from "./PromotionDetail";
import { ErrorBoundary } from "../molecules/ErrorBoundary";

const DefaultComponent = () => {
  const { path } = useRouteMatch();

  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            exact
            path={`${path}`}
            component={RecommendableItemsSummary}
          />
          <AuthorizeRoute path={`${path}/detail/:id`} component={ItemDetail} />
          <AuthorizeRoute
            exact
            path={`${path}/create`}
            component={CreateItem}
          />
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};

const PromotionsComponent = () => {
  const { path } = useRouteMatch();

  return (
    <React.Fragment>
      <ErrorBoundary>
        <Switch>
          <AuthorizeRoute
            path={`${path}/promotions`}
            component={DefaultComponent}
          />
          <Route path={path}>
            <Redirect to={`${path}/promotions`} />
          </Route>
        </Switch>
      </ErrorBoundary>
    </React.Fragment>
  );
};

export default PromotionsComponent;
