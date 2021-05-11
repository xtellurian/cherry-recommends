import React from "react";
import { Switch, useRouteMatch } from "react-router-dom";
import AuthorizeRoute from "../api-authorization/AuthorizeRoute";
import { BeerDemoComponent } from "./beer/BeerDemoComponent";
import { ShampooCancelationComponent } from "./shampoo-cancelation/ShampooCancelationComponent"
import { SoftwareDemoComponent } from "./software/SoftwareDemoComponent";

export const DemoComponent = () => {
  let { path } = useRouteMatch();

  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute path={`${path}/beer`} component={BeerDemoComponent} />
        <AuthorizeRoute path={`${path}/shampoo`} component={ShampooCancelationComponent} />
        <AuthorizeRoute path={`${path}/software`} component={SoftwareDemoComponent} />
      </Switch>
    </React.Fragment>
  );
};
