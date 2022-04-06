import React from "react";
import { useAnalytics } from "../../analytics/analyticsHooks";
import { useSegmentCustomers } from "../../api-hooks/segmentsApi";
import { useAccessToken } from "../../api-hooks/token";
import { addCustomerAsync, removeCustomerAsync } from "../../api/segmentsApi";
import { EmptyList, Paginator } from "../molecules";
import { Subtitle } from "../molecules/layout";
import { Spinner } from "../molecules/Spinner";
import { BigPopup } from "../molecules/popups/BigPopup";
import { SearchBox } from "../molecules/SearchBox";
import { MemberRow } from "../molecules/MemberRow";
import { SearchCustomer } from "../molecules/SearchCustomer";
import { useNavigation } from "../../utility/useNavigation";

export const MembersSection = ({ segment }) => {
  const token = useAccessToken();
  const { analytics } = useAnalytics();

  const [error, setError] = React.useState();
  const [searchTerm, setSearchTerm] = React.useState("");
  const [trigger, setTrigger] = React.useState({});
  const [isEditMembersPopupOpen, setEditMembersPopupOpen] =
    React.useState(false);

  const members = useSegmentCustomers({ id: segment.id, searchTerm, trigger });
  const numMembers = members?.items?.length;

  const handleAddCustomer = (customer) => {
    addCustomerAsync({ token, id: segment.id, customerId: customer.id })
      .then(() => {
        analytics.track("site:segment_addCustomer_success");
        setEditMembersPopupOpen(false);
        setTrigger(customer);
      })
      .catch((e) => {
        analytics.track("site:segment_addCustomer_failure");
        setError(e);
      });
  };
  const handleRemoveCustomer = (customerId) => {
    removeCustomerAsync({ token, id: segment.id, customerId })
      .then(() => {
        analytics.track("site:segment_removeCustomer_success");
        setTrigger(customerId);
      })
      .catch((e) => {
        analytics.track("site:segment_removeCustomer_failure");
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
      {numMembers === 0 && <EmptyList>Segment has no members.</EmptyList>}
      <div className="mt-3">
        {members.items &&
          members.items.map((u) => (
            <MemberRow
              key={u.id}
              member={u}
              onDelete={handleRemoveCustomer}
              error={error}
            />
          ))}
      </div>
      <Paginator {...members.pagination} />
      <BigPopup
        isOpen={isEditMembersPopupOpen}
        setIsOpen={setEditMembersPopupOpen}
      >
        <SearchCustomer
          subtitle="Add Member to Segment"
          onAddCustomer={handleAddCustomer}
          error={error}
        />
      </BigPopup>
    </React.Fragment>
  );
};
