import React from "react";
import {
  AsyncButton,
  PageHeading,
  MoveUpHierarchyPrimaryButton,
  Selector,
} from "../molecules";
import { createParameterAsync } from "../../api/parametersApi";
import { useAccessToken } from "../../api-hooks/token";
import {
  TextInput,
  createServerErrorValidator,
  joinValidators,
  InputGroup,
} from "../molecules/TextInput";
import { useAnalytics } from "../../analytics/analyticsHooks";
import { useCommonId } from "../../utility/utility";
import CreatePageLayout from "../molecules/layout/CreatePageLayout";
import { useNavigation } from "../../utility/useNavigation";

const parameterTypeOptions = [
  { label: "Numerical", value: "Numerical" },
  { label: "Categorical", value: "Categorical" },
];

export const CreateParameter = () => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const { analytics } = useAnalytics();
  const { generateCommonId } = useCommonId();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);
  const [parameter, setParameter] = React.useState({
    name: "",
    commonId: "",
    description: "",
    defaultValue: "",
    parameterType: "",
  });

  const handleCreate = () => {
    setLoading(true);
    setError(null);
    createParameterAsync({
      payload: parameter,
      token,
    })
      .then((r) => {
        analytics.track("site:parameter_create_success");
        navigate("/parameters/parameters");
      })
      .catch((e) => {
        analytics.track("site:parameter_create_failure");
        setError(e);
      })
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

  React.useEffect(() => {
    setParameter({
      ...parameter,
      commonId: generateCommonId(parameter.name),
    });
  }, [parameter.name]);

  return (
    <CreatePageLayout
      createButton={
        <AsyncButton
          loading={loading}
          className="btn btn-primary btn-block mt-2"
          onClick={handleCreate}
        >
          Save
        </AsyncButton>
      }
      backButton={
        <MoveUpHierarchyPrimaryButton to="/parameters/parameters">
          Back to Parameters
        </MoveUpHierarchyPrimaryButton>
      }
      header={<PageHeading title="Create Parameter" />}
      error={error}
    >
      <div>
        <div className="mb-2">
          <Selector
            placeholder="Select a parameter type"
            onChange={(v) =>
              setParameter({
                ...parameter,
                parameterType: v.value,
              })
            }
            options={parameterTypeOptions}
          />
        </div>

        <InputGroup>
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
        </InputGroup>
        <InputGroup>
          <TextInput
            label="Common Id"
            placeholder="A unique identifier"
            value={parameter.commonId}
            validator={commonIdValidator}
            onChange={(e) =>
              setParameter({
                ...parameter,
                commonId: e.target.value,
              })
            }
            onHideErrors={() => setError(null)}
          />
        </InputGroup>

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
        <InputGroup>
          <TextInput
            label="Default Value"
            placeholder="Value will be recommended as a backup"
            value={parameter.defaultValue}
            onChange={(e) =>
              setParameter({
                ...parameter,
                defaultValue: e.target.value,
              })
            }
          />
        </InputGroup>
      </div>
    </CreatePageLayout>
  );
};
