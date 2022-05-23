import React from "react";
import { Card, CardBody } from "reactstrap";

import { useActivityFeedEntities } from "../../api-hooks/activityFeedApi";
import { Typography, Spinner } from "../molecules";
import { RecommendationRow } from "../recommendations/RecommendationRow";
import { EventRow } from "../events/EventRow";

const ActivityFeed = () => {
  const activityFeedEntities = useActivityFeedEntities();

  return (
    <React.Fragment>
      <Typography variant="h4" style={{ fontFamily: "Prelia" }}>
        Activity Feed
      </Typography>
      <div className="row">
        {activityFeedEntities.loading ? (
          <div className="col">
            <Spinner />
          </div>
        ) : (
          <React.Fragment>
            <div className="col-6">
              <Card className="shadow-sm mt-4">
                <CardBody>
                  <Typography variant="h6" className="semi-bold pb-3">
                    Latest Events
                  </Typography>
                  {activityFeedEntities[0].activityKind === "event" &&
                  activityFeedEntities[0].activityItems.items.length > 0
                    ? activityFeedEntities[0].activityItems.items.map(
                        (event, index) => <EventRow key={index} event={event} />
                      )
                    : null}
                </CardBody>
              </Card>
            </div>
            <div className="col-6">
              <Card className="shadow-sm mt-4">
                <CardBody>
                  <Typography variant="h6" className="semi-bold pb-3">
                    Latest Recommendations
                  </Typography>
                  {activityFeedEntities[1].activityKind === "recommendation" &&
                  activityFeedEntities[1].activityItems.items.length > 0
                    ? activityFeedEntities[1].activityItems.items.map(
                        (recommendation, index) => (
                          <RecommendationRow
                            key={index}
                            recommendation={recommendation}
                          />
                        )
                      )
                    : null}
                </CardBody>
              </Card>
            </div>
          </React.Fragment>
        )}
      </div>
    </React.Fragment>
  );
};

export default ActivityFeed;
