import React from "react";
import Modal from "react-modal";
import { useAnalytics } from "../../../analytics/analyticsHooks";
import { closeButton, small } from "../../molecules/popups/styles";
import { createRecommendationConsumedEventAsync } from "../../../api/eventsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { ErrorCard } from "../../molecules";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";

export const ConsumeRecommendationPopup = ({
  isOpen,
  setIsOpen,
  recommendation,
  onConsumed,
}) => {
  const token = useAccessToken();
  const { analytics } = useAnalytics();
  const [error, setError] = React.useState();

  if (!recommendation) {
    throw new Error("recommendation must not be undefinated or null");
  }
  const handleConsume = () => {
    createRecommendationConsumedEventAsync({
      token,
      customerId: recommendation.customerId,
      correlatorId: recommendation.correlatorId,
    })
      .then(() => {
        analytics.track("site:recommender_consume_success");
        setIsOpen(false);
        if (onConsumed) {
          onConsumed();
        }
      })
      .catch((e) => {
        analytics.track("site:recommender_consume_failure");
        setError(e);
      });
  };

  const onRequestClose = () => setIsOpen(false);

  return (
    <React.Fragment>
      <Modal
        isOpen={isOpen}
        onRequestClose={onRequestClose}
        style={small}
        contentLabel="Consume Recommendation"
      >
        <button
          className="btn btn-link"
          onClick={onRequestClose}
          style={closeButton}
        >
          <FontAwesomeIcon icon={faCircleXmark} />
        </button>
        {error && <ErrorCard error={error} />}
        <div className="m-3">Consume this recommendation?</div>

        <div
          className="btn-group mt-1 w-100"
          role="group"
          aria-label="Delete or rename buttons"
        >
          <button
            className="btn btn-secondary"
            onClick={() => setIsOpen(false)}
          >
            Cancel
          </button>
          <button className="btn btn-success" onClick={handleConsume}>
            Confirm
          </button>
        </div>
      </Modal>
    </React.Fragment>
  );
};
