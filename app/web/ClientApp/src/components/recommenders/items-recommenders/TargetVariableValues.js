import React from "react";
import { useParams } from "react-router-dom";
import { Spinner, ErrorCard } from "../../molecules";
import {
  useItemsRecommender,
  useTargetVariables,
} from "../../../api-hooks/itemsRecommendersApi";

import { TargetVariableValuesUtility } from "../utils/TargetVariableValues";
export const TargetVariableValues = () => {
  const { id } = useParams();
  const recommender = useItemsRecommender({ id });
  const targetVariableValues = useTargetVariables({ id });
  if (targetVariableValues.loading) {
    return <Spinner>Loading Data</Spinner>;
  } else if (targetVariableValues.error) {
    return <ErrorCard error={targetVariableValues.error} />;
  }
  return (
    <TargetVariableValuesUtility
      recommender={recommender}
      rootPath="/recommenders/items-recommenders"
      targetVariableValues={targetVariableValues}
    />
  );
};
