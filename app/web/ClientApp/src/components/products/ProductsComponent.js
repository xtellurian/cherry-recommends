import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { ProductSummary } from "./ProductSummary";
import { CreateProduct } from "./CreateProduct";
import { ProductDetail } from "./ProductDetail";

export const ProductsComponent = () => {
  const { path } = useRouteMatch();
  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={ProductSummary} />
        <AuthorizeRoute path={`${path}/detail/:id`} component={ProductDetail} />
        <AuthorizeRoute
          exact
          path={`${path}/create`}
          component={CreateProduct}
        />
      </Switch>
    </React.Fragment>
  );
};
