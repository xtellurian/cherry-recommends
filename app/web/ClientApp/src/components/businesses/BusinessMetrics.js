import React from "react";
import { useParams } from "react-router-dom";
import { useBusiness } from "../../api-hooks/businessesApi";
import {
  useBusinessMetrics,
  useBusinessMetric,
} from "../../api-hooks/metricsApi";
import {
  Spinner,
  ExpandableCard,
  EmptyList,
  ErrorCard,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
} from "../molecules";
import { CopyableField } from "../molecules/fields/CopyableField";
import { toDate } from "../../utility/utility";

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
const MetricValuesRow = ({ businessId, metric }) => {
  return (
    <div className="mb-2">
      <ExpandableCard label={metric.name}>
        <MetricValues businessId={businessId} metric={metric} />
      </ExpandableCard>
    </div>
  );
};

export const BusinessMetrics = () => {
  const { id } = useParams();
  const business = useBusiness({ id });
  const businessMetrics = useBusinessMetrics({ id });
  return (
    <React.Fragment>
      <MoveUpHierarchyPrimaryButton to={`/businesses/detail/${id}`}>
        Back to Business Details
      </MoveUpHierarchyPrimaryButton>
      <PageHeading
        title={business.name || business.commonId || business.id || "..."}
        subtitle="Metrics"
      />

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
