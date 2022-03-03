import React from "react";
import { Subtitle, Spinner, EmptyList, Paginator } from "../molecules";
import { BigPopup } from "../molecules/popups/BigPopup";
import { SearchBox } from "../molecules/SearchBox";
import { MemberRow } from "./MemberRow";
import { useBusinessMembers } from "../../api-hooks/businessesApi";

export const MembersSection = ({ business }) => {

  const [searchTerm, setSearchTerm] = React.useState("");
  const [isEditMembersPopupOpen, setEditMembersPopupOpen] = React.useState(false);
  const members = useBusinessMembers({ id: business.id, searchTerm });
  const numMembers = members?.items?.length; 

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
      <BigPopup isOpen={isEditMembersPopupOpen} setIsOpen={setEditMembersPopupOpen}>        
      </BigPopup>
    </React.Fragment>
  );
};
