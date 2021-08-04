import React from "react";
import { useHubspotCrmCardBehaviour } from "../../../../api-hooks/hubspotApi";
import { setHubspotCrmCardBehaviourAsync } from "../../../../api/hubspotApi";
import { useFeatures } from "../../../../api-hooks/featuresApi";
import {
  BackButton,
  Title,
  Subtitle,
  Selector,
  AsyncButton,
  ErrorCard,
} from "../../../molecules";
import { useAccessToken } from "../../../../api-hooks/token";

const Top = ({ integratedSystem }) => {
  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`/settings/integrations/detail/${integratedSystem.id}`}
      >
        Overview
      </BackButton>
      <Title> Hubspot CRM Card Behaviour</Title>
      <Subtitle>
        {integratedSystem.name || integratedSystem.commonId || "..."}
      </Subtitle>
      <hr />
    </React.Fragment>
  );
};

export const CrmCardBehaviour = ({ integratedSystem }) => {
  const token = useAccessToken();
  const [updateTrigger, setUpdateTrigger] = React.useState({});
  const behaviour = useHubspotCrmCardBehaviour({
    id: integratedSystem.id,
    trigger: updateTrigger,
  });

  const [featureOptions, setFeatureOptions] = React.useState([]);
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);

  const features = useFeatures();

  React.useEffect(() => {
    if (features.items) {
      setFeatureOptions(
        features.items.map((f) => ({ label: f.name, value: f }))
      );
    }
  }, [features]);

  React.useEffect(() => {
    if (behaviour.items) {
      setFeatureOptions(
        features.items.map((f) => ({ label: f.name, value: f }))
      );
    }
  }, [behaviour]);

  const [excludedFeatures, setExcludedFeatures] = React.useState([]);

  React.useEffect(() => {
    setExcludedFeatures(behaviour.excludedFeatures || []);
  }, [behaviour]);

  const [excludedFeatureOptions, setExcludedFeatureOptions] = React.useState(
    []
  );

  React.useEffect(() => {
    if (behaviour && behaviour.excludedFeatures) {
      setExcludedFeatureOptions(
        featureOptions.filter((_) =>
          behaviour.excludedFeatures.includes(_.value.commonId)
        )
      );
    }
  }, [behaviour, featureOptions]);

  const handleSave = () => {
    setLoading(true);
    setHubspotCrmCardBehaviourAsync({
      id: integratedSystem.id,
      token,
      behaviour: { excludedFeatures },
    })
      .then(setUpdateTrigger)
      .catch(setError)
      .finally(() => setLoading(false));
  };
  return (
    <React.Fragment>
      <Top integratedSystem={integratedSystem} />
      {error && <ErrorCard error={error} />}
     
      <h5>Exclude Features from Hubspot CRM Card.</h5>
      <p>
          By default, all Features for a user will be shown in a Hubspot CRM Card.
          Which Features would you like to exclude from the Card?
      </p>
      <div>
        <Selector
          isMulti
          isSearchable
          placeholder="Select features to exclude from CRM Cards"
          noOptionsMessage={({inputValue}) => `No Feature matches ${inputValue}`}
          defaultValue={excludedFeatureOptions}
          value={excludedFeatureOptions}
          onChange={(so) => {
            setExcludedFeatureOptions(so);
            setExcludedFeatures([...so.map((_) => _.value.commonId)]);
          }}
          options={featureOptions}
        />
      </div>
      <AsyncButton
        loading={loading}
        className="btn btn-primary btn-block"
        onClick={handleSave}
      >
        Save
      </AsyncButton>
    </React.Fragment>
  );
};
