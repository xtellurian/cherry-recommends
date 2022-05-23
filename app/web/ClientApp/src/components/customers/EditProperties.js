import React from "react";
import { useParams } from "react-router-dom";
import { updateMergePropertiesAsync } from "../../api/customersApi";
import { useCustomer } from "../../api-hooks/customersApi";
import {
  Title,
  Subtitle,
  AsyncButton,
  ErrorCard,
  Spinner,
  MoveUpHierarchyButton,
} from "../molecules";
import { PropertiesEditor } from "../molecules/PropertiesEditor";
import { useAccessToken } from "../../api-hooks/token";
import { useNavigation } from "../../utility/useNavigation";

export const EditProperties = () => {
  const { id } = useParams();
  const { navigate } = useNavigation();
  const token = useAccessToken();
  const trackedUser = useCustomer({ id });
  const [saving, setSaving] = React.useState(false);
  const [error, setError] = React.useState();
  const [properties, setProperties] = React.useState(trackedUser.properties);
  const handleSave = () => {
    setError(null);
    setSaving(true);
    updateMergePropertiesAsync({ token, id, properties })
      .then(() => navigate(`/customers/customers/detail/${id}`))
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
      <MoveUpHierarchyButton
        to={`/customers/customers/detail/${id}`}
        className="float-right"
      >
        Back
      </MoveUpHierarchyButton>
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
