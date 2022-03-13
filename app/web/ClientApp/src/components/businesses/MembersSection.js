import React from "react";
import { useHistory } from "react-router-dom";
import { Subtitle, Spinner, EmptyList, Paginator } from "../molecules";
import { BigPopup } from "../molecules/popups/BigPopup";
import { SearchBox } from "../molecules/SearchBox";
import { MemberRow } from "./MemberRow";
import { useBusinessMembers } from "../../api-hooks/businessesApi";
import { SearchCustomer } from "../molecules/SearchCustomer";
import { addBusinessMemberAsync } from "../../api/businessesApi";
import { useAccessToken } from "../../api-hooks/token";
import { useAnalytics } from "../../analytics/analyticsHooks";

export const MembersSection = ({ business }) => {
  const token = useAccessToken();
  const history = useHistory();
  const { analytics } = useAnalytics();

  const [error, setError] = React.useState();
  const [searchTerm, setSearchTerm] = React.useState("");
  const [isEditMembersPopupOpen, setEditMembersPopupOpen] =
    React.useState(false);

  const members = useBusinessMembers({ id: business.id, searchTerm });
  const numMembers = members?.items?.length;

  const handleAddCustomer = (customer) => {
    addBusinessMemberAsync({ token, id: business.id, customer })
      .then(() => {
        analytics.track("site:business_addMember_success");
        history.push(`/businesses/detail/${business.id}?tab=members`);
      })
      .catch((e) => {
        analytics.track("site:business_addMember_failure");
        setError(e);
      });
  };

  return (
    <React.Fragment>
      <Subtitle>Members ({numMembers})</Subtitle>
      <button
        className="btn btn-outline-primary float-right mb-3"
        onClick={() => setEditMembersPopupOpen(true)}
      >
        Add a Member
      </button>
      <SearchBox onSearch={setSearchTerm} />
      {members.loading && <Spinner>Loading Members</Spinner>}
      {numMembers === 0 && <EmptyList>Business has no members.</EmptyList>}
      <div className="mt-3">
        {members.items &&
          members.items.map((u) => (
            <MemberRow key={u.id} businessId={business.id} member={u} />
          ))}
      </div>
      <Paginator {...members.pagination} />
      <BigPopup
        isOpen={isEditMembersPopupOpen}
        setIsOpen={setEditMembersPopupOpen}
      >
        <SearchCustomer
          subtitle="Add Member to Business"
          onAddCustomer={handleAddCustomer}
          error={error}
        />
      </BigPopup>
    </React.Fragment>
  );
};
