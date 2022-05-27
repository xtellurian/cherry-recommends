import React from "react";
import { useParams } from "react-router-dom";
import {
  useParameterSetCampaign,
  useInvokationLogs,
} from "../../../api-hooks/parameterSetCampaignsApi";
import { Spinner } from "../../molecules";
import { InvokationLogsUtil } from "../utils/InvokationLogsUtil";
export const InvokationLogs = () => {
  const { id } = useParams();
  const recommender = useParameterSetCampaign({ id });
  const invokations = useInvokationLogs({ id });

  if (recommender.loading || invokations.loading) {
    return <Spinner />;
  } else
    return (
      <InvokationLogsUtil
        invokations={invokations}
        recommender={recommender}
        rootPath="/campaigns/parameter-set-campaigns"
      />
    );
};
