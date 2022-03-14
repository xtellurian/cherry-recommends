import React from "react";
import { useParams } from "react-router-dom";
import { useBusiness } from "../../api-hooks/businessesApi";
import {
  useBusinessMetrics,
  useBusinessMetric,
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

const MetricValues = ({ businessId, metric }) => {
  const values = useBusinessMetric({
    id: businessId,
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
const MetricValuesRow = ({ businessId, metric }) => {
  return (
    <ExpandableCard label={metric.name}>
      <MetricValues businessId={businessId} metric={metric} />
    </ExpandableCard>
  );
};

export const BusinessMetrics = () => {
  const { id } = useParams();
  const business = useBusiness({ id });
  const businessMetrics = useBusinessMetrics({ id });
  return (
    <React.Fragment>
      <BackButton className="float-right" to={`/businesses/detail/${id}`}>
        Business Details
      </BackButton>
      <Title>Metrics</Title>
      <Subtitle>
        {business.name || business.commonId || business.id || "..."}
      </Subtitle>
      <hr />
      {(business.loading || businessMetrics.loading) && <Spinner />}
      {businessMetrics.length > 0 &&
        businessMetrics.map((f) => (
          <MetricValuesRow metric={f} businessId={id} key={f.id} />
        ))}
      {businessMetrics.length === 0 && (
        <EmptyList>This business has no metrics.</EmptyList>
      )}
    </React.Fragment>
  );
};
