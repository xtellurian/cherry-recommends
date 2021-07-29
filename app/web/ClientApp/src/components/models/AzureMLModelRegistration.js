import React from "react";
import { useHistory } from "react-router-dom";
import { ErrorCard, AsyncButton } from "../molecules";
import { useAccessToken } from "../../api-hooks/token";
import { createModelRegistrationAsync } from "../../api/modelRegistrationsApi";

export const AzureMLModelRegistration = ({ hostingType, modelType }) => {
  const history = useHistory();
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
      .then((m) => history.push(`/models/test/${m.id}`))
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
        <input
          type="text"
          className="form-control"
          placeholder="Model Name"
          value={modelRegistration.name}
          onChange={(e) =>
            setModelRegistration({
              ...modelRegistration,
              name: e.target.value,
            })
          }
        />

        <div className="input-group">
          <div className="w-75">
            <label className="form-label">Scoring URL</label>
            <input
              type="text"
              className="form-control"
              placeholder="https://model-url.com/score"
              value={modelRegistration.scoringUrl}
              onChange={(e) =>
                setModelRegistration({
                  ...modelRegistration,
                  scoringUrl: e.target.value,
                })
              }
            />
          </div>
          <div>
            <label className="form-label">Key</label>
            <input
              type="text"
              className="form-control"
              placeholder="Secret Key"
              value={modelRegistration.key}
              onChange={(e) =>
                setModelRegistration({
                  ...modelRegistration,
                  key: e.target.value,
                })
              }
            />
          </div>
        </div>
        <div className="input-group">
          <div className="w-50">
            <label className="form-label">Swagger URL</label>
            <input
              type="text"
              className="form-control"
              placeholder="https://model-url/com/swagger.json"
              value={modelRegistration.swaggerUrl}
              onChange={(e) =>
                setModelRegistration({
                  ...modelRegistration,
                  swaggerUrl: e.target.value,
                })
              }
            />
          </div>
        </div>

        <div className="mt-2">
          <AsyncButton loading={loading} onClick={handleCreate}>
            Create
          </AsyncButton>
        </div>
      </div>
    </React.Fragment>
  );
};
