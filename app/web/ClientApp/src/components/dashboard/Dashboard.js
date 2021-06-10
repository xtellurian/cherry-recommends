import React from "react";
import { useDashboard } from "../../api-hooks/dataSummaryApi";
import { Subtitle } from "../molecules/PageHeadings";
import { Spinner } from "../molecules/Spinner";
import { DashboardEventChart } from "./DashboardEventChart";
import { GetStarted } from "./GetStarted";

export const Dashboard = () => {
  const { result } = useDashboard();

  if (!result || result.loading) {
    return <Spinner />;
  }
  return (
    <React.Fragment>
      <div className="row">
        <div className="col">
          <Subtitle>Latest Tracked Events</Subtitle>
          <DashboardEventChart timeline={result.eventTimeline} />
        </div>
        <div className="col">
          <Subtitle>Get Started</Subtitle>
          <GetStarted />
        </div>
      </div>
    </React.Fragment>
  );
};
