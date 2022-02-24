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
    const _item = Object.assign({}, item);
    _item[property] = value;
    updateItemAsync({ token, id, item: _item })
      .then((v) => {
        setTrigger(v);
        setError(undefined);
      })
      .catch(setError);
  };

  return (
    <React.Fragment>
      <button
        onClick={() => setisDeletePopupOpen(true)}
        className="float-right ml-1 btn btn-danger"
      >
        Delete Promotion
      </button>
      <BackButton className="float-right" to="/promotions">
        All Promotions
      </BackButton>
      <Title>Promotion Detail</Title>
      <Subtitle>{item.name || "..."}</Subtitle>
      <hr />
      {item.loading && <Spinner>Loading Promotion</Spinner>}
      {item.error && <ErrorCard error={item.error} />}
      {!isDeletePopupOpen && error && (
        <div className="mb-3">
          <ErrorCard error={error} />
        </div>
      )}
      {!item.loading && !item.error && (
        <div>
          <CopyableField label="Promotion Id" value={item.commonId} />

          <CopyableField
            label="Promotion Type"
            value={item.promotionType || "Not Set"}
            isEditable={false}
            onValueEdited={(v) => handleEditProperty("promotionType", v)}
          />

          <CopyableField
            label="Benefit Type"
            value={item.benefitType || "Not Set"}
            isEditable={false}
            onValueEdited={(v) => handleEditProperty("benefitType", v)}
          />

          <CopyableField
            label="Benefit Value"
            value={item.benefitValue}
            isNumeric={true}
            min={1}
            isEditable={true}
            onValueEdited={(v) => handleEditProperty("benefitValue", v)}
          />

          <CopyableField
            label="Direct Cost"
            value={item.directCost || "Not Set"}
            isNumeric={true}
            min={0}
            isEditable={true}
            onValueEdited={(v) => handleEditProperty("directCost", v)}
          />

          <CopyableField
            label="Redeemable (1-6)"
            value={item.numberOfRedemptions || "Not Set"}
            isNumeric={true}
            min={1}
            max={6}
            isEditable={true}
            onValueEdited={(v) => handleEditProperty("numberOfRedemptions", v)}
          />

          <div className="ml-1 mb-3">
            <label className="font-weight-bold">Description</label>
            <p>{item.description}</p>
          </div>

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
                .then(() => history.push("/promotions"))
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
