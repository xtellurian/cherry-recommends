import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { RecommendableItemsSummary } from "./PromotionsSummary";
import { CreateItem } from "./CreatePromotion";
import { ItemDetail } from "./PromotionDetail";

const PromotionsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute
          exact
          path={`${path}`}
          component={RecommendableItemsSummary}
        />
        <AuthorizeRoute path={`${path}/detail/:id`} component={ItemDetail} />
        <AuthorizeRoute exact path={`${path}/create`} component={CreateItem} />
      </Switch>
    </React.Fragment>
  );
};

export default PromotionsComponent;
