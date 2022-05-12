import React from "react";
import { useParams } from "react-router-dom";
import dayjs from "dayjs";

import { useMetric } from "../../api-hooks/metricsApi";
import { deleteMetricAsync, fetchExportCustomers } from "../../api/metricsApi";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import {
  PageHeading,
  Spinner,
  ErrorCard,
  AsyncButton,
  Navigation,
  MoveUpHierarchyPrimaryButton,
} from "../molecules";
import { CopyableField } from "../molecules/fields/CopyableField";
import { useAccessToken, useTokenScopes } from "../../api-hooks/token";
import { MetricDestinations } from "./MetricDestinations";

import { MetricGenerators } from "./MetricGenerators";
import { saveBlob } from "../../utility/utility";
import { TabActivator, Tabs } from "../molecules/layout/Tabs";
import MetricReports from "./MetricReports";
import { SectionHeading } from "../molecules/layout";
import { useNavigation } from "../../utility/useNavigation";
import EntityDetailPageLayout from "../molecules/layout/EntityDetailPageLayout";

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
  const { navigate } = useNavigation();
  const metric = useMetric({ id });

  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [deleteError, setDeleteError] = React.useState();
  const handleDelete = () => {
    deleteMetricAsync({ id, token })
      .then(() => navigate("/metrics"))
      .catch(setDeleteError);
  };

  const [isExportLoading, setIsExportLoading] = React.useState(false);
  const [exportError, setExportError] = React.useState();

  const scopes = useTokenScopes();
  const canWrite = scopes && scopes.find((_) => _ == "write:metrics");

  const exportTopCustomers = () => {
    setIsExportLoading(true);
    fetchExportCustomers({ token, id })
      .then(async (response) => {
        const blob = await response.blob();
        const dateFormat = dayjs().format("YYYYMMDD");
        const fileName =
          `${metric.name}_cherry_customer_exports_${dateFormat}.csv`.toLowerCase();
        saveBlob({ blob, name: fileName });
      })
      .catch((e) => {
        console.error(e);
        setExportError(e);
      })
      .finally(() => {
        setIsExportLoading(false);
      });
  };

  return (
    <EntityDetailPageLayout
      backButton={
        <MoveUpHierarchyPrimaryButton
          to={{ pathname: "/metrics/", search: null }}
        >
          Back to Metrics
        </MoveUpHierarchyPrimaryButton>
      }
      header={<PageHeading title={metric.name || "..."} subtitle="Metric" />}
      options={
        <>
          {canWrite && (
            <Navigation to={`/metrics/set-value/${id}`}>
              <button className="btn btn-primary mr-1">
                Manually Set a Metric Value
              </button>
            </Navigation>
          )}
          <AsyncButton
            loading={isExportLoading}
            className="btn btn-outline-primary mr-1"
            onClick={exportTopCustomers}
          >
            Export top customers
          </AsyncButton>
          <button
            onClick={() => setDeleteOpen(true)}
            className="btn btn-danger"
          >
            Delete Metric
          </button>
        </>
      }
    >
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
              {metric.scope === "customer" && (
                <CopyableField
                  label="Value Type"
                  value={metric.valueType ?? "Unset"}
                />
              )}
              <CopyableField label="Scope" value={metric.scope} />
            </>
          )}
          <ConfirmDeletePopup
            entity={metric}
            open={deleteOpen}
            setOpen={setDeleteOpen}
            error={deleteError}
            handleDelete={handleDelete}
          />
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
    </EntityDetailPageLayout>
  );
};

export default MetricDetail;
