import React from "react";
import { usePromotionsCampaigns } from "../../api-hooks/promotionsCampaignsApi";
import { useParameterSetCampaigns } from "../../api-hooks/parameterSetCampaignsApi";
import { EmptyState, Navigation, Spinner } from "../molecules";
import { EmptyStateText } from "../molecules/empty/EmptyStateText";
import { CampaignRow } from "../campaigns/CampaignRow";
import { CardSection, Label, MoreLink } from "../molecules/layout/CardSection";

const MAX_LIST_LENGTH = 5;
export const Campaigns = ({ className, hasItems }) => {
  const itemsRecommenders = usePromotionsCampaigns();
  const parameterRecommenders = useParameterSetCampaigns();
  const [allRecommenders, setAllRecommenders] = React.useState([]);
  const loading = itemsRecommenders.loading || parameterRecommenders.loading;
  React.useEffect(() => {
    if (itemsRecommenders.items && parameterRecommenders.items) {
      for (const itemRec of itemsRecommenders.items) {
        itemRec.recommenderSubPath = "promotions-campaigns";
        itemRec.uniqueId = `items-${itemRec.id}`;
      }

      for (const parameterRec of parameterRecommenders.items) {
        parameterRec.recommenderSubPath = "parameter-set-campaigns";
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
          <Label>Campaigns</Label>
          {loading && <Spinner />}
          {allRecommenders.map((r) => (
            <CampaignRow key={r.uniqueId} recommender={r} />
          ))}

          {!loading && allRecommenders.length === 0 && (
            <EmptyState>
              <EmptyStateText>
                You haven't created any campaigns.
              </EmptyStateText>
              <Navigation
                to={{
                  pathname: "/campaigns/promotions-campaigns/create",
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
              pathname: "/campaigns/promotions-campaigns",
            }}
          >
            View More
          </MoreLink>
        </CardSection>
      </div>
    </>
  );
};
