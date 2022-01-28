import React from "react";
import { useParams } from "react-router-dom";
import { useCustomer } from "../../api-hooks/customersApi";
import {
  useCustomerMetrics,
  useCustomersMetrics,
} from "../../api-hooks/metricsApi";
import {
  Title,
  Subtitle,
  Spinner,
  ExpandableCard,
  EmptyList,
  BackButton,
  ErrorCard,
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
    return (
      <div>
        <CopyableField label="Metric Common Id" value={metric.commonId} />
        <CopyableField label="Current Version" value={values.version} />
        <CopyableField label="Current Value" value={`${values.value}`} />
        <CopyableField label="Value Type" value={valueType} />
      </div>
    );
  }
};
const MetricValuesRow = ({ customerId, metric }) => {
  return (
    <ExpandableCard label={metric.name}>
      <MetricValues customerId={customerId} metric={metric} />
    </ExpandableCard>
  );
};

const Metrics = () => {
  const { id } = useParams();
  const trackedUser = useCustomer({ id });
  const customerMetrics = useCustomerMetrics({ id });
  return (
    <React.Fragment>
      <BackButton className="float-right" to={`/customers/detail/${id}`}>
        User Details
      </BackButton>
      <Title>Metrics</Title>
      <Subtitle>
        {trackedUser.name || trackedUser.commonId || trackedUser.id || "..."}
      </Subtitle>
      <hr />
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