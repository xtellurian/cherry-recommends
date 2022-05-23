import React from "react";
import { Person } from "react-bootstrap-icons";
import FlexRow from "./layout/EntityFlexRow";
import { useNavigation } from "../../utility/useNavigation";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";

export const MemberRow = ({ member, onDelete, error }) => {
  const [isDeletePopupOpen, setIsDeletePopupOpen] = React.useState(false);
  const { navigate } = useNavigation();

  const handleMemberClick = (member) => {
    navigate({ pathname: `/customers/customers/detail/${member.id}` });
  };

  return (
    <FlexRow
      className="clickable-row"
      onClick={() => handleMemberClick(member)}
      style={{ cursor: "pointer" }}
    >
      <div className="flex-shrink-0">
        <Person className="m-2" size={25} />
      </div>
      <div className="flex-grow-1">{member.name || member.commonId}</div>

      <div onClick={(e) => e.stopPropagation()}>
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
      </div>
    </FlexRow>
  );
};
