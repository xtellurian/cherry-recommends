import React from "react";
import { Link } from "react-router-dom";
import {
  useDestinations,
  useItemsRecommendations,
} from "../../../api-hooks/itemsRecommendersApi";
import { NoteBox } from "../../molecules/NoteBox";

export const GettingStartedSection = ({ recommender }) => {
  const recommendations = useItemsRecommendations({ id: recommender.id });
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
                  To use this recommender, you can send recommendations to an
                  Integrated System, or by using the Javascript SDK.
                </p>
                <Link
                  to={`/recommenders/promotions-recommenders/destinations/${recommender.id}`}
                >
                  <button className="btn btn-outline-primary">
                    Add a destination
                  </button>
                </Link>
              </NoteBox>
            </div>
          )}
        </div>
      )}
    </>
  );
};
