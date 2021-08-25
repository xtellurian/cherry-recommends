import React from "react";
import { useHistory } from "react-router-dom";
import { useAccessToken } from "../../../api-hooks/token";
import { createFeatureGeneratorAsync } from "../../../api/featureGeneratorsApi";
import {
  Title,
  Subtitle,
  ErrorCard,
  Spinner,
  BackButton,
} from "../../molecules";
import { AsyncButton } from "../../molecules/AsyncButton";
import { AsyncSelectFeature } from "../../molecules/selectors/AsyncSelectFeature";
export const CreateFeatureGenerator = () => {
  const token = useAccessToken();
  const history = useHistory();
  const [error, setError] = React.useState();
  const [selectedFeature, setSelectedFeature] = React.useState();
  const [loading, setLoading] = React.useState(false);

  const [generator, setGenerator] = React.useState({
    featureId: null,
    generatorType: "MonthsSinceEarliestEvent",
  });
  const handleCreate = () => {
    setLoading(true);
    generator.featureCommonId = selectedFeature.value.commonId;
    createFeatureGeneratorAsync({ token, payload: generator })
      .then(() => history.push("/admin/feature-generators"))
      .catch(setError)
      .finally(() => setLoading(false));
  };
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/admin/feature-generators">
        All Generators
      </BackButton>
      <Title>Create Feature Generator</Title>
      <Subtitle>Feature Generators run periodically</Subtitle>
      <hr />
      {error && <ErrorCard error={error} />}
      <div>Generator Type: {generator.generatorType}</div>
      <AsyncSelectFeature
        placeholder="Select a feature to generate values for"
        onChange={setSelectedFeature}
      />

      <div className="mt-3">
        <AsyncButton loading={loading} onClick={handleCreate}>
          Create
        </AsyncButton>
      </div>
    </React.Fragment>
  );
};
