import React, { useState } from "react";
import { usePromotions } from "../../api-hooks/promotionsApi";
import { Title, EmptyList, Spinner, ErrorCard, Paginator } from "../molecules";
import { CreateButtonClassic } from "../molecules/CreateButton";
import { InputGroup, TextInput } from "../molecules/TextInput";

import { ItemRow } from "./ItemRow";
import { PromotionsFilter } from "./PromotionsFilter";

export const RecommendableItemsSummary = () => {
  const [trigger, setTrigger] = useState({});

  const [filters, setFilters] = useState({
    searchTerm: "",
    promotionType: [],
    benefitType: [],
    addedBy: [],
  });

  const items = usePromotions({
    trigger: trigger,
    searchTerm: trigger?.searchTerm,
    benefitType: trigger?.benefitType?.join(","),
    promotionType: trigger?.promotionType?.join(","),
    weeksAgo: trigger?.addedBy?.length ? Math.max(...trigger?.addedBy) : 0,
  });

  const onSearch = (value) => {
    setTrigger({ ...filters, searchTerm: value });
  };

  const isEmpty = items.items && items.items.length === 0;

  return (
    <React.Fragment>
      <CreateButtonClassic className="float-right" to="/promotions/create">
        Create Promotion
      </CreateButtonClassic>
      <Title>Promotion Catalogue</Title>
      <hr />

      <InputGroup>
        <TextInput
          label="Search"
          placeholder="Press Enter to search"
          onReturn={onSearch}
        />
      </InputGroup>

      <PromotionsFilter filters={filters} setFilters={setFilters} />

      <div className="mt-4">
        {items.loading && <Spinner>Loading Promotions</Spinner>}
        {isEmpty && (
          <div className="text-center my-5">
            No results match your search. Please try again.
            {/* <div className="text-black-50 mt-2"></div> */}
          </div>
        )}
        {items.error && <ErrorCard error={items.error} />}
        {items.items && items.items.map((p) => <ItemRow key={p.id} item={p} />)}

        {!isEmpty && items.pagination && <Paginator {...items.pagination} />}
      </div>
    </React.Fragment>
  );
};
