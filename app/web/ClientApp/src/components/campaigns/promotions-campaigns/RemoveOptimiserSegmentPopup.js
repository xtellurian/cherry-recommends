import React from "react";
import { ConfirmationPopup } from "../../molecules/popups/ConfirmationPopup";
import { ErrorCard } from "../../molecules/ErrorCard";
import { removePromotionOptimiserSegmentAsync } from "../../../api/promotionsCampaignsApi";
import { AsyncButton } from "../../molecules";
import { useAccessToken } from "../../../api-hooks/token";
import { useAnalytics } from "../../../analytics/analyticsHooks";

export const RemoveOptimiserSegmentPopup = ({
  segment,
  recommender,
  isOpen,
  setIsOpen,
  onRemoved,
}) => {
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const token = useAccessToken();
  const { analytics } = useAnalytics();

  const handleRemove = () => {
    removePromotionOptimiserSegmentAsync({
      token,
      id: recommender.id,
      segmentId: segment.id,
    })
      .then((r) => {
        analytics.track("site:promotionsCampaign_removeDistribution_success");
        onRemoved(r);
        setIsOpen(false);
      })
      .catch((e) => {
        analytics.track("site:promotionsCampaign_removeDistribution_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };

  if (segment && recommender) {
    const label = segment.name
      ? `Remove ${segment.name} from ${recommender.name}?`
      : "Loading...";
    return (
      <ConfirmationPopup isOpen={isOpen} setIsOpen={setIsOpen} label={label}>
        <div className="m-2">{`Remove ${segment.name} from ${recommender.name}?`}</div>
        {error && <ErrorCard error={error} />}
        <div
          className="btn-group"
          role="group"
          aria-label="Delete or cancel buttons"
        >
          <button
            className="btn btn-secondary"
            onClick={() => setIsOpen(false)}
          >
            Cancel
          </button>
          <AsyncButton
            loading={loading}
            className="btn btn-danger"
            onClick={handleRemove}
          >
            Remove
          </AsyncButton>
        </div>
      </ConfirmationPopup>
    );
  } else {
    return <React.Fragment></React.Fragment>;
  }
};
