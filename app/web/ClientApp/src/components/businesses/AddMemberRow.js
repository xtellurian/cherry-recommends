import React from "react";
import { Person } from "react-bootstrap-icons";
import { useHistory } from "react-router-dom";
import { useAccessToken } from "../../api-hooks/token";
import FlexRow from "../molecules/layout/EntityFlexRow";
import { addBusinessMemberAsync } from "../../api/businessesApi";

export const AddMemberRow = ({ businessId, member }) => {
  const token = useAccessToken();
  const history = useHistory();
  const [error, setError] = React.useState();

  const handleAdd = () => {
    addBusinessMemberAsync({ id: businessId, token, customer: member })
      .then(() => {
        history.push(`/businesses/detail/${businessId}?tab=members`);
      })
      .catch(setError);
  };

  return (
    <FlexRow>
      <div className="flex-shrink-0">
        <Person className="m-2" size={25} />
      </div>
      <div className="flex-grow-1">{member.name || member.commonId}</div>
      <div className="text-right">
        <button
          className="btn btn-outline-primary"
          onClick={() => handleAdd(member)}
        >
          Add
        </button>
      </div>
    </FlexRow>
  );
};
