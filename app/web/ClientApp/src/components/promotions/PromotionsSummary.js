import React, { useState } from "react";
import { usePromotions } from "../../api-hooks/promotionsApi";
import { Spinner, Paginator } from "../molecules";
import { PromotionRow } from "./PromotionRow";
import { PromotionsFilter } from "./PromotionsFilter";
import { SearchBox } from "../molecules/SearchBox";

import Layout, {
  CreateEntityButton,
} from "../molecules/layout/EntitySummaryLayout";

export const RecommendableItemsSummary = () => {
  const [trigger, setTrigger] = useState({});
  const [searchTerm, setSearchTerm] = useState("");

  const [filters, setFilters] = useState({
    searchTerm: "",
    promotionType: [],
    benefitType: [],
    addedBy: [],
  });

  const promos = usePromotions({
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

  const isEmpty = promos.items && promos.items.length === 0;

  return (
    <Layout
      header="Promotions"
      createButton={
        <CreateEntityButton to="/promotions/promotions/create">
          Create a Promotion
        </CreateEntityButton>
      }
      error={promos.error}
    >
      <SearchBox onSearch={onSearch} />

      <PromotionsFilter filters={filters} setFilters={setFilters} />

      <div className="mt-4">
        {promos.loading && <Spinner>Loading Promotions</Spinner>}
        {isEmpty && (
          <div className="text-center my-5">
            No results match your search. Please try again.
            {/* <div className="text-black-50 mt-2"></div> */}
          </div>
        )}

        {promos.items &&
          promos.items.map((p) => <PromotionRow key={p.id} promotion={p} />)}

        {!isEmpty && promos.pagination && <Paginator {...promos.pagination} />}
      </div>
    </Layout>
  );
};
