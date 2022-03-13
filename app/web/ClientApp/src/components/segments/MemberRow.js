import React from "react";
import { Person } from "react-bootstrap-icons";
import FlexRow from "../molecules/layout/EntityFlexRow";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";

export const MemberRow = ({ member, onDelete, error }) => {
  const [isDeletePopupOpen, setIsDeletePopupOpen] = React.useState(false);

  return (
    <FlexRow>
      <div className="flex-shrink-0">
        <Person className="m-2" size={25} />
      </div>
      <div className="flex-grow-1">{member.name}</div>
      <div className="text-right">
        <button
          className="btn btn-outline-primary"
          onClick={() => setIsDeletePopupOpen(true)}
        >
          X
        </button>
      </div>
      <ConfirmDeletePopup
        entity={member}
        error={error}
        open={isDeletePopupOpen}
        setOpen={setIsDeletePopupOpen}
        handleDelete={() => onDelete(member.id)}
      />
    </FlexRow>
  );
};
