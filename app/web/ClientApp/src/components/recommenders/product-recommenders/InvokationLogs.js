import React from "react";
import { useParams } from "react-router-dom";
import {
  useProductRecommender,
  useInvokationLogs,
} from "../../../api-hooks/productRecommendersApi";
import { Spinner } from "../../molecules";
import { InvokationLogsUtil } from "../utils/InvokationLogsUtil";
export const InvokationLogs = () => {
  const { id } = useParams();
  const recommender = useProductRecommender({ id });
  const invokations = useInvokationLogs({ id });

  if (recommender.loading || invokations.loading) {
    return <Spinner />;
  } else
    return (
      <InvokationLogsUtil
        invokations={invokations}
        recommender={recommender}
        rootPath="/recommenders/product-recommenders"
      />
    );
};
