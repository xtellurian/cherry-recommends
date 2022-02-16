import React from "react";
import { useAccessToken } from "../../../api-hooks/token";
import { manualTriggerMetricGeneratorsAsync } from "../../../api/metricGeneratorsApi";
import { AsyncButton, ErrorCard } from "../../molecules";
import { ButtonGroup } from "../../molecules/buttons/ButtonGroup";
import { SectionHeading } from "../../molecules/layout";
import { EntityRow } from "../../molecules/layout/EntityRow";
import { DateTimeField } from "../../molecules/DateTimeField";
import { CopyableField } from "../../molecules/fields/CopyableField";

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
  let timeWindow = "All Time";
  if (generator.timeWindow === "sevenDays")
    timeWindow = "7 Days";
  else if (generator.timeWindow === "thirtyDays")
    timeWindow = "30 Days";

  return (
    <>
      <div>
        <SectionHeading>Generator Information</SectionHeading>
        <div className="text-muted">Generators are run once per day.</div>
        {error && <ErrorCard error={error} />}
        {generator.filterSelectAggregateSteps
          .sort((a, b) => a.order - b.order)
          .map((s) => (
            <StepRow key={s.order} step={s} />
          ))}
        <CopyableField label="Time Window" value={timeWindow} />
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
