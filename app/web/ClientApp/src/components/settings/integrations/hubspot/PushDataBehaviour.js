import React from "react";

import {
  ErrorCard,
  Spinner,
  Title,
  Subtitle,
  AsyncButton,
  MoveUpHierarchyButton,
} from "../../../molecules";
import { AsyncSelectPromotionsCampaign } from "../../../molecules/selectors/AsyncSelectPromotionsCampaign";
import { useHubspotPushBehaviourAsync } from "../../../../api-hooks/hubspotApi";
import { setHubspotPushBehaviourAsync } from "../../../../api/hubspotApi";
import { useAccessToken } from "../../../../api-hooks/token";
import { SettingRow } from "../../../molecules/layout/SettingRow";

const Top = ({ integratedSystem }) => {
  return (
    <React.Fragment>
      <MoveUpHierarchyButton
        className="float-right"
        to={`/settings/integrations/detail/${integratedSystem.id}`}
      >
        Overview
      </MoveUpHierarchyButton>
      <Title> Data Push Behaviour</Title>
      <Subtitle>
        {integratedSystem.name || integratedSystem.commonId || "..."}
      </Subtitle>
      <hr />
    </React.Fragment>
  );
};

export const PushDataBehaviour = ({ integratedSystem }) => {
  const [loading, setLoading] = React.useState(false);
  const token = useAccessToken();
  const [updateTrigger, setUpdateTrigger] = React.useState({});
  const [error, setError] = React.useState();
  const behaviour = useHubspotPushBehaviourAsync({
    id: integratedSystem.id,
    trigger: updateTrigger,
  });

  const [recommenderIds, setRecommenderIds] = React.useState([]);
  const handleSelectItemsRecommenders = (v) => {
    setRecommenderIds(v.map((_) => _.value.id));
  };

  const handleSave = () => {
    setLoading(true);
    setHubspotPushBehaviourAsync({
      token,
      id: integratedSystem.id,
      behaviour: {
        recommenderIds,
      },
    })
      .then(setUpdateTrigger)
      .catch(setError)
      .finally(() => setLoading(false));
  };

  if (behaviour.loading) {
    return (
      <React.Fragment>
        <Top integratedSystem={integratedSystem} />
        <Spinner>Loading Behaviour</Spinner>
      </React.Fragment>
    );
  }
  if (loading) {
    return (
      <React.Fragment>
        <Top integratedSystem={integratedSystem} />
        <Spinner>Loading</Spinner>
        {error && <ErrorCard error={error} />}
      </React.Fragment>
    );
  }

  return (
    <React.Fragment>
      <Top integratedSystem={integratedSystem} />
      <div>
        {error && <ErrorCard error={error} />}
        <SettingRow
          label="Recommendations"
          description="Which campaigns should be connected to properties in Hubspot?"
        >
          <div>
            <AsyncSelectPromotionsCampaign
              defaultIds={behaviour.recommenderIds}
              isMulti={true}
              onChange={handleSelectItemsRecommenders}
              placeholder="Select a Promotion Campaign"
            />
          </div>
        </SettingRow>
      </div>
      <div>
        <AsyncButton
          className="btn btn-primary btn-block"
          loading={loading}
          onClick={handleSave}
        >
          Save
        </AsyncButton>
      </div>
    </React.Fragment>
  );
};
