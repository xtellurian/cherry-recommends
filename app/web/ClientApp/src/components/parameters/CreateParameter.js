import React from "react";
import { Subtitle, ErrorCard, AsyncButton } from "../molecules";
import { DropdownComponent, DropdownItem } from "../molecules/Dropdown";
import { createParameterAsync } from "../../api/parametersApi";
import { useAccessToken } from "../../api-hooks/token";
import {
  TextInput,
  createServerErrorValidator,
  joinValidators,
} from "../molecules/TextInput";

const parameterTypes = ["Numerical", "Categorical"];
export const CreateParameterPanel = ({ onCreated }) => {
  const token = useAccessToken();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const [parameter, setParameter] = React.useState({
    name: "",
    commonId: "",
    description: "",
    defaultValue: "",
    parameterType: parameterTypes[0],
  });

  const handleCreate = () => {
    setLoading(true);
    setError(null);
    createParameterAsync({
      payload: parameter,
      token,
    })
      .then((r) => {
        setParameter({
          name: "",
          commonId: "",
          description: "",
          defaultValue: "",
          parameterType: parameterTypes[0],
        });
        if (onCreated) {
          onCreated(r);
        }
      })
      .catch(setError)
      .finally(() => setLoading(false));
  };

  const commonIdValidator = joinValidators([
    createServerErrorValidator("CommonId", error),
    (val) => {
      if (val && val.length < 3) {
        return ["Common Id must be 3 or more characters"];
      }
    },
  ]);
  return (
    <React.Fragment>
      <Subtitle>Create Parameter</Subtitle>
      {error && <ErrorCard error={error} />}
      <div>
        <TextInput
          label="Name"
          placeholder="A human readable name"
          value={parameter.name}
          validator={createServerErrorValidator("Name", error)}
          onChange={(e) =>
            setParameter({
              ...parameter,
              name: e.target.value,
            })
          }
        />
        <TextInput
          label="Common Id"
          placeholder="An unique identifier"
          value={parameter.commonId}
          validator={commonIdValidator}
          onChange={(e) =>
            setParameter({
              ...parameter,
              commonId: e.target.value,
            })
          }
        />

        <div className="input-group m-2">
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
        <TextInput
          label="Default Value"
          placeholder="Value will be recommended as a backup"
          value={parameter.defaultValue}
          // validator={commonIdValidator}
          onChange={(e) =>
            setParameter({
              ...parameter,
              defaultValue: e.target.value,
            })
          }
        />

        <div className="text-center">
          <DropdownComponent
            title={parameter.parameterType}
            className="w-50 text-center"
          >
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

        <AsyncButton
          loading={loading}
          className="btn btn-primary btn-block mt-2"
          onClick={handleCreate}
        >
          Save
        </AsyncButton>
      </div>
    </React.Fragment>
  );
};
