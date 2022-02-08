import React from "react";
import { Link, useHistory, useParams } from "react-router-dom";
import { useMetric } from "../../api-hooks/metricsApi";
import { deleteMetricAsync } from "../../api/metricsApi";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";

import { Title, Subtitle, Spinner, ErrorCard, BackButton } from "../molecules";
import { CopyableField } from "../molecules/fields/CopyableField";
import { useAccessToken, useTokenScopes } from "../../api-hooks/token";
import { MetricDestinations } from "./MetricDestinations";

import { MetricGenerators } from "./MetricGenerators";

const MetricDetail = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const history = useHistory();
  const metric = useMetric({ id });

  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [deleteError, setDeleteError] = React.useState();
  const handleDelete = () => {
    deleteMetricAsync({ id, token })
      .then(() => history.push("/metrics"))
      .catch(setDeleteError);
  };

  const scopes = useTokenScopes();
  const canWrite = scopes && scopes.find((_) => _ == "write:metrics");
  return (
    <React.Fragment>
      <BackButton className="float-right" to="/metrics/">
        All Metrics
      </BackButton>
      {canWrite && (
        <Link to={`/metrics/set-value/${id}`}>
          <button className="btn btn-primary float-right">
            Manually Set a Metric Value
          </button>
        </Link>
      )}
      <Title>Metric</Title>
      <Subtitle>{metric.name ? metric.name : "..."}</Subtitle>
      <hr />
      {metric.loading && <Spinner />}
      {metric.error && <ErrorCard error={metric.error} />}
      {metric.commonId && (
        <>
          <CopyableField label="Common Id" value={metric.commonId} />
          <CopyableField
            label="Value Type"
            value={metric.valueType ?? "Unset"}
          />
        </>
      )}
      <ConfirmDeletePopup
        entity={metric}
        open={deleteOpen}
        setOpen={setDeleteOpen}
        error={deleteError}
        handleDelete={handleDelete}
      />
      <div className="row">
        <div className="col">
          <MetricGenerators metric={metric} />
        </div>
        <div className="col">
          <MetricDestinations metric={metric} />
        </div>
      </div>
      <div className="mt-2">
        <button onClick={() => setDeleteOpen(true)} className="btn btn-danger">
          Delete Metric
        </button>
      </div>
    </React.Fragment>
  );
};

export default MetricDetail;
