import React from "react";
import Modal from "react-modal";
import { small } from "../../molecules/popups/styles";
import { createRecommendationConsumedEventAsync } from "../../../api/eventsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { ErrorCard } from "../../molecules";

export const ConsumeRecommendationPopup = ({
  isOpen,
  setIsOpen,
  recommendation,
  onConsumed,
}) => {
  const token = useAccessToken();
  const [error, setError] = React.useState();

  if (!recommendation) {
    throw new Error("recommendation must not be undefinated or null");
  }
  const handleConsume = () => {
    createRecommendationConsumedEventAsync({
      token,
      commonUserId: recommendation.commonUserId,
      correlatorId: recommendation.correlatorId,
    })
      .then(() => {
          setIsOpen(false);
          if (onConsumed) {
            onConsumed();
          }
      })
      .catch((error) => setError(error));
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
