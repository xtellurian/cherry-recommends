import React from "react";
import { Person } from "react-bootstrap-icons";
import { useHistory } from "react-router-dom";
import { useAccessToken } from "../../api-hooks/token";
import FlexRow from "../molecules/layout/EntityFlexRow";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import { deleteBusinessMemberAsync } from "../../api/businessesApi";

export const MemberRow = ({ businessId, member }) => {
  const token = useAccessToken();
  const history = useHistory();
  const [isDeletePopupOpen, setIsDeletePopupOpen] = React.useState(false);
  const [error, setError] = React.useState();

  return (
    <FlexRow>
      <div className="flex-shrink-0">
        <Person className="m-2" size={25} />
      </div>
      <div className="flex-grow-1">{member.name || member.commonId}</div>
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
        handleDelete={() =>
          deleteBusinessMemberAsync({
            id: businessId,
            token,
            customerId: member.id,
          })
            .then(() =>
              history.push(`/businesses/detail/${businessId}?tab=members`)
            )
            .catch(setError)
        }
      />
    </FlexRow>
  );
};
