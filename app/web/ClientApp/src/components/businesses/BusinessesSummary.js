import React from "react";
import { Link } from "react-router-dom";
import { Title, Spinner, Paginator, ErrorCard } from "../molecules";
import { SearchBox } from "../molecules/SearchBox";
import { BusinessListItem } from "../molecules/BusinessList";
import { useBusinesses } from "../../api-hooks/businessesApi";
import { EmptyList } from "../molecules";

const CreateButton = () => {
  return (
    <Link to="/businesses/create">
      <button className="btn btn-primary">Create New</button>
    </Link>
  );
};
export const BusinessesSummary = () => {
  const [searchTerm, setSearchTerm] = React.useState("");
  const businesses = useBusinesses({ searchTerm });

  return (
    <div>
      <div className="float-right">
        <CreateButton />
      </div>
      <Title>Businesses</Title>
      <hr />
      {businesses.loading && <Spinner />}
      {businesses.error && <ErrorCard error={businesses.error} />}
      <SearchBox onSearch={setSearchTerm} />
      {businesses.items && businesses.items.length === 0 && (
        <EmptyList>
          There are no Businesses.
          <div className="mt-3">
            <CreateButton />
          </div>
        </EmptyList>
      )}
      <div className="mt-3">
        {businesses.items &&
          businesses.items.map((u) => (
            <BusinessListItem key={u.id} business={u} />
          ))}
      </div>
      <Paginator {...businesses.pagination} />
    </div>
  );
};
