import React from "react";
import { useDashboard } from "../../api-hooks/dataSummaryApi";
import { Subtitle } from "../molecules/PageHeadings";
import { Spinner } from "../molecules/Spinner";
import { DashboardEventChart } from "./DashboardEventChart";
export const Dashboard = () => {
  const { result } = useDashboard();

  if (!result || result.loading) {
    return <Spinner />;
  }
  return (
    <React.Fragment>
      <Subtitle>Latest Tracked Events</Subtitle>
      <DashboardEventChart timeline={result.eventTimeline} />
    </React.Fragment>
  );
};
