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
import { TabActivator, Tabs } from "../molecules/layout/Tabs";
import MetricReports from "./MetricReports";
import { SectionHeading } from "../molecules/layout";

const tabs = [
  {
    id: "details",
    label: "Details",
  },
  {
    id: "generator",
    label: "Generator",
  },
  {
    id: "reports",
    label: "Reports",
  },
  {
    id: "destinations",
    label: "Destinations",
  },
];
const defaultTabId = tabs[0].id;

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
      <Tabs tabs={tabs} defaultTabId={tabs[0].id} />
      <TabActivator tabId={tabs[0].id} defaultTabId={defaultTabId}>
        <div className="mt-3 mb-2">
          <div className="mb-3">
            <SectionHeading>General Information</SectionHeading>
          </div>
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
          <div className="mt-2">
            <button
              onClick={() => setDeleteOpen(true)}
              className="btn btn-danger"
            >
              Delete Metric
            </button>
          </div>
        </div>
      </TabActivator>
      <TabActivator tabId={"generator"} defaultTabId={defaultTabId}>
        {metric.loading && <Spinner />}
        {metric.error && <ErrorCard error={metric.error} />}
        {metric && !metric.loading && <MetricGenerators metric={metric} />}
      </TabActivator>
      <TabActivator tabId={"reports"} defaultTabId={defaultTabId}>
        {metric.loading && <Spinner />}
        {metric.error && <ErrorCard error={metric.error} />}
        {metric && !metric.loading && <MetricReports metric={metric} />}
      </TabActivator>
      <TabActivator tabId={"destinations"} defaultTabId={defaultTabId}>
        {metric.loading && <Spinner />}
        {metric.error && <ErrorCard error={metric.error} />}
        {metric && !metric.loading && <MetricDestinations metric={metric} />}
      </TabActivator>
    </React.Fragment>
  );
};

export default MetricDetail;
