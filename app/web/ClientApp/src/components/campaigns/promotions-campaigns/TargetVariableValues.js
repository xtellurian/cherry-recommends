import React from "react";
import { useParams } from "react-router-dom";
import { Spinner, ErrorCard } from "../../molecules";
import {
  usePromotionsCampaign,
  useTargetVariables,
} from "../../../api-hooks/promotionsCampaignsApi";

import { TargetVariableValuesUtility } from "../utils/TargetVariableValues";
export const TargetVariableValues = () => {
  const { id } = useParams();
  const recommender = usePromotionsCampaign({ id });
  const targetVariableValues = useTargetVariables({ id });
  if (targetVariableValues.loading) {
    return <Spinner>Loading Data</Spinner>;
  } else if (targetVariableValues.error) {
    return <ErrorCard error={targetVariableValues.error} />;
  }
  return (
    <TargetVariableValuesUtility
      recommender={recommender}
      rootPath="/campaigns/promotions-campaigns"
      targetVariableValues={targetVariableValues}
    />
  );
};
