import React from "react";
import { useParams } from "react-router";
import { PrimaryBackButton } from "../../molecules/BackButton";
import { Title, Subtitle } from "../../molecules";
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
      <PrimaryBackButton to={"/recommenders/parameter-set-recommenders"}>
        Back to Recommenders
      </PrimaryBackButton>
      <Title>{recommender.name || "..."}</Title>
      <Subtitle>Parameter Set Recommender</Subtitle>
      <ParameterSetRecommenderPrimaryNav id={id} />
      {children}
    </>
  );
};
