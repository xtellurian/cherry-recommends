import React from "react";
import { useRouteMatch, Switch } from "react-router-dom";
import AuthorizeRoute from "../auth0/ProtectedRoute";
import { Title } from "../molecules/PageHeadings";
import { Tabs, TabActivator } from "../molecules/Tabs";
import { ViewEventData } from "./ViewEventData";

const tabs = [
  {
    id: "events",
    name: "Event Data",
  },
];
const defaultTab = "events";
const DataViewHome = () => {
  return (
    <div>
      <Title>Data View</Title>
      <Tabs tabs={tabs} />
      <hr />
      <TabActivator tabId="events" defaultTab={defaultTab}>
        <ViewEventData />
      </TabActivator>
    </div>
  );
};

export const DataViewComponent = () => {
  const { path } = useRouteMatch();

  return (
    <React.Fragment>
      <Switch>
        <AuthorizeRoute exact path={`${path}`} component={DataViewHome} />
        <AuthorizeRoute
          exact
          path={`${path}/events`}
          component={ViewEventData}
        />
      </Switch>
    </React.Fragment>
  );
};
