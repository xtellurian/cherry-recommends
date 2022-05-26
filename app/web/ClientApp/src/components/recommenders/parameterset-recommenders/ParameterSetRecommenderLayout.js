import React from "react";
import { useParams } from "react-router";

import { Title, Subtitle, MoveUpHierarchyPrimaryButton } from "../../molecules";
import { ParameterSetRecommenderPrimaryNav } from "./ParameterSetRecommenderPrimaryNav";
import { useParameterSetRecommender } from "../../../api-hooks/parameterSetRecommendersApi";
import { RecommenderStatusBox } from "../../molecules/RecommenderStatusBox";
import { setSettingsAsync } from "../../../api/parameterSetRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";

export const ParameterSetRecommenderLayout = ({ children }) => {
  const [trigger, setTrigger] = React.useState({});
  const { id } = useParams();
  const token = useAccessToken();
  const recommender = useParameterSetRecommender({ id, trigger });
  const setEnabled = (value) => {
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
  return (
    <>
      <RecommenderStatusBox
        className="float-right"
        recommender={recommender}
        setEnabled={setEnabled}
      />
      <MoveUpHierarchyPrimaryButton to="/recommenders/parameter-set-recommenders">
        Back to Campaigns
      </MoveUpHierarchyPrimaryButton>
      <Title>{recommender.name || "..."}</Title>
      <Subtitle>Parameter Set Campaign</Subtitle>
      <ParameterSetRecommenderPrimaryNav id={id} />
      {children}
    </>
  );
};
