import React from "react";
import { useItems } from "../../api-hooks/recommendableItemsApi";
import { Title, EmptyList, Spinner, ErrorCard, Paginator } from "../molecules";
import { CreateButtonClassic } from "../molecules/CreateButton";

import { ItemRow } from "./ItemRow";

export const RecommendableItemsSummary = () => {
  const items = useItems();
  return (
    <React.Fragment>
      <CreateButtonClassic className="float-right" to="/promotions/create">
        Create Promotion
      </CreateButtonClassic>
      <Title>Promotion Catalogue</Title>
      <hr />
      {items.loading && <Spinner>Loading Promotions</Spinner>}
      {items.items && items.items.length === 0 && (
        <EmptyList>
          <div>No Existing Promotions</div>
          <CreateButtonClassic to="/promotions/create">
            Create Promotion
          </CreateButtonClassic>
        </EmptyList>
      )}
      {items.error && <ErrorCard error={items.error} />}
      {items.items && items.items.map((p) => <ItemRow key={p.id} item={p} />)}

      {items.pagination && <Paginator {...items.pagination} />}
    </React.Fragment>
  );
};
