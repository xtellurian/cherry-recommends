import React from "react";
import { useDashboard, useLatestActions } from "../../api-hooks/dataSummaryApi";
import {
  Subtitle,
  ExpandableCard,
  Spinner,
  EmptyList,
  ErrorCard,
} from "../molecules";
import { DateTimeField } from "../molecules/DateTimeField";
import { JsonView } from "../molecules/JsonView";
import { DashboardEventChart } from "./DashboardEventChart";
import { GetStarted } from "./GetStarted";

const ActionRow = ({ action }) => {
  const ts = new Date(action.timestamp);
  return (
    <ExpandableCard label={`${action.category}  @  ${ts.toLocaleDateString()}`}>
      <DateTimeField label="Timestamp" date={action.timestamp} />
      <JsonView data={action} />
    </ExpandableCard>
  );
};
export const Dashboard = () => {
  const dashboard = useDashboard({ scope: null }); // choose null, kind, or type
  const latestActions = useLatestActions();
  return (
    <React.Fragment>
      <Subtitle>Get Started</Subtitle>
      <GetStarted />
      <hr />
      <Subtitle>Recent Aggregated Events</Subtitle>
      {dashboard.loading && <Spinner>Loading Dashboard Data</Spinner>}
      {dashboard.eventTimeline && (
        <DashboardEventChart timeline={dashboard.eventTimeline} />
      )}
      <hr />
      <Subtitle>Recent Activity</Subtitle>
      {latestActions.loading && <Spinner> Loading Latest Actions </Spinner>}
      {latestActions.error && <ErrorCard error={latestActions.error} />}
      {latestActions.items &&
        latestActions.items.map((a) => <ActionRow key={a.id} action={a} />)}
      {latestActions.items && latestActions.items.length === 0 && (
        <EmptyList>No Recent Activity</EmptyList>
      )}
    </React.Fragment>
  );
};
