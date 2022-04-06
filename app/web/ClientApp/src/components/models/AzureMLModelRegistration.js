import React from "react";
import { ErrorCard, AsyncButton } from "../molecules";
import {
  TextInput,
  InputGroup,
  createServerErrorValidator,
} from "../molecules/TextInput";
import { useAccessToken } from "../../api-hooks/token";
import { createModelRegistrationAsync } from "../../api/modelRegistrationsApi";
import { useNavigation } from "../../utility/useNavigation";

export const AzureMLModelRegistration = ({ hostingType, modelType }) => {
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
        <label className="form-label">
          Register a new model that will be available via the API.
        </label>
        <InputGroup>
          <TextInput
            placeholder="Model Name"
            value={modelRegistration.name}
            validator={createServerErrorValidator("Name", error)}
            onChange={(e) =>
              setModelRegistration({
                ...modelRegistration,
                name: e.target.value,
              })
            }
            resetTrigger={error}
          />
        </InputGroup>

        <label>Location and credentials</label>

        <InputGroup>
          <TextInput
            placeholder="https://model-url.com/score"
            value={modelRegistration.scoringUrl}
            onChange={(e) =>
              setModelRegistration({
                ...modelRegistration,
                scoringUrl: e.target.value,
              })
            }
            validator={createServerErrorValidator("ScoringUrl", error)}
            resetTrigger={error}
          />

          <TextInput
            placeholder="Secret Key"
            value={modelRegistration.key}
            onChange={(e) =>
              setModelRegistration({
                ...modelRegistration,
                key: e.target.value,
              })
            }
            validator={createServerErrorValidator("Key", error)}
            resetTrigger={error}
          />
        </InputGroup>

        <label className="form-label">Swagger URL (Optional)</label>
        <InputGroup>
          <TextInput
            placeholder="https://model-url/com/swagger.json"
            value={modelRegistration.swaggerUrl}
            onChange={(e) =>
              setModelRegistration({
                ...modelRegistration,
                swaggerUrl: e.target.value,
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
