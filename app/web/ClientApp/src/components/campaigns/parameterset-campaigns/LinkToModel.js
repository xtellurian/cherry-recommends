import React from "react";
import { useParams } from "react-router-dom";
import {
  useParameterSetCampaign,
  useLinkedRegisteredModel,
} from "../../../api-hooks/parameterSetCampaignsApi";
import { createLinkRegisteredModelAsync } from "../../../api/parameterSetCampaignsApi";
import { LinkToModelUtility } from "../utils/LinkToModel";
export const LinkToModel = () => {
  const { id } = useParams();
  const campaign = useParameterSetCampaign({ id });
  const linkedModel = useLinkedRegisteredModel({ id });
  return (
    <LinkToModelUtility
      recommender={campaign}
      createLinkRegisteredModelAsync={createLinkRegisteredModelAsync}
      rootPath="/campaigns/parameter-set-campaigns"
      linkedModel={linkedModel}
    />
  );
};
