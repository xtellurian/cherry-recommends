import React from "react";

import { Spinner, Paginator } from "../molecules";
import { SearchBox } from "../molecules/SearchBox";
import { BusinessRow } from "./BusinessRow";
import { useBusinesses } from "../../api-hooks/businessesApi";
import { EmptyList } from "../molecules";
import Layout, {
  CreateEntityButton,
} from "../molecules/layout/EntitySummaryLayout";

export const BusinessesSummary = () => {
  const [searchTerm, setSearchTerm] = React.useState("");
  const businesses = useBusinesses({ searchTerm });

  return (
    <Layout
      header="Businesses"
      createButton={
        <CreateEntityButton to="/customers/businesses/create">
          Create a Business
        </CreateEntityButton>
      }
      error={businesses.error}
    >
      {businesses.loading && <Spinner />}
      <SearchBox onSearch={setSearchTerm} />
      {businesses.items && businesses.items.length === 0 && (
        <EmptyList>
          There are no Businesses.
          <CreateEntityButton to="/customers/businesses/create">
            Create Business
          </CreateEntityButton>
        </EmptyList>
      )}
      <div className="mt-3">
        {businesses.items &&
          businesses.items.map((u) => <BusinessRow key={u.id} business={u} />)}
      </div>
      <Paginator {...businesses.pagination} />
    </Layout>
  );
};
