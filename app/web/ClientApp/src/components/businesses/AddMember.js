import React from "react";
import { Subtitle, Spinner, EmptyList, Paginator } from "../molecules";
import { SearchBox } from "../molecules/SearchBox";
import { AddMemberRow } from "./AddMemberRow";
import { useSearchCustomers } from "../../api-hooks/customersApi";

export const AddMember = ({ business }) => {
  const [searchTerm, setSearchTerm] = React.useState("");
  const customers = useSearchCustomers({ searchTerm });

  return (
    <React.Fragment>
      <Subtitle>Add Member to Business</Subtitle>
      <hr />
      <SearchBox onSearch={setSearchTerm} />
      {customers.items?.length === 0 && (
        <EmptyList>Customer not found.</EmptyList>
      )}
      <div className="mt-3">
        {customers.items &&
          customers.items.map((u) => (
            <AddMemberRow
              key={u.id}
              businessId={business.id}
              member={{ commonId: u.commonId, name: u.name }}
            />
          ))}
      </div>
      {customers.items?.length && <Paginator {...customers.pagination} />}
    </React.Fragment>
  );
};
