import React from "react";
import { Title } from "../molecules/PageHeadings";
import { useAccessToken } from "../../api-hooks/token";
import { createModelRegistration } from "../../api/modelRegistrationsApi";
import { DropdownComponent, DropdownItem } from "../molecules/Dropdown";

const modelTypes = ["SingleClassClassifier"];
const hostingTypes = ["AzureMLContainerInstance"];

export const CreateModelRegistration = () => {
  const token = useAccessToken();
  const [modelRegistration, setModelRegistration] = React.useState({
    name: "",
    scoringUrl: "",
    swaggerUrl: "",
    key: "",
    hostingType: hostingTypes[0],
    modelType: modelTypes[0],
  });

  return (
    <React.Fragment>
      <Title>Register Model</Title>
      <div>
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
        <div className="input-group">
          <div className="m-1">
            <label className="form-label">Model Type</label>
            <DropdownComponent title={modelRegistration.modelType}>
              {modelTypes.map((c) => (
                <DropdownItem key={c}>
                  <div
                    onClick={() =>
                      setModelRegistration({
                        ...modelRegistration,
                        modelType: c,
                      })
                    }
                  >
                    {c}
                  </div>
                </DropdownItem>
              ))}
            </DropdownComponent>
          </div>
          <div>
            <div className="m-1">
              <label className="form-label">Hosting Type</label>
              <DropdownComponent title={modelRegistration.hostingType}>
                {hostingTypes.map((c) => (
                  <DropdownItem key={c}>
                    <div
                      onClick={() =>
                        setModelRegistration({
                          ...modelRegistration,
                          hostingType: c,
                        })
                      }
                    >
                      {c}
                    </div>
                  </DropdownItem>
                ))}
              </DropdownComponent>
            </div>
          </div>
        </div>
        <div className="mt-2">
          <button
            className="btn btn-primary"
            onClick={() => {
              createModelRegistration({
                payload: modelRegistration,
                success: () => alert("Registered New Model"),
                error: () => alert("Something broke."),
                token,
              });
            }}
          >
            Create
          </button>
        </div>
      </div>
    </React.Fragment>
  );
};
