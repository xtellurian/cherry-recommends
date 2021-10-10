import React from "react";
import { useHistory, useParams } from "react-router-dom";
import { useItem } from "../../api-hooks/recommendableItemsApi";
import {
  deleteItemAsync,
  updateItemAsync,
  setPropertiesAsync,
} from "../../api/recommendableItemsApi";
import { Title, Subtitle, Spinner, ErrorCard, BackButton } from "../molecules";
import { CopyableField } from "../molecules/fields/CopyableField";
import { PropertiesTableView } from "../molecules/PropertiesTableView";

import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import { CommonEntityPropertyEditorPopup } from "../molecules/popups/CommonEntityPropertyEditorPopup";
import { useAccessToken } from "../../api-hooks/token";

export const ItemDetail = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const [trigger, setTrigger] = React.useState({});
  const item = useItem({ id, trigger });
  const [isDeletePopupOpen, setisDeletePopupOpen] = React.useState(false);
  const [isPropertyEditorPopupOpen, setIsPropertyEditorPopupOpen] =
    React.useState(false);
  const [error, setError] = React.useState();
  const history = useHistory();

  const handleSetAsync = async (properties) => {
    await setPropertiesAsync({ token, id, properties });
    setTrigger({});
  };

  const handleEditProperty = (property, value) => {
    item[property] = value;
    updateItemAsync({ token, id, item }).then(setTrigger).catch(setError);
  };

  return (
    <React.Fragment>
      <button
        onClick={() => setisDeletePopupOpen(true)}
        className="float-right ml-1 btn btn-danger"
      >
        Delete Item
      </button>
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
          <CopyableField
            label="List Price"
            value={item.listPrice}
            isEditable={true}
            onValueEdited={(v) => handleEditProperty("listPrice", v)}
          />

          <CopyableField
            label="Direct Cost"
            value={item.directCost || "Not Set"}
            isEditable={true}
            onValueEdited={(v) => handleEditProperty("directCost", v)}
          />

          <p>{item.description}</p>

          <PropertiesTableView
            properties={item.properties}
            onEdit={() => setIsPropertyEditorPopupOpen(true)}
          />

          <ConfirmDeletePopup
            entity={item}
            error={error}
            open={isDeletePopupOpen}
            setOpen={setisDeletePopupOpen}
            handleDelete={() =>
              deleteItemAsync({ id: item.id, token })
                .then(() => history.push("/recommendable-items"))
                .catch(setError)
            }
          />

          <CommonEntityPropertyEditorPopup
            entity={item}
            error={error}
            isOpen={isPropertyEditorPopupOpen}
            setIsOpen={setIsPropertyEditorPopupOpen}
            initialProperties={item.properties}
            handleSave={async (properties) => await handleSetAsync(properties)}
          />
        </div>
      )}
    </React.Fragment>
  );
};
