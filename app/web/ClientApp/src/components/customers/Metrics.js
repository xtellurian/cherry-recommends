import React from "react";
import { useParams } from "react-router-dom";
import { useCustomer } from "../../api-hooks/customersApi";
import {
  useCustomerMetrics,
  useCustomersMetrics,
} from "../../api-hooks/metricsApi";
import { toDate } from "../../utility/utility";
import {
  Spinner,
  ExpandableCard,
  EmptyList,
  ErrorCard,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
} from "../molecules";
import { CopyableField } from "../molecules/fields/CopyableField";

const MetricValues = ({ customerId, metric }) => {
  const values = useCustomersMetrics({
    id: customerId,
    metricId: metric.commonId,
  });
  if (values.loading) {
    return <Spinner>Loading Metric Value</Spinner>;
  } else if (values.error) {
    return <ErrorCard error={values.error} />;
  } else {
    const valueType = values.numericValue ? "Numeric" : "String";
    const dateValue = toDate(values.created);
    return (
      <div>
        <CopyableField label="Metric Common Id" value={metric.commonId} />
        <CopyableField
          label="Date Created"
          value={dateValue.toLocaleString()}
          tooltip="Date created in local timezone"
        />
        <CopyableField label="Current Version" value={values.version} />
        <CopyableField label="Current Value" value={`${values.value}`} />
        <CopyableField label="Value Type" value={valueType} />
      </div>
    );
  }
};
const MetricValuesRow = ({ customerId, metric }) => {
  return (
    <div className="mb-2">
      <ExpandableCard label={metric.name}>
        <MetricValues customerId={customerId} metric={metric} />
      </ExpandableCard>
    </div>
  );
};

const Metrics = () => {
  const { id } = useParams();
  const trackedUser = useCustomer({ id });
  const customerMetrics = useCustomerMetrics({ id });
  return (
    <React.Fragment>
      <MoveUpHierarchyPrimaryButton to={`/customers/detail/${id}`}>
        Back to User Details
      </MoveUpHierarchyPrimaryButton>
      <PageHeading
        title={
          trackedUser.name || trackedUser.commonId || trackedUser.id || "..."
        }
        subtitle="Metrics"
      />
      {(trackedUser.loading || customerMetrics.loading) && <Spinner />}
      {customerMetrics.length > 0 &&
        customerMetrics.map((f) => (
          <MetricValuesRow metric={f} customerId={id} key={f.id} />
        ))}
      {customerMetrics.length === 0 && (
        <EmptyList>This user has no metrics.</EmptyList>
      )}
    </React.Fragment>
  );
};

export default Metrics;
