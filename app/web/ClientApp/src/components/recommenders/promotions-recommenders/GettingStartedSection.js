import React from "react";
import {
  useDestinations,
  usePromotionsRecommendations,
} from "../../../api-hooks/promotionsRecommendersApi";
import { Navigation } from "../../molecules";
import { NoteBox } from "../../molecules/NoteBox";

export const GettingStartedSection = ({ recommender }) => {
  const recommendations = usePromotionsRecommendations({ id: recommender.id });
  const destinations = useDestinations({ id: recommender.id });

  const noRecommendations =
    recommendations.pagination &&
    recommendations.pagination.totalItemCount === 0;

  const noDestinations = destinations && destinations.length === 0;
  return (
    <>
      {!recommendations.loading && (
        <div>
          {noRecommendations && noDestinations && (
            <div>
              <NoteBox label="Use recommendations">
                <p>
                  To use this campaign, you can send recommendations to an
                  Integrated System via a channel.
                </p>
                <Navigation
                  to={`/recommenders/promotions-recommenders/delivery/${recommender.id}?tab=delivery`}
                >
                  <button className="btn btn-outline-primary">
                    Add a destination channel
                  </button>
                </Navigation>
              </NoteBox>
            </div>
          )}
        </div>
      )}
    </>
  );
};
