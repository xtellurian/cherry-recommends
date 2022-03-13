import React from "react";
import { Person } from "react-bootstrap-icons";
import { useSearchCustomers } from "../../api-hooks/customersApi";
import FlexRow from "../molecules/layout/EntityFlexRow";
import { EmptyList } from "./empty/EmptyList";
import { ErrorCard } from "./ErrorCard";
import { Subtitle } from "./layout/PageHeadings";
import { Paginator } from "./Paginator";
import { SearchBox } from "./SearchBox";

export const AddCustomerRow = ({ customer, onAdd }) => {
  return (
    <FlexRow>
      <div className="flex-shrink-0">
        <Person className="m-2" size={25} />
      </div>
      <div className="flex-grow-1">{customer.name || customer.commonId}</div>
      <div className="text-right">
        <button
          className="btn btn-outline-primary"
          onClick={() => onAdd(customer)}
        >
          Add
        </button>
      </div>
    </FlexRow>
  );
};

export const SearchCustomer = ({ subtitle, onAddCustomer, error }) => {
  const [searchTerm, setSearchTerm] = React.useState("");
  const customers = useSearchCustomers({ searchTerm });

  return (
    <React.Fragment>
      <Subtitle>{subtitle}</Subtitle>
      <hr />
      <ErrorCard error={error} />
      <SearchBox onSearch={setSearchTerm} />
      {customers.items?.length === 0 && (
        <EmptyList>Customer not found.</EmptyList>
      )}
      <div className="mt-3">
        {customers.items &&
          customers.items.map((u) => (
            <AddCustomerRow key={u.id} customer={u} onAdd={onAddCustomer} />
          ))}
      </div>
      {customers.items?.length && <Paginator {...customers.pagination} />}
    </React.Fragment>
  );
};
