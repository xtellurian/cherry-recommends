import React from "react";
import { useHistory } from "react-router";
import { useAccessToken } from "../../../api-hooks/token";
import { createEnvironmentAsync } from "../../../api/environmentsApi";
import { Title, AsyncButton, BackButton } from "../../molecules";
import {
  TextInput,
  InputGroup,
  createRequiredByServerValidator,
  createLengthValidator,
  joinValidators,
} from "../../molecules/TextInput";

export const CreateEnvironment = () => {
  const history = useHistory();
  const token = useAccessToken();
  const [loading, setLoading] = React.useState(false);
  const [error, setError] = React.useState();
  const [name, setName] = React.useState("");
  const handleCreate = () => {
    setLoading(true);
    createEnvironmentAsync({
      token,
      environment: {
        name,
      },
    })
      .then(() => {
        history.push("/settings/environments");
        return setTimeout(() => window.location.reload(), 50);
      })
      .catch(setError);
  };
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/settings/environments">
        Back
      </BackButton>
      <Title>Create New Environment</Title>

      <hr />
      <InputGroup>
        <TextInput
          label="Environment Name"
          placeholder="e.g. Dev, Test, Production"
          validator={joinValidators([
            createLengthValidator(3),
            createRequiredByServerValidator(error),
          ])}
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
        <AsyncButton
          loading={loading}
          onClick={handleCreate}
          className="btn btn-primary w-25"
        >
          Create
        </AsyncButton>
      </InputGroup>
    </React.Fragment>
  );
};
