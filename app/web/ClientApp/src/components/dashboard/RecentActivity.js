import React from "react";
import { useDashboard } from "../../api-hooks/dataSummaryApi";
import { Spinner, EmptyList, ErrorCard } from "../molecules";
import { NoteBox } from "../molecules/NoteBox";
import { EventRow } from "../events/EventRow";
import { RecommendationRow } from "../recommendations/RecommendationRow";

export const RecentActivity = () => {
  const dashboard = useDashboard({ scope: null }); // choose null, kind, or type
  return (
    <React.Fragment>
      <NoteBox label="Activity">
        {dashboard.loading ? <Spinner /> : null}
        {dashboard.error ? <ErrorCard error={dashboard.error} /> : null}
        <div className="row">
          {dashboard.events && (
            <div className="col">
              <h6>Latest Events</h6>
              {dashboard.events &&
                dashboard.events.map((e) => <EventRow key={e.id} event={e} />)}
              {dashboard.events && dashboard.events.length === 0 && (
                <EmptyList>No Recent Events</EmptyList>
              )}
            </div>
          )}
          {dashboard.events && (
            <div className="col">
              <h6>Latest Recommendations</h6>
              {dashboard.recommendations &&
                dashboard.recommendations.map((r, i) => (
                  <RecommendationRow key={i} recommendation={r} />
                ))}
              {dashboard.recommendations &&
                dashboard.recommendations.length === 0 && (
                  <EmptyList>No Recent Activity</EmptyList>
                )}
            </div>
          )}
        </div>
      </NoteBox>
    </React.Fragment>
  );
};
