import React from "react";
import { useHistory, useParams } from "react-router-dom";
import { updateMergePropertiesAsync } from "../../api/trackedUsersApi";
import { useTrackedUser } from "../../api-hooks/trackedUserApi";
import {
  Title,
  Subtitle,
  BackButton,
  AsyncButton,
  ErrorCard,
  Spinner
} from "../molecules";
import { PropertiesEditor } from "../molecules/PropertiesEditor";
import { useAccessToken } from "../../api-hooks/token";

export const EditProperties = () => {
  const { id } = useParams();
  const history = useHistory();
  const token = useAccessToken();
  const trackedUser = useTrackedUser({ id });
  const [saving, setSaving] = React.useState(false);
  const [error, setError] = React.useState();
  const [properties, setProperties] = React.useState(trackedUser.properties);
  const handleSave = () => {
    setError(null);
    setSaving(true);
    updateMergePropertiesAsync({ token, id, properties })
      .then(() => history.push(`/tracked-users/detail/${id}`))
      .catch(setError)
      .finally(() => setSaving(false));
  };
  React.useEffect(() => {
    if (trackedUser.properties) {
      setProperties(trackedUser.properties);
    }
  }, [trackedUser]);
  return (
    <React.Fragment>
      <BackButton to={`/tracked-users/detail/${id}`} className="float-right">
        Back
      </BackButton>
      <Title>{trackedUser.name || trackedUser.commonId}</Title>
      <Subtitle>Edit Properties</Subtitle>
      <hr />
      {trackedUser.loading && <Spinner>Loading Properties</Spinner>}
      {error && <ErrorCard error={error} />}
      {properties && (
        <PropertiesEditor
          initialProperties={properties}
          onPropertiesChanged={setProperties}
          placeholder={`${trackedUser.name || trackedUser.commonId} Properties`}
        />
      )}
      <AsyncButton
        className="mt-3 btn btn-primary btn-block"
        loading={saving}
        onClick={handleSave}
      >
        Save
      </AsyncButton>
    </React.Fragment>
  );
};
