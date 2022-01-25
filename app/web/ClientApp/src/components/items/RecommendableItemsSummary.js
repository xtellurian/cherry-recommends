import React from "react";
import { useItems } from "../../api-hooks/recommendableItemsApi";
import { Title, EmptyList, Spinner, ErrorCard, Paginator } from "../molecules";
import { CreateButtonClassic } from "../molecules/CreateButton";

import { ItemRow } from "./ItemRow";

export const RecommendableItemsSummary = () => {
  const items = useItems();
  return (
    <React.Fragment>
      <CreateButtonClassic
        className="float-right"
        to="/recommendable-items/create"
      >
        Create Recommendable Item
      </CreateButtonClassic>
      <Title>Item Catalogue</Title>
      <hr />
      {items.loading && <Spinner>Loading Items</Spinner>}
      {items.items && items.items.length === 0 && (
        <EmptyList>
          <div>No Existing Items</div>
          <CreateButtonClassic to="/recommendable-items/create">
            Create Recommendable Item
          </CreateButtonClassic>
        </EmptyList>
      )}
      {items.error && <ErrorCard error={items.error} />}
      {items.items && items.items.map((p) => <ItemRow key={p.id} item={p} />)}

      {items.pagination && <Paginator {...items.pagination} />}
    </React.Fragment>
  );
};
