import React from "react";

import { useCustomers } from "../../api-hooks/customersApi";
import { Spinner, Paginator } from "../molecules";
import { SearchBox } from "../molecules/SearchBox";
import { CustomerRow } from "../customers/CustomerRow";
import { EmptyList } from "../molecules";
import Layout, {
  CreateEntityButton,
} from "../molecules/layout/EntitySummaryLayout";

export const CustomersSummary = () => {
  const [searchTerm, setSearchTerm] = React.useState("");
  const customers = useCustomers({ searchTerm });

  return (
    <Layout
      header="Customers"
      createButton={
        <CreateEntityButton to="/customers/customers/create">
          Create a Customer
        </CreateEntityButton>
      }
      error={customers.error}
    >
      {customers.loading && <Spinner />}
      <SearchBox onSearch={setSearchTerm} />
      {customers.items && customers.items.length === 0 && (
        <EmptyList>
          There are no Customers.
          <div className="mt-3">
            <CreateEntityButton to="/customers/customers/create">
              Create a Customer
            </CreateEntityButton>
          </div>
        </EmptyList>
      )}
      <div className="mt-3">
        {customers.items &&
          customers.items.map((u) => <CustomerRow key={u.id} customer={u} />)}
      </div>
      <Paginator {...customers.pagination} />
    </Layout>
  );
};
