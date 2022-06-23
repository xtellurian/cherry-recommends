import React from "react";
import { Typography, ErrorCard, AsyncButton } from "../../molecules";
import { BigPopup } from "../../molecules/popups/BigPopup";
import AsyncSelectSegment from "../../molecules/selectors/AsyncSelectSegment";

export const AddSegmentPopup = ({
  isOpen,
  setIsOpen,
  onAdd,
  error,
  loading,
}) => {
  const [selectedItem, setSelectedItem] = React.useState();

  return (
    <React.Fragment>
      <BigPopup isOpen={isOpen} setIsOpen={setIsOpen}>
        <Typography variant="h6" className="semi-bold border-bottom pb-2">
          Add Segment
        </Typography>
        {error && <ErrorCard error={error} />}
        <div style={{ minHeight: "100px" }}>
          <AsyncSelectSegment
            label="Segment"
            isMulti={false}
            onChange={(v) => {
              setSelectedItem(v.value);
            }}
          />
        </div>
        <AsyncButton
          loading={loading}
          className="btn btn-block btn-primary"
          disabled={!selectedItem}
          onClick={() => onAdd(selectedItem)}
        >
          Add
        </AsyncButton>
      </BigPopup>
    </React.Fragment>
  );
};
