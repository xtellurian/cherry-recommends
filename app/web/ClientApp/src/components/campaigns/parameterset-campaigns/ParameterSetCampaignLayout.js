import React from "react";
import { useParams } from "react-router";

import { Title, Subtitle, MoveUpHierarchyPrimaryButton } from "../../molecules";
import { ParameterSetCampaignPrimaryNav } from "./ParameterSetCampaignPrimaryNav";
import { useParameterSetCampaign } from "../../../api-hooks/parameterSetCampaignsApi";
import { CampaignStatusBox } from "../../molecules/CampaignStatusBox";
import { setSettingsAsync } from "../../../api/parameterSetCampaignsApi";
import { useAccessToken } from "../../../api-hooks/token";

export const ParameterSetCampaignLayout = ({ children }) => {
  const [trigger, setTrigger] = React.useState({});
  const { id } = useParams();
  const token = useAccessToken();
  const recommender = useParameterSetCampaign({ id, trigger });
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
      <CampaignStatusBox
        className="float-right"
        recommender={recommender}
        setEnabled={setEnabled}
      />
      <MoveUpHierarchyPrimaryButton to="/campaigns/parameter-set-campaigns">
        Back to Campaigns
      </MoveUpHierarchyPrimaryButton>
      <Title>{recommender.name || "..."}</Title>
      <Subtitle>Parameter Set Campaign</Subtitle>
      <ParameterSetCampaignPrimaryNav id={id} />
      {children}
    </>
  );
};
