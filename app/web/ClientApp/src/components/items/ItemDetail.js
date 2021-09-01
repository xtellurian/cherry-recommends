import React from "react";
import { useHistory, useParams } from "react-router-dom";
import { useItem } from "../../api-hooks/recommendableItemsApi";
import { deleteItemAsync } from "../../api/recommendableItemsApi";
import { Title, Subtitle, Spinner, ErrorCard, BackButton } from "../molecules";
import { CopyableField } from "../molecules/CopyableField";

import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import { useAccessToken } from "../../api-hooks/token";

export const ItemDetail = () => {
  const { id } = useParams();
  const item = useItem({ id });
  const [isPopupOpen, setIsPopupOpen] = React.useState(false);
  const [error, setError] = React.useState();
  const history = useHistory();

  return (
    <React.Fragment>
      <BackButton className="float-right" to="/recommendable-items">
        All Recommendable Items
      </BackButton>
      <Title>Item Detail</Title>
      <Subtitle>{item.name || "..."}</Subtitle>
      <hr />
      {item.loading && <Spinner>Loading Item</Spinner>}
      {item.error && <ErrorCard error={item.error} />}
      {!item.loading && !item.error && (
        <div>
          <CopyableField label="Item Id" value={item.commonId} />
          <CopyableField label="List Price" value={item.listPrice} />
          {item.directCost && (
            <CopyableField label="Direct Cost" value={item.directCost} />
          )}
          <p>{item.description}</p>
          <button
            onClick={() => setIsPopupOpen(true)}
            className="btn btn-danger"
          >
            Delete Item
          </button>
          <ConfirmDeletePopup
            entity={item}
            error={error}
            open={isPopupOpen}
            setOpen={setIsPopupOpen}
            handleDelete={() =>
              deleteItemAsync({ id: item.id })
                .then(history.push("/recommendable-items"))
                .catch(setError)
            }
          />
        </div>
      )}
    </React.Fragment>
  );
};
