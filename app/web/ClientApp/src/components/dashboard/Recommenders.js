import React from "react";
import { usePromotionsRecommenders } from "../../api-hooks/promotionsRecommendersApi";
import { useParameterSetRecommenders } from "../../api-hooks/parameterSetRecommendersApi";
import { EmptyState, Navigation, Spinner } from "../molecules";
import { EmptyStateText } from "../molecules/empty/EmptyStateText";
import { RecommenderRow } from "../recommenders/RecommenderRow";
import { CardSection, Label, MoreLink } from "../molecules/layout/CardSection";

const MAX_LIST_LENGTH = 5;
export const Recommenders = ({ className, hasItems }) => {
  const itemsRecommenders = usePromotionsRecommenders();
  const parameterRecommenders = useParameterSetRecommenders();
  const [allRecommenders, setAllRecommenders] = React.useState([]);
  const loading = itemsRecommenders.loading || parameterRecommenders.loading;
  React.useEffect(() => {
    if (itemsRecommenders.items && parameterRecommenders.items) {
      for (const itemRec of itemsRecommenders.items) {
        itemRec.recommenderSubPath = "promotions-recommenders";
        itemRec.uniqueId = `items-${itemRec.id}`;
      }

      for (const parameterRec of parameterRecommenders.items) {
        parameterRec.recommenderSubPath = "parameter-set-recommenders";
        parameterRec.uniqueId = `parameter-${parameterRec.id}`;
      }

      let allRecs = [
        ...itemsRecommenders.items,
        ...parameterRecommenders.items,
      ];
      if (allRecs && allRecs.length > MAX_LIST_LENGTH) {
        allRecs = allRecs.slice(1, MAX_LIST_LENGTH + 1);
      }

      setAllRecommenders(allRecs);
    }
  }, [itemsRecommenders, parameterRecommenders]);
  return (
    <>
      <div className={className}>
        <CardSection className="p-4">
          <Label>Campaignss</Label>
          {loading && <Spinner />}
          {allRecommenders.map((r) => (
            <RecommenderRow key={r.uniqueId} recommender={r} />
          ))}

          {!loading && allRecommenders.length === 0 && (
            <EmptyState>
              <EmptyStateText>
                You haven't created any campaigns.
              </EmptyStateText>
              <Navigation
                to={{
                  pathname: "/recommenders/promotions-recommenders/create",
                }}
              >
                <button disabled={!hasItems} className="btn btn-primary">
                  Create a Campaign
                </button>
              </Navigation>
            </EmptyState>
          )}
          <MoreLink
            to={{
              pathname: "/recommenders/promotions-recommenders",
            }}
          >
            View More
          </MoreLink>
        </CardSection>
      </div>
    </>
  );
};
