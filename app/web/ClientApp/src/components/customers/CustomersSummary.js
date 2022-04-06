import React from "react";

import { useCustomers } from "../../api-hooks/customersApi";
import { Title, Spinner, Paginator, ErrorCard, Navigation } from "../molecules";
import { SearchBox } from "../molecules/SearchBox";
import { CustomerRow } from "../customers/CustomerRow";
import { EmptyList } from "../molecules";

const CreateButton = () => {
  return (
    <Navigation to="/customers/create">
      <button className="btn btn-primary">Add a Customer</button>
    </Navigation>
  );
};

export const CustomersSummary = () => {
  const [searchTerm, setSearchTerm] = React.useState("");
  const trackedUsers = useCustomers({ searchTerm });

  return (
    <div>
      <div className="float-right">
        <CreateButton />
      </div>
      <Title>Customers</Title>
      <hr />
      {trackedUsers.loading && <Spinner />}
      {trackedUsers.error && <ErrorCard error={trackedUsers.error} />}
      <SearchBox onSearch={setSearchTerm} />
      {trackedUsers.items && trackedUsers.items.length === 0 && (
        <EmptyList>
          There are no Customers.
          <div className="mt-3">
            <CreateButton />
          </div>
        </EmptyList>
      )}
      <div className="mt-3">
        {trackedUsers.items &&
          trackedUsers.items.map((u) => (
            <CustomerRow key={u.id} customer={u} />
          ))}
      </div>
      <Paginator {...trackedUsers.pagination} />
    </div>
  );
};
