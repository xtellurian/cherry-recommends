import React from "react";
import { Typography, ErrorCard, AsyncButton } from "../../molecules";
import { BigPopup } from "../../molecules/popups/BigPopup";
import AsyncSelectSegment from "../../molecules/selectors/AsyncSelectSegment";
import { useAccessToken } from "../../../api-hooks/token";
import { useAnalytics } from "../../../analytics/analyticsHooks";
import { addPromotionOptimiserSegmentAsync } from "../../../api/promotionsCampaignsApi";

export const AddOptimiserSegmentPopup = ({
  isOpen,
  setIsOpen,
  recommender,
  onAdded,
}) => {
  const [selectedItem, setSelectedItem] = React.useState();
  const token = useAccessToken();
  const { analytics } = useAnalytics();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);

  const handleAdd = () => {
    setError(null);
    setLoading(true);
    addPromotionOptimiserSegmentAsync({
      token,
      id: recommender.id,
      segmentId: selectedItem.id,
    })
      .then((r) => {
        analytics.track("site:promotionsCampaign_addDistribution_success");
        onAdded(r);
        setSelectedItem(null);
        setIsOpen(false);
      })
      .catch((e) => {
        analytics.track("site:promotionsCampaign_addDistribution_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };

  React.useEffect(() => {
    if (!isOpen) {
      setError(null);
      setSelectedItem(null);
    }
  }, [isOpen]);

  return (
    <React.Fragment>
      <BigPopup isOpen={isOpen} setIsOpen={setIsOpen}>
        <Typography variant="h6" className="semi-bold border-bottom pb-2">
          Add Segment
        </Typography>
        {error && <ErrorCard error={error} />}
        <div style={{ minHeight: "100px" }}>
          <AsyncSelectSegment
            label="Segment"
            isMulti={false}
            onChange={(v) => {
              setError(null);
              setSelectedItem(v.value);
            }}
          />
        </div>
        <AsyncButton
          loading={loading}
          className="btn btn-block btn-primary"
          disabled={!selectedItem}
          onClick={handleAdd}
        >
          Add
        </AsyncButton>
      </BigPopup>
    </React.Fragment>
  );
};
