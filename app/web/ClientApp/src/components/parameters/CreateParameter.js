import React from "react";
import { Subtitle } from "../molecules/PageHeadings";
import { DropdownComponent, DropdownItem } from "../molecules/Dropdown";
import { ErrorCard } from "../molecules/ErrorCard";
import { createParameter } from "../../api/parametersApi";
import { useAccessToken } from "../../api-hooks/token";

const parameterTypes = ["Numerical", "Categorical"];
export const CreateParameterPanel = ({ onCreated }) => {
  const token = useAccessToken();
  const [error, setError] = React.useState();
  const [parameter, setParameter] = React.useState({
    name: "",
    commonId: "",
    description: "",
    parameterType: parameterTypes[0],
  });
  const handleCreate = () => {
    setError(null);
    createParameter({
      payload: parameter,
      success: (r) => {
        setParameter({
          name: "",
          commonId: "",
          description: "",
          parameterType: parameterTypes[0],
        });
        if (onCreated) {
          onCreated(r);
        }
      },
      error: setError,
      token,
    });
  };
  return (
    <React.Fragment>
      <Subtitle>Create Parameter</Subtitle>
      {error && <ErrorCard error={error} />}
      <div>
        <div className="input-group m-1">
          <div className="input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              Friendly Name
            </span>
          </div>
          <input
            type="text"
            className="form-control"
            placeholder="Name"
            value={parameter.name}
            onChange={(e) =>
              setParameter({
                ...parameter,
                name: e.target.value,
              })
            }
          />
        </div>
        <div className="input-group m-1">
          <div className="input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              Identity
            </span>
          </div>
          <input
            type="text"
            className="form-control"
            placeholder="Unique Id"
            value={parameter.commonId}
            onChange={(e) =>
              setParameter({
                ...parameter,
                commonId: e.target.value,
              })
            }
          />
        </div>
        <div className="input-group m-1">
          <textarea
            className="form-control"
            placeholder="What is this parameter? Where is it used?"
            value={parameter.description}
            onChange={(e) =>
              setParameter({
                ...parameter,
                description: e.target.value,
              })
            }
          />
        </div>
        <div className="m-1 text-right">
          <DropdownComponent title={parameter.parameterType}>
            {parameterTypes.map((t) => (
              <DropdownItem key={t}>
                <div
                  onClick={() =>
                    setParameter({
                      ...parameter,
                      parameterType: t,
                    })
                  }
                >
                  {t}
                </div>
              </DropdownItem>
            ))}
          </DropdownComponent>
        </div>

        <button className="btn btn-primary w-100" onClick={handleCreate}>
          Save
        </button>
      </div>
    </React.Fragment>
  );
};
