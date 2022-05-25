import React from "react";

import { PropertiesEditor } from "../PropertiesEditor";
import { BigPopup } from "./BigPopup";
import EditPageLayout from "../layout/EditPageLayout";
import { Typography } from "../Typography";
import { suggestedPromotionProperties } from "../../promotions/SuggestedProperties";

export const CommonEntityPropertyEditorPopup = ({
  isOpen,
  setIsOpen,
  entity,
  initialProperties,
  handleSave,
}) => {
  const [properties, setProperties] = React.useState(entity.properties);
  const [error, setError] = React.useState();

  if (handleSave === undefined) {
    throw new Error("handleSave is not a function");
  }
  const onSave = () => {
    setError(null);
    handleSave(properties).then(setIsOpen(false)).catch(setError);
  };

  return (
    <BigPopup isOpen={isOpen} setIsOpen={setIsOpen}>
      <EditPageLayout
        header={
          <Typography className="semi-bold" variant="h6">
            Edit Properties
          </Typography>
        }
        error={error}
      >
        <div>
          <div className="m-2">
            <PropertiesEditor
              label=""
              placeholder="Add properties to this resource"
              onPropertiesChanged={setProperties}
              initialProperties={initialProperties}
              suggestions={suggestedPromotionProperties}
            />

            <div
              className="btn-group mt-3 w-100"
              role="group"
              aria-label="Delete or rename buttons"
            >
              <button
                className="btn btn-secondary"
                onClick={() => setIsOpen(false)}
              >
                Cancel
              </button>
              <button className="btn btn-success" onClick={onSave}>
                Save
              </button>
            </div>
          </div>
        </div>
      </EditPageLayout>
    </BigPopup>
  );
};
