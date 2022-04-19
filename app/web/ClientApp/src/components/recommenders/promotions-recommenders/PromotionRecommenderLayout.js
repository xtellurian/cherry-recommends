import React from "react";
import { useLocation } from "react-router-dom";
import { useParams } from "react-router";

import { useNavigation } from "../../../utility/useNavigation";
import { MoveUpHierarchyPrimaryButton, PageHeading } from "../../molecules";
import { ItemsRecommenderPrimaryNav } from "./ItemsRecommenderPrimaryNav";
import { usePromotionsRecommender } from "../../../api-hooks/promotionsRecommendersApi";
import { RecommenderStatusBox } from "../../molecules/RecommenderStatusBox";
import { setSettingsAsync } from "../../../api/promotionsRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";

export const PromotionRecommenderLayout = ({ children }) => {
  const location = useLocation();
  const token = useAccessToken();
  const { id } = useParams();
  const { appendCurrentURL } = useNavigation();
  const [trigger, setTrigger] = React.useState({});
  const recommender = usePromotionsRecommender({ id, trigger });

  const setEnabled = (value) => {
    console.log(value);
    setSettingsAsync({
      id,
      settings: {
        enabled: value,
      },
      token,
    })
      .then(setTrigger)
      .catch(console.error);
  };

  const searchParams = new URLSearchParams(location.search);
  searchParams.delete("tab");

  return (
    <>
      <RecommenderStatusBox recommender={recommender} setEnabled={setEnabled} />
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
