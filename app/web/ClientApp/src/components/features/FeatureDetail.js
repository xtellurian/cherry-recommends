import React from "react";
import { Link, useHistory, useParams } from "react-router-dom";
import { useFeature } from "../../api-hooks/featuresApi";
import { deleteFeatureAsync } from "../../api/featuresApi";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";

import { Title, Subtitle, Spinner, ErrorCard, BackButton } from "../molecules";
import { CopyableField } from "../molecules/fields/CopyableField";
import { useAccessToken, useTokenScopes } from "../../api-hooks/token";
import { FeatureDestinations } from "./FeatureDestinations";

import { FeatureGenerators } from "./FeatureGenerators";

export const FeatureDetail = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const history = useHistory();
  const feature = useFeature({ id });

  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [deleteError, setDeleteError] = React.useState();
  const handleDelete = () => {
    deleteFeatureAsync({ id, token })
      .then(() => history.push("/features"))
      .catch(setDeleteError);
  };

  const scopes = useTokenScopes();
  const canWriteFeatures = scopes && scopes.find((_) => _ == "write:features");
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/features">
        All Features
      </BackButton>
      {canWriteFeatures && (
        <Link to={`/features/set-value/${id}`}>
          <button className="btn btn-primary float-right">
            Manually Set a Feature Value
          </button>
        </Link>
      )}
      <Title>Feature</Title>
      <Subtitle>{feature.name ? feature.name : "..."}</Subtitle>
      <hr />
      {feature.loading && <Spinner />}
      {feature.error && <ErrorCard error={feature.error} />}
      {feature.commonId && (
        <CopyableField label="Common Id" value={feature.commonId} />
      )}
      <ConfirmDeletePopup
        entity={feature}
        open={deleteOpen}
        setOpen={setDeleteOpen}
        error={deleteError}
        handleDelete={handleDelete}
      />
      <div className="row">
        <div className="col">
          <FeatureGenerators feature={feature} />
        </div>
        <div className="col">
          <FeatureDestinations feature={feature} />
        </div>
      </div>
      <div className="mt-2">
        <button onClick={() => setDeleteOpen(true)} className="btn btn-danger">
          Delete Feature
        </button>
      </div>
    </React.Fragment>
  );
};
