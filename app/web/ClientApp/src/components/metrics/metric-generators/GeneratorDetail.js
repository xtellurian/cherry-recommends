import React from "react";
import { useAccessToken } from "../../../api-hooks/token";
import { manualTriggerMetricGeneratorsAsync } from "../../../api/metricGeneratorsApi";
import { AsyncButton, ErrorCard, Navigation } from "../../molecules";
import { ButtonGroup } from "../../molecules/buttons/ButtonGroup";
import { SectionHeading } from "../../molecules/layout";
import EntityRow from "../../molecules/layout/EntityFlexRow";
import { DateTimeField } from "../../molecules/DateTimeField";

const GeneratorTypeDisplay = ({ generator }) => {
  const labels = {
    joinTwoMetrics: "Ratio of Metrics",
  };
  return (
    <>
      {generator.generatorType && <div>{labels[generator.generatorType]}</div>}
    </>
  );
};
const TimeWindowDisplay = ({ generator }) => {
  if (generator.metric?.scope !== "customer") {
    return <></>;
  }
  const timeWindowLabels = {
    sevenDays: "7 Days",
    thirtyDays: "30 Days",
  };

  const timeWindow = timeWindowLabels[generator.timeWindow] || "All Time";
  return (
    <>
      <div>{timeWindow}</div>
    </>
  );
};

const StepRow = ({ step }) => {
  return (
    <EntityRow>
      <div className="col-2">{step.order}</div>
      {step.filter && (
        <div className="col">Filter on {step.filter.eventTypeMatch}</div>
      )}
      {step.select && (
        <div className="col">
          {step.select.propertyNameMatch &&
            `Select property ${step.select.propertyNameMatch}`}
          {step.select.propertyNameMatch === null && "Counting each event"}
        </div>
      )}
      {step.aggregate && (
        <div className="col">Aggregate by {step.aggregate.aggregationType}</div>
      )}
    </EntityRow>
  );
};

const AggregateCustomerMetricInfo = ({ aggregateCustomerMetric }) => {
  return (
    <>
      <EntityRow>
        <div>Aggregating another metric</div>
        <div>
          Calculating the {aggregateCustomerMetric.aggregationType} of{" "}
          <Navigation
            to={`/metrics/detail/${aggregateCustomerMetric.metricId}`}
          >
            {aggregateCustomerMetric.metric?.name}
          </Navigation>
          {aggregateCustomerMetric.categoricalValue &&
            `, by counting the value '${aggregateCustomerMetric.categoricalValue}'`}
        </div>
      </EntityRow>
    </>
  );
};
const JoinTwoMetrics = ({ joinTwoMetrics }) => {
  return (
    <>
      <EntityRow>
        <div>Joining 2 Metrics</div>
        <div>
          Dividing{" "}
          <Navigation to={`/metrics/detail/${joinTwoMetrics.metric1Id}`}>
            {joinTwoMetrics.metric1?.name}
          </Navigation>
          {" by "}
          <Navigation to={`/metrics/detail/${joinTwoMetrics.metric2Id}`}>
            {joinTwoMetrics.metric2?.name}
          </Navigation>
        </div>
      </EntityRow>
    </>
  );
};
export const GeneratorDetail = ({ generator, requestClose }) => {
  const [error, setError] = React.useState();
  const [running, setRunning] = React.useState(false);
  const token = useAccessToken();
  const handleTrigger = () => {
    setRunning(true);
    setError(null);
    manualTriggerMetricGeneratorsAsync({
      token,
      id: generator.id,
    })
      .then(requestClose)
      .catch(setError)
      .finally(() => setRunning(false));
  };

  return (
    <>
      <div>
        <SectionHeading>Generator Information</SectionHeading>
        <div className="text-muted">Generator ID: {generator.id}</div>
        <GeneratorTypeDisplay generator={generator} />
        <TimeWindowDisplay generator={generator} />
        <hr />
        {error && <ErrorCard error={error} />}
        {generator.generatorType === "filterSelectAggregate" &&
          generator.filterSelectAggregateSteps
            .sort((a, b) => a.order - b.order)
            .map((s) => <StepRow key={s.order} step={s} />)}

        {generator.generatorType === "aggregateCustomerMetric" && (
          <AggregateCustomerMetricInfo
            aggregateCustomerMetric={generator.aggregateCustomerMetric}
          />
        )}
        {generator.generatorType === "joinTwoMetrics" && (
          <JoinTwoMetrics joinTwoMetrics={generator.joinTwoMetrics} />
        )}

        <hr />
        <div>
          {generator.lastEnqueued ? (
            <DateTimeField
              label="Last Enqueued"
              date={generator.lastEnqueued}
            />
          ) : (
            <div>Generator has never been enqueued.</div>
          )}
          {generator.lastCompleted ? (
            <DateTimeField
              label="Last Completed"
              date={generator.lastCompleted}
            />
          ) : (
            <div>Generator has never completed.</div>
          )}
        </div>
        <div className="text-right mt-3">
          <ButtonGroup>
            <AsyncButton
              className="btn btn-primary"
              onClick={handleTrigger}
              loading={running}
            >
              Trigger Now
            </AsyncButton>
            <button
              className="btn btn-outline-secondary"
              onClick={requestClose}
            >
              Cancel
            </button>
          </ButtonGroup>
        </div>
      </div>
    </>
  );
};
