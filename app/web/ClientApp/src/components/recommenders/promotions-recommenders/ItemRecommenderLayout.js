import React from "react";
import { useLocation } from "react-router-dom";
import { useParams } from "react-router";

import { useNavigation } from "../../../utility/useNavigation";
import { MoveUpHierarchyPrimaryButton, PageHeading } from "../../molecules";
import { ItemsRecommenderPrimaryNav } from "./ItemsRecommenderPrimaryNav";
import { usePromotionsRecommender } from "../../../api-hooks/promotionsRecommendersApi";
import { RecommenderStatusBox } from "../../molecules/RecommenderStatusBox";

export const ItemRecommenderLayout = ({ children }) => {
  const location = useLocation();
  const { id } = useParams();
  const { appendCurrentURL } = useNavigation();
  const recommender = usePromotionsRecommender({ id });

  const searchParams = new URLSearchParams(location.search);
  searchParams.delete("tab");

  return (
    <>
      <RecommenderStatusBox recommender={recommender} />
      <MoveUpHierarchyPrimaryButton
        to={appendCurrentURL({
          pathname: "/recommenders/promotions-recommenders",
          search: searchParams.toString(),
        })}
      >
        Back to Recommenders
      </MoveUpHierarchyPrimaryButton>
      <PageHeading
        title={recommender.name || "..."}
        subtitle="Promotion Recommender"
      />
      <ItemsRecommenderPrimaryNav id={id} />
      {children}
    </>
  );
};
