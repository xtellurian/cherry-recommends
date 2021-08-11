import React from "react";
import { Link } from "react-router-dom";
import { useDashboard } from "../../api-hooks/dataSummaryApi";
import {
  Subtitle,
  ExpandableCard,
  Spinner,
  EmptyList,
  ErrorCard,
} from "../molecules";
import { DateTimeField } from "../molecules/DateTimeField";
import { JsonView } from "../molecules/JsonView";
import { GetStarted } from "./GetStarted";

// const ActionRow = ({ action }) => {
//   const ts = new Date(action.timestamp);
//   return (
//     <ExpandableCard label={`${action.category}  @  ${ts.toLocaleDateString()}`}>
//       <DateTimeField label="Timestamp" date={action.timestamp} />
//       <JsonView data={action} />
//     </ExpandableCard>
//   );
// };
const EventRow = ({ event }) => {
  const ts = new Date(event.timestamp);
  return (
    <ExpandableCard label={`${event.kind}  @  ${ts.toLocaleDateString()}`}>
      <Link to={`/events/detail/${event.eventId}`}>
        <button className="btn btn-primary float-right">Detail</button>
      </Link>
      <DateTimeField label="Timestamp" date={event.timestamp} />
      <JsonView data={event} />
    </ExpandableCard>
  );
};
const RecommendationRow = ({ recommendation }) => {
  const ts = new Date(recommendation.created);
  try {
    const modelInput = JSON.parse(recommendation.modelInput);
    const modelOutput = JSON.parse(recommendation.modelOutput);
    return (
      <ExpandableCard
        label={`${
          recommendation.recommenderType || ""
        }  @  ${ts.toLocaleDateString()}`}
      >
        <DateTimeField label="Timestamp" date={recommendation.created} />
        Model Input
        <JsonView data={modelInput} />
        Model Output
        <JsonView data={modelOutput} />
      </ExpandableCard>
    );
  } catch (ex) {
    return (
      <ExpandableCard label="Oops">
        An error occurred when parsing model input or output.
      </ExpandableCard>
    );
  }
};
export const Dashboard = () => {
  const dashboard = useDashboard({ scope: null }); // choose null, kind, or type
  return (
    <React.Fragment>
      <Subtitle>Get Started</Subtitle>
      <GetStarted />
      <hr />
      <Subtitle>Recent Activity</Subtitle>
      {dashboard.loading && <Spinner />}
      {dashboard.error && <ErrorCard error={dashboard.error} />}
      <div className="row">
        {dashboard.events && (
          <div className="col">
            <h6>Events</h6>
            {dashboard.events &&
              dashboard.events.map((e) => <EventRow key={e.id} event={e} />)}
            {dashboard.events && dashboard.events.length === 0 && (
              <EmptyList>No Recent Events</EmptyList>
            )}
          </div>
        )}
        {dashboard.events && (
          <div className="col">
            <h6>Recommendations</h6>
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
    </React.Fragment>
  );
};
