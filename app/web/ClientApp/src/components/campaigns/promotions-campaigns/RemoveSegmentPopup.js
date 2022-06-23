import React from "react";
import { ConfirmationPopup } from "../../molecules/popups/ConfirmationPopup";
import { ErrorCard } from "../../molecules/ErrorCard";
import { AsyncButton } from "../../molecules";

export const RemoveSegmentPopup = ({
  segment,
  recommender,
  isOpen,
  setIsOpen,
  onRemove,
  error,
  loading,
}) => {
  if (segment && recommender) {
    const label = "Remove Segment";
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
            onClick={() => onRemove(segment)}
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
