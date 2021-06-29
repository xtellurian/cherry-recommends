import React from "react";
import { useParams, useHistory, Link } from "react-router-dom";
import {
  useParameterSetRecommender,
  useLinkedRegisteredModel,
} from "../../../api-hooks/parameterSetRecommendersApi";
import { createLinkRegisteredModel } from "../../../api/parameterSetRecommendersApi";
import { useModelRegistrations } from "../../../api-hooks/modelRegistrationsApi";
import { BackButton } from "../../molecules/BackButton";
import { Title, Spinner, ErrorCard, Selector } from "../../molecules";
import { CopyableField } from "../../molecules/CopyableField";
import { useAccessToken } from "../../../api-hooks/token";

const LinkedModelInfo = ({ linkedModel }) => {
  return (
    <div className="card">
      <div className="card-header">Currently Linked</div>
      <div className="card-body">
        <CopyableField label="Model Name" value={linkedModel.name} />
        <Link to={`/models/test/${linkedModel.id}`}>
          <button className="btn btn-secondary float-right">
              Test Model
          </button>
        </Link>
      </div>
    </div>
  );
};
export const LinkToModel = () => {
  const { id } = useParams();
  const history = useHistory();
  const token = useAccessToken();
  const parameterSetRecommender = useParameterSetRecommender({ id });
  const [error, setError] = React.useState();
  const linkedModel = useLinkedRegisteredModel({ id });
  const modelRegistrations = useModelRegistrations();
  const [modelRegistrationOptions, setModelRegistrationOptions] =
    React.useState();

  React.useEffect(() => {
    if (modelRegistrations.items) {
      setModelRegistrationOptions(
        modelRegistrations.items.map((m) => ({ value: m, label: m.name }))
      );
    }
  }, [modelRegistrations]);

  const [selectedModel, setSelectedModel] = React.useState();

  const handleLink = () => {
    createLinkRegisteredModel({
      success: () =>
        history.push(`/recommenders/parameter-set-recommenders/detail/${id}`),
      error: setError,
      token,
      id,
      modelId: selectedModel.id,
    });
  };
  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`/recommenders/parameter-set-recommenders/detail/${id}`}
      >
        Back to Recommender
      </BackButton>
      <Title>Link to Model</Title>
      <hr />
      {error && <ErrorCard error={error} />}
      {(linkedModel.loading || parameterSetRecommender.loading) && <Spinner />}

      {linkedModel.error && <div className="text-muted">No Model Linked</div>}
      {!linkedModel.error && !linkedModel.loading && (
        <LinkedModelInfo linkedModel={linkedModel} />
      )}
      <Selector
        isSearchable
        placeholder="Select Model"
        noOptionsMessage={(inputValue) => "No Models Available"}
        onChange={(so) => setSelectedModel(so.value)}
        options={modelRegistrationOptions}
      />
      <button onClick={handleLink} className="btn btn-primary">
        Link Model
      </button>
    </React.Fragment>
  );
};
