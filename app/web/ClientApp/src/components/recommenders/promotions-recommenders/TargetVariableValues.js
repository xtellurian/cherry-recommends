import React from "react";
import { useParams } from "react-router-dom";
import { Spinner, ErrorCard } from "../../molecules";
import {
  usePromotionsRecommender,
  useTargetVariables,
} from "../../../api-hooks/promotionsRecommendersApi";

import { TargetVariableValuesUtility } from "../utils/TargetVariableValues";
export const TargetVariableValues = () => {
  const { id } = useParams();
  const recommender = usePromotionsRecommender({ id });
  const targetVariableValues = useTargetVariables({ id });
  if (targetVariableValues.loading) {
    return <Spinner>Loading Data</Spinner>;
  } else if (targetVariableValues.error) {
    return <ErrorCard error={targetVariableValues.error} />;
  }
  return (
    <TargetVariableValuesUtility
      recommender={recommender}
      rootPath="/recommenders/promotions-recommenders"
      targetVariableValues={targetVariableValues}
    />
  );
};
