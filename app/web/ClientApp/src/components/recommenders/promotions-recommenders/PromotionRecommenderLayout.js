import React from "react";
import { useLocation } from "react-router-dom";
import { useParams } from "react-router";

import { MoveUpHierarchyPrimaryButton, PageHeading } from "../../molecules";
import { ItemsRecommenderPrimaryNav } from "./ItemsRecommenderPrimaryNav";
import { usePromotionsRecommender } from "../../../api-hooks/promotionsRecommendersApi";
import { RecommenderStatusBox } from "../../molecules/RecommenderStatusBox";
import { setSettingsAsync } from "../../../api/promotionsRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import EntityDetailPageLayout from "../../molecules/layout/EntityDetailPageLayout";

export const PromotionRecommenderLayout = ({ children }) => {
  const location = useLocation();
  const token = useAccessToken();
  const { id } = useParams();
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
    <EntityDetailPageLayout
      backButton={
        <MoveUpHierarchyPrimaryButton
          to={{
            pathname: "/recommenders/promotions-recommenders",
            search: searchParams.toString(),
          }}
        >
          Back to Recommenders
        </MoveUpHierarchyPrimaryButton>
      }
      header={
        <PageHeading
          title={recommender.name || "..."}
          subtitle="Promotion Recommender"
        />
      }
      options={
        <RecommenderStatusBox
          recommender={recommender}
          setEnabled={setEnabled}
        />
      }
    >
      <ItemsRecommenderPrimaryNav id={id} />
      {children}
    </EntityDetailPageLayout>
  );
};
