import React from "react";
import { ConfirmationPopup } from "../../molecules/popups/ConfirmationPopup";
import { ErrorCard } from "../../molecules/ErrorCard";
import { removeItemAsync } from "../../../api/itemsRecommendersApi";
import { AsyncButton } from "../../molecules";
import { useAccessToken } from "../../../api-hooks/token";

export const RemoveItemPopup = ({
  item,
  recommender,
  open,
  setOpen,
  onRemoved,
}) => {
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const token = useAccessToken();
  const handleRemove = () => {
    removeItemAsync({ token, id: recommender.id, itemId: item.id })
      .then((r) => {
        onRemoved(r);
        setOpen(false);
      })
      .catch(setError)
      .finally(() => setLoading(false));
  };

  if (item && recommender) {
    const label = item
      ? `Remove ${item.name} from ${recommender.name}?`
      : "Loading...";
    return (
      <ConfirmationPopup isOpen={open} setIsOpen={setOpen} label={label}>
        <div className="m-2">{`Remove ${item.name} from ${recommender.name}?`}</div>
        {error && <ErrorCard error={error} />}
        <div
          className="btn-group"
          role="group"
          aria-label="Delete or cancel buttons"
        >
          <button className="btn btn-secondary" onClick={() => setOpen(false)}>
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
