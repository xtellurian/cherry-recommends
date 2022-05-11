import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { RecommendableItemsSummary } from "./PromotionsSummary";
import { CreateItem } from "./CreatePromotion";
import { ItemDetail } from "./PromotionDetail";
import { ErrorBoundary } from "../molecules/ErrorBoundary";

const PromotionsComponent = () => {
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

export default PromotionsComponent;
