import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import React from "react";
import Modal from "react-modal";
import { ErrorCard } from "../ErrorCard";
import { PropertiesEditor } from "../PropertiesEditor";
import { big, closeButton } from "./styles";

export const CommonEntityPropertyEditorPopup = ({
  isOpen,
  setIsOpen,
  label,
  entity,
  initialProperties,
  handleSave,
}) => {
  const onRequestClose = () => setIsOpen(false);
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
    <Modal
      isOpen={isOpen}
      onRequestClose={onRequestClose}
      style={big}
      contentLabel="Edit Properties"
    >
      <button
        className="btn btn-link"
        onClick={onRequestClose}
        style={closeButton}
      >
        <FontAwesomeIcon icon={faCircleXmark} />
      </button>
      <div className="text-center">
        <div className="pb-3">
          <h5>{label}</h5>
        </div>
        {error && <ErrorCard error={error} />}
        <div className="m-2">
          <PropertiesEditor
            label="Resource Properties"
            placeholder="Add properties to this resource"
            onPropertiesChanged={setProperties}
            initialProperties={initialProperties}
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
    </Modal>
  );
};
