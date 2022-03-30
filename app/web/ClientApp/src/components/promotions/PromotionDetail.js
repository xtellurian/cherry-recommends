import React from "react";
import { useHistory, useParams } from "react-router-dom";

import { usePromotion } from "../../api-hooks/promotionsApi";
import {
  deletePromotionAsync,
  updatePromotionAsync,
  setPropertiesAsync,
} from "../../api/promotionsApi";
import {
  PageHeading,
  Spinner,
  ErrorCard,
  MoveUpHierarchyPrimaryButton,
} from "../molecules";
import { CopyableField } from "../molecules/fields/CopyableField";
import { PropertiesTableView } from "../molecules/PropertiesTableView";
import { BigPopup } from "../molecules/popups/BigPopup";

import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import { CommonEntityPropertyEditorPopup } from "../molecules/popups/CommonEntityPropertyEditorPopup";
import { useAccessToken } from "../../api-hooks/token";
import { EditItem } from "./EditPromotion";

export const ItemDetail = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const [trigger, setTrigger] = React.useState({});
  const item = usePromotion({ id, trigger });
  const [isDeletePopupOpen, setisDeletePopupOpen] = React.useState(false);
  const [isEditPopupOpen, setIsEditPopupOpen] = React.useState(false);
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
    updatePromotionAsync({ token, id, promotion: _item })
      .then((v) => {
        setTrigger(v);
        setError(undefined);
      })
      .catch(setError);
  };

  return (
    <>
      <MoveUpHierarchyPrimaryButton to="/promotions">
        Back to Promotions
      </MoveUpHierarchyPrimaryButton>
      <button
        onClick={() => setisDeletePopupOpen(true)}
        className="float-right ml-2 btn btn-danger"
      >
        Delete Promotion
      </button>
      <button
        onClick={() => setIsEditPopupOpen(true)}
        className="float-right ml-2 btn btn-outline-primary"
      >
        Edit Promotion
      </button>
      <PageHeading
        title={item.name || "..."}
        subtitle="Promotion Detail"
        showHr={true}
      />
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
            value={item.benefitValue || "Not Set"}
            isNumeric={true}
            min={0}
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
            label="Redemption Limit"
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
              deletePromotionAsync({ id: item.id, token })
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
      <BigPopup isOpen={isEditPopupOpen} setIsOpen={setIsEditPopupOpen}>
        <EditItem item={item} />
      </BigPopup>
    </>
  );
};
