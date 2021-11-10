import React from "react";
import { Link, useHistory, useParams } from "react-router-dom";
import { useFeature } from "../../api-hooks/featuresApi";
import { deleteFeatureAsync } from "../../api/featuresApi";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import { Title, Subtitle, Spinner, ErrorCard, BackButton } from "../molecules";
import { CopyableField } from "../molecules/fields/CopyableField";
import { useAccessToken } from "../../api-hooks/token";
import { FeatureDestinations } from "./FeatureDestinations";
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
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/features">
        All Features
      </BackButton>
      <Link to={`/features/set-value/${id}`}>
        <button className="btn btn-primary float-right">
          Set Value for Tracked User
        </button>
      </Link>
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
      <FeatureDestinations feature={feature} />
      <button onClick={() => setDeleteOpen(true)} className="btn btn-danger">
        Delete Feature
      </button>
    </React.Fragment>
  );
};
