import React from "react";
import { useParams } from "react-router-dom";
import { Spinner, ErrorCard } from "../../molecules";
import {
  useParameterSetRecommender,
  useTargetVariables,
} from "../../../api-hooks/parameterSetRecommendersApi";

import { TargetVariableValuesUtility } from "../utils/TargetVariableValues";
export const TargetVariableValues = () => {
  const { id } = useParams();
  const recommender = useParameterSetRecommender({ id });
  const targetVariableValues = useTargetVariables({ id });
  if (targetVariableValues.loading) {
    return <Spinner>Loading Data</Spinner>;
  } else if (targetVariableValues.error) {
    return <ErrorCard error={targetVariableValues.error} />;
  }
  console.log(targetVariableValues);
  return (
    <TargetVariableValuesUtility
      recommender={recommender}
      rootPath="/recommenders/parameter-set-recommenders"
      targetVariableValues={targetVariableValues}
    />
  );
};
