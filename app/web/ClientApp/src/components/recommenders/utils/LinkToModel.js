import React from "react";
import { useHistory, Link } from "react-router-dom";
import { useModelRegistrations } from "../../../api-hooks/modelRegistrationsApi";
import { BackButton } from "../../molecules/BackButton";
import { Title, Spinner, ErrorCard, Selector } from "../../molecules";
import { NoteBox } from "../../molecules/NoteBox";
import { CopyableField } from "../../molecules/CopyableField";
import { useAccessToken } from "../../../api-hooks/token";

const LinkedModelInfo = ({ linkedModel }) => {
  return (
    <div className="card">
      <div className="card-header">Currently Linked</div>
      <div className="card-body">
        <CopyableField label="Model Name" value={linkedModel.name} />
        <Link to={`/models/test/${linkedModel.id}`}>
          <button className="btn btn-secondary float-right">Test Model</button>
        </Link>
      </div>
    </div>
  );
};
export const LinkToModelUtility = ({
  recommender,
  linkedModel,
  createLinkRegisteredModel,
  rootPath,
}) => {
  const history = useHistory();
  const token = useAccessToken();

  const [error, setError] = React.useState();
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
      success: () => history.push(`${rootPath}/detail/${recommender.id}`),
      error: setError,
      token,
      id: recommender.id,
      modelId: selectedModel.id,
    });
  };
  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`${rootPath}/detail/${recommender.id}`}
      >
        Back to Recommender
      </BackButton>
      <Title>Link to Model</Title>
      <hr />
      <NoteBox className="m-auto w-50" label="Warning">
        This area is for administrators only.
      </NoteBox>
      {error && <ErrorCard error={error} />}
      {(linkedModel.loading || recommender.loading) && <Spinner />}

      {linkedModel.error && <div className="text-muted">No Model Linked</div>}
      {!linkedModel.error && !linkedModel.loading && (
        <LinkedModelInfo linkedModel={linkedModel} />
      )}
      <Selector
        isSearchable
        placeholder="Select a registered model to link to this recommender"
        noOptionsMessage={(inputValue) => "No Models Available"}
        onChange={(so) => setSelectedModel(so.value)}
        options={modelRegistrationOptions}
      />
      <div className="text-center m-3">
        <button onClick={handleLink} className="btn btn-primary w-50">
          Link Model
        </button>
      </div>
    </React.Fragment>
  );
};
