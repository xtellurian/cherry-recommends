import React from "react";
import { useHistory, useParams } from "react-router-dom";
import { updateBusinessPropertiesAsync } from "../../api/businessesApi";
import { useBusiness } from "../../api-hooks/businessesApi";
import {
  Title,
  Subtitle,
  BackButton,
  AsyncButton,
  ErrorCard,
  Spinner,
} from "../molecules";
import { PropertiesEditor } from "../molecules/PropertiesEditor";
import { useAccessToken } from "../../api-hooks/token";

export const EditBusinessProperties = () => {
  const { id } = useParams();
  const history = useHistory();
  const token = useAccessToken();
  const business = useBusiness({ id });
  const [saving, setSaving] = React.useState(false);
  const [error, setError] = React.useState();
  const [properties, setProperties] = React.useState(business.properties);

  const handleSave = () => {
    setError(null);
    setSaving(true);
    updateBusinessPropertiesAsync({ token, id, properties })
      .then(() => history.push(`/businesses/detail/${id}`))
      .catch(setError)
      .finally(() => setSaving(false));
  };

  React.useEffect(() => {
    if (business.properties) {
      setProperties(business.properties);
    }
  }, [business]);

  return (
    <React.Fragment>
      <BackButton to={`/businesses/detail/${id}`} className="float-right">
        Back
      </BackButton>
      <Title>{business.name || business.commonId}</Title>
      <Subtitle>Edit Properties</Subtitle>
      <hr />
      {business.loading && <Spinner>Loading Properties</Spinner>}
      {error && <ErrorCard error={error} />}
      {properties && (
        <PropertiesEditor
          initialProperties={properties}
          onPropertiesChanged={setProperties}
          placeholder={`${business.name || business.commonId} Properties`}
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
