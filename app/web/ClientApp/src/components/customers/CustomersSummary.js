import React from "react";
import { Link } from "react-router-dom";
import { useCustomers } from "../../api-hooks/customersApi";
import { Title, Spinner, Paginator, ErrorCard } from "../molecules";
import { SearchBox } from "../molecules/SearchBox";
import { CustomerListItem } from "../molecules/CustomerLists";
import { EmptyList } from "../molecules";

const CreateButton = () => {
  return (
    <Link to="/customers/create">
      <button className="btn btn-primary">Add a Customer</button>
    </Link>
  );
};
export const CustomersSummary = () => {
  const [searchTerm, setSearchTerm] = React.useState("");
  const trackedUsers = useCustomers({ searchTerm });

  return (
    <div>
      <div className="float-right">
        <CreateButton />
        {/* <Link to="/customers/upload">
          <button className="btn btn-outline-primary ml-1">Upload CSV</button>
        </Link> */}
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
            <CustomerListItem
              key={u.id}
              customer={u}
            />
          ))}
      </div>
      <Paginator {...trackedUsers.pagination} />
    </div>
  );
};