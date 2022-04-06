import React from "react";
import { ErrorCard, AsyncButton } from "../molecules";
import { TextInput, InputGroup } from "../molecules/TextInput";
import { useAccessToken } from "../../api-hooks/token";
import { createModelRegistrationAsync } from "../../api/modelRegistrationsApi";
import { useNavigation } from "../../utility/useNavigation";

export const AzurePersonalizerModelRegistration = ({
  hostingType,
  modelType,
}) => {
  const { navigate } = useNavigation();
  const token = useAccessToken();
  const [error, setError] = React.useState();
  const [modelRegistration, setModelRegistration] = React.useState({
    name: "",
    scoringUrl: "",
    swaggerUrl: "",
    key: "",
  });

  const [loading, setLoading] = React.useState(false);
  const handleCreate = () => {
    setLoading(true);
    modelRegistration.hostingType = hostingType.value;
    modelRegistration.modelType = modelType.value;
    createModelRegistrationAsync({
      payload: modelRegistration,
      token,
    })
      .then((m) => navigate(`/models/test/${m.id}`))
      .catch(setError)
      .finally(() => setLoading(false));
  };

  return (
    <React.Fragment>
      {error && <ErrorCard error={error} />}

      <div className="mt-3">
        <InputGroup>
          <TextInput
            label="Model Name"
            placeholder="A name you recognise"
            value={modelRegistration.name}
            onChange={(e) =>
              setModelRegistration({
                ...modelRegistration,
                name: e.target.value,
              })
            }
          />
        </InputGroup>

        <InputGroup className="mt-1">
          <TextInput
            label="Scoring URL"
            placeholder="The Service Endpoint of the Personalizer"
            value={modelRegistration.scoringUrl}
            onChange={(e) =>
              setModelRegistration({
                ...modelRegistration,
                scoringUrl: e.target.value,
              })
            }
          />
          <TextInput
            label="API Key"
            placeholder="Personalizer Key"
            value={modelRegistration.key}
            onChange={(e) =>
              setModelRegistration({
                ...modelRegistration,
                key: e.target.value,
              })
            }
          />
        </InputGroup>

        <div className="mt-2">
          <AsyncButton loading={loading} onClick={handleCreate}>
            Create
          </AsyncButton>
        </div>
      </div>
    </React.Fragment>
  );
};
