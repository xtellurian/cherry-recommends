import React from "react";
import { useParams } from "react-router-dom";
import {
  usePromotionsCampaign,
  useInvokationLogs,
} from "../../../api-hooks/promotionsCampaignsApi";
import { Spinner } from "../../molecules";
import { InvokationLogsUtil } from "../utils/InvokationLogsUtil";
export const InvokationLogs = ({ trigger }) => {
  const { id } = useParams();
  const recommender = usePromotionsCampaign({ id });
  const invokations = useInvokationLogs({ id, pageSize: 5, trigger });

  if (recommender.loading || invokations.loading) {
    return <Spinner />;
  } else
    return (
      <InvokationLogsUtil
        invokations={invokations}
        recommender={recommender}
        rootPath="/campaigns/promotions-campaigns"
      />
    );
};
