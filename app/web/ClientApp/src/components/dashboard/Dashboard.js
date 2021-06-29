import React from "react";
import { useDashboard } from "../../api-hooks/dataSummaryApi";
import { Subtitle } from "../molecules/PageHeadings";
import { Spinner } from "../molecules/Spinner";
import { DashboardEventChart } from "./DashboardEventChart";
import { GetStarted } from "./GetStarted";

export const Dashboard = () => {
  const dashboard = useDashboard({scope: null}); // choose null, kind, or type

  return (
    <React.Fragment>
      <Subtitle>Get Started</Subtitle>
      <GetStarted />
      <hr />
      <Subtitle>Latest Tracked Events</Subtitle>
      {dashboard.loading && <Spinner>Loading Dashboard Data</Spinner>}
      {dashboard.eventTimeline && (
        <DashboardEventChart timeline={dashboard.eventTimeline} />
      )}
    </React.Fragment>
  );
};
