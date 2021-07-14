import React from "react";
import { useHistory } from "react-router-dom";
import { useAccessToken } from "../../../api-hooks/token";
import { createFeatureAsync } from "../../../api/featuresApi";
import { BackButton, ErrorCard, Title } from "../../molecules";
import {
  TextInput,
  createLengthValidator,
  createServerErrorValidator,
} from "../../molecules/TextInput";
export const CreateFeature = () => {
  const token = useAccessToken();
  const history = useHistory();
  const [error, setError] = React.useState();
  const [feature, setFeature] = React.useState({
    commonId: "",
    name: "",
  });

  const handleCreate = () => {
    setError(null);
    createFeatureAsync({
      feature,
      token,
    })
      .then((r) => history.push(`/admin/features/detail/${r.id}`))
      .catch((e) => setError(e));
  };
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/admin/features">
        All Features
      </BackButton>
      <Title>Create Feature</Title>
      <hr />

      {error && <ErrorCard error={error} />}

      <div>
        <TextInput
          placeholder="Something human readable"
          value={feature.name}
          label="Friendly Name"
          validator={createLengthValidator(5)}
          onChange={(e) =>
            setFeature({
              ...feature,
              name: e.target.value,
            })
          }
        />
        <TextInput
          placeholder="Something unique"
          value={feature.commonId}
          label="Common Id"
          validator={createServerErrorValidator("CommonId", error)}
          onChange={(e) =>
            setFeature({
              ...feature,
              commonId: e.target.value,
            })
          }
        />

        <button className="btn btn-primary" onClick={handleCreate}>
          Create
        </button>
      </div>
    </React.Fragment>
  );
};
