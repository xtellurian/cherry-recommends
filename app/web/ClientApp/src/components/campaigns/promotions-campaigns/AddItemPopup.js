import React from "react";
import { useAnalytics } from "../../../analytics/analyticsHooks";
import { addPromotionAsync } from "../../../api/promotionsCampaignsApi";
import { AsyncButton, ErrorCard, Subtitle, Title } from "../../molecules";
import { BigPopup } from "../../molecules/popups/BigPopup";
import AsyncSelectItem from "../../molecules/selectors/AsyncSelectPromotion";
import { PromotionRow } from "../../promotions/PromotionRow";
import { useAccessToken } from "../../../api-hooks/token";
export const AddItemPopup = ({ isOpen, setIsOpen, recommender, onAdded }) => {
  const token = useAccessToken();
  const { analytics } = useAnalytics();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const [selectedItem, setSelectedItem] = React.useState();
  const handleAdd = () => {
    setError(null);
    setLoading(true);
    addPromotionAsync({
      token,
      id: recommender.id,
      promotion: selectedItem,
    })
      .then((r) => {
        analytics.track("site:itemsCampaign_addItem_success");
        onAdded(r);
        setSelectedItem(null);
        setIsOpen(false);
      })
      .catch((e) => {
        analytics.track("site:itemsCampaign_addItem_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };
  return (
    <React.Fragment>
      <BigPopup
        isOpen={isOpen}
        setIsOpen={setIsOpen}
        buttons={
          <AsyncButton
            loading={loading}
            className="btn btn-primary w-50 float-right"
            onClick={handleAdd}
          >
            Add
          </AsyncButton>
        }
        header={
          recommender.name
            ? `Add Promotion to ${recommender.name}`
            : "Add Promotion"
        }
        headerDivider
      >
        {error && <ErrorCard error={error} />}
        <div style={{ minHeight: "30vh" }}>
          <AsyncSelectItem
            isMulti={false}
            onChange={(v) => setSelectedItem(v.value)}
          />

          <div className="mt-2">
            {selectedItem && <PromotionRow promotion={selectedItem} />}
          </div>
        </div>
      </BigPopup>
    </React.Fragment>
  );
};
