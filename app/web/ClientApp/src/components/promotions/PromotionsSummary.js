import React, { useState } from "react";
import { usePromotions } from "../../api-hooks/promotionsApi";
import { Title, Spinner, ErrorCard, Paginator } from "../molecules";
import { CreateButtonClassic } from "../molecules/CreateButton";
import { PromotionRow } from "./PromotionRow";
import { PromotionsFilter } from "./PromotionsFilter";
import { SearchBox } from "../molecules/SearchBox";

export const RecommendableItemsSummary = () => {
  const [trigger, setTrigger] = useState({});
  const [searchTerm, setSearchTerm] = useState("");

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
    setSearchTerm(value);
    setTrigger({ ...filters, searchTerm: value });
  };

  React.useEffect(() => {
    setTrigger({ ...filters, searchTerm: searchTerm });
  }, [filters]);

  const isEmpty = items.items && items.items.length === 0;

  return (
    <React.Fragment>
      <CreateButtonClassic className="float-right" to="promotions/create">
        Create Promotion
      </CreateButtonClassic>
      <Title>Promotion Catalogue</Title>
      <hr />

      <SearchBox onSearch={onSearch} />

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
        {items.items &&
          items.items.map((p) => <PromotionRow key={p.id} promotion={p} />)}

        {!isEmpty && items.pagination && <Paginator {...items.pagination} />}
      </div>
    </React.Fragment>
  );
};
