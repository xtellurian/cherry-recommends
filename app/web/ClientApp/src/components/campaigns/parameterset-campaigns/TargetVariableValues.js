import React from "react";
import { useParams } from "react-router-dom";
import { Spinner, ErrorCard } from "../../molecules";
import {
  useParameterSetCampaign,
  useTargetVariables,
} from "../../../api-hooks/parameterSetCampaignsApi";

import { TargetVariableValuesUtility } from "../utils/TargetVariableValues";
export const TargetVariableValues = () => {
  const { id } = useParams();
  const recommender = useParameterSetCampaign({ id });
  const targetVariableValues = useTargetVariables({ id });
  if (targetVariableValues.loading) {
    return <Spinner>Loading Data</Spinner>;
  } else if (targetVariableValues.error) {
    return <ErrorCard error={targetVariableValues.error} />;
  }
  console.info(targetVariableValues);
  return (
    <TargetVariableValuesUtility
      recommender={recommender}
      rootPath="/campaigns/parameter-set-campaigns"
      targetVariableValues={targetVariableValues}
    />
  );
};
