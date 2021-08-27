import React from "react";
import { AsyncButton, Title, ErrorCard, BackButton } from "../../molecules";
import { AsyncSelectActionName } from "../../molecules/selectors/AsyncSelectActionName";
import { Selector } from "../../molecules/Select";
import { SettingRow } from "../../molecules/SettingRow";
import { createRewardSelectorAsync } from "../../../api/rewardSelectorsApi";
import { useAccessToken } from "../../../api-hooks/token";
import { useHistory } from "react-router-dom";

const rewardTypeOptions = [{ label: "Revenue", value: "Revenue" }];

export const CreateRewardSelector = () => {
  const token = useAccessToken();
  const history = useHistory();
  const [actionName, setActionName] = React.useState("");
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState();
  const [selectorType, setSelectorType] = React.useState(rewardTypeOptions[0]);
  const selector = {
    selectorType: rewardTypeOptions[0].value,
    actionName,
  };

  const handleCreate = () => {
    setLoading(true);
    selector.actionName = actionName;
    selector.selectorType = selectorType.value;
    createRewardSelectorAsync({ token, entity: selector })
      .then((v) => history.push(`/settings/rewards/reward-selector/${v.id}`))
      .catch(setError)
      .finally(() => setLoading(false));
  };
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/settings/rewards">
        All Rewards
      </BackButton>
      <Title>Create Reward</Title>
      <hr />
      {error && <ErrorCard error={error} />}
      <SettingRow
        label="Reward Type"
        description="What kind of reward is this? Currently, only revenue is supported."
      >
        <Selector
          isDisabled={true}
          placeholder="Reward Type"
          value={selectorType}
          defaultValue={selectorType}
          onChange={setSelectorType}
          options={rewardTypeOptions}
        />
      </SettingRow>
      <SettingRow
        label="Reward Property"
        description={`Which user or event property represents ${selector.selectorType}?`}
      >
        <AsyncSelectActionName
          onChange={(v) => setActionName(v.value)}
          placeholder={`Choose the property for ${selector.selectorType}`}
        />
      </SettingRow>

      <AsyncButton loading={loading} onClick={handleCreate}>
        Create
      </AsyncButton>
    </React.Fragment>
  );
};
