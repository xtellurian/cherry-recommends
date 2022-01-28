import React from "react";
import { useAccessToken } from "../../../api-hooks/token";
import { createMetricGeneratorAsync } from "../../../api/metricGeneratorsApi";
import {
  Title,
  Subtitle,
  ErrorCard,
  EmptyState,
  Selector,
} from "../../molecules";
import { AsyncButton } from "../../molecules/AsyncButton";
import { LearnMore } from "../../molecules/help/LearnMore";
import { EntityRow } from "../../molecules/layout/EntityRow";
import {
  StatefulTabs,
  TabActivator,
} from "../../molecules/layout/StatefulTabs";

import {
  InputGroup,
  TextInput,
  createRequiredByServerValidator,
} from "../../molecules/TextInput";
import LearnMoreContent from "./LearnMoreAboutMetricGenerators";

const aggregationOptions = [
  { value: "Mean", label: "Average of all event property values" },
  {
    label: "Sum of all event property values",
    value: "Sum",
  },
];
const FilterStepSection = ({ step, addStep, setStep, removeStep, error }) => {
  return (
    <EntityRow>
      {!step && (
        <EmptyState>
          <div className="w-100">
            <button
              onClick={addStep}
              className="btn btn-outline-primary m-auto"
            >
              Add a Filter
            </button>
          </div>
        </EmptyState>
      )}
      {step && (
        <div className="w-100">
          <InputGroup>
            <TextInput
              label="Match event type"
              value={step.eventTypeMatch}
              onChange={(e) =>
                setStep({ ...step, eventTypeMatch: e.target.value })
              }
            />
            <button
              onClick={() => removeStep(step)}
              className="btn btn-outline-secondary"
            >
              Remove
            </button>
          </InputGroup>
        </div>
      )}
    </EntityRow>
  );
};
const SelectStepSection = ({ step, addStep, setStep, removeStep, error }) => {
  return (
    <EntityRow>
      {!step && (
        <EmptyState>
          <div className="w-100">
            <button
              onClick={addStep}
              className="btn btn-outline-primary m-auto"
            >
              Select a Property
            </button>
          </div>
        </EmptyState>
      )}
      {step && (
        <div className="w-100">
          <InputGroup className="">
            <TextInput
              label="Choose an event property to aggregate"
              validator={createRequiredByServerValidator(error)}
              value={step.propertyNameMatch}
              onChange={(e) =>
                setStep({ ...step, propertyNameMatch: e.target.value })
              }
            />
            <button
              onClick={() => removeStep(step)}
              className="btn btn-outline-secondary"
            >
              Remove
            </button>
          </InputGroup>
        </div>
      )}
    </EntityRow>
  );
};
const AggregateStepSection = ({
  step,
  addStep,
  setStep,
  removeStep,
  error,
}) => {
  return (
    <EntityRow>
      {!step && (
        <EmptyState>
          <div className="w-100">
            <button
              onClick={addStep}
              className="btn btn-outline-primary m-auto"
            >
              Select an Aggregation Type
            </button>
          </div>
        </EmptyState>
      )}
      {step && (
        <div className="w-100">
          <button
            onClick={() => removeStep(step)}
            className="btn btn-outline-secondary float-right"
            style={{ borderRadius: 0 }}
          >
            Remove
          </button>
          <Selector
            defaultValue={aggregationOptions[0]}
            options={aggregationOptions}
            onChange={(o) => setStep({ ...step, aggregationType: o.value })}
          />
        </div>
      )}
    </EntityRow>
  );
};

const tabs = [
  {
    id: "templates",
    label: "Templates",
  },
  {
    id: "custom",
    label: "Custom",
  },
];
export const CreateOrEditFilterSelectAggregateGenerator = ({
  metric,
  onCreated,
}) => {
  const token = useAccessToken();
  const [error, setError] = React.useState();
  const [loading, setLoading] = React.useState(false);

  const [generator, setGenerator] = React.useState({
    metricCommonId: metric.commonId,
    generatorType: "FilterSelectAggregate",
    steps: [
      // defaults
      { propertyNameMatch: "", type: "Select", order: 1, id: "default-select" },
      {
        aggregationType: aggregationOptions[0].value,
        type: "Aggregate",
        order: 2,
        id: "default-agg",
      },
    ],
  });

  const addStep = (step) => {
    step.id = Date.now();
    setGenerator({ ...generator, steps: [...generator.steps, step] });
  };
  const setStep = (step) => {
    let steps = generator.steps;
    steps = steps.filter((_) => _.id !== step.id);
    setGenerator({ ...generator, steps: [...steps, step] });
  };
  const removeStep = (step) => {
    let steps = generator.steps;
    steps = steps.filter((_) => _.id !== step.id);
    setGenerator({ ...generator, steps: [...steps] });
  };
  const filterStep = generator.steps.find((_) => _.type === "Filter");
  const selectStep = generator.steps.find((_) => _.type === "Select");
  const aggregateStep = generator.steps.find((_) => _.type === "Aggregate");

  const handleCreateCustom = () => {
    setLoading(true);
    createMetricGeneratorAsync({ token, payload: generator })
      .then(onCreated)
      .catch(setError)
      .finally(() => {
        setLoading(false);
      });
  };

  const handleCreateEventCountTemplate = () => {
    setLoading(true);
    createMetricGeneratorAsync({
      token,
      payload: {
        metricCommonId: metric.commonId,
        generatorType: "FilterSelectAggregate",
        steps: [
          {
            propertyNameMatch: null,
            type: "Select",
            order: 1,
          },
          {
            aggregationType: "Sum",
            type: "Aggregate",
            order: 2,
          },
        ],
      },
    })
      .then(onCreated)
      .catch(setError)
      .finally(() => {
        setLoading(false);
      });
  };

  const [currentTabId, setCurrentTabId] = React.useState(tabs[0].id);

  return (
    <React.Fragment>
      <Title>Metric Generation</Title>
      <LearnMore
        className="float-right"
        tooltip="Learn More about Metric Generators"
      >
        <LearnMoreContent />
      </LearnMore>
      <Subtitle>{metric.name}</Subtitle>
      <StatefulTabs
        currentTabId={currentTabId}
        setCurrentTabId={setCurrentTabId}
        tabs={tabs}
      />

      {error && <ErrorCard error={error} />}

      <TabActivator currentTabId={currentTabId} tabId="custom">
        <div className="text-muted">
          Generator Type: {generator.generatorType}
        </div>

        <div>
          <FilterStepSection
            step={filterStep}
            removeStep={removeStep}
            setStep={setStep}
            addStep={() =>
              addStep({ eventTypeMatch: "", type: "Filter", order: 0 })
            }
            error={error}
          />
        </div>
        <div>
          <SelectStepSection
            step={selectStep}
            removeStep={removeStep}
            setStep={setStep}
            addStep={() =>
              addStep({ propertyNameMatch: "", type: "Select", order: 1 })
            }
            error={error}
          />
        </div>
        <div>
          <AggregateStepSection
            step={aggregateStep}
            removeStep={removeStep}
            setStep={setStep}
            addStep={() =>
              addStep({ aggregationType: "", type: "Aggregate", order: 2 })
            }
            error={error}
          />
        </div>

        <div className="mt-3">
          <AsyncButton loading={loading} onClick={handleCreateCustom}>
            Create
          </AsyncButton>
        </div>
      </TabActivator>

      <TabActivator tabId="templates" currentTabId={currentTabId}>
        <EntityRow>
          <div className="col">
            <h5>Event Count Template</h5>
            <p>
              This template counts the total number of event from each Customer.
            </p>
          </div>
          <div className="col-4">
            <AsyncButton
              loading={loading}
              className="btn btn-outline-primary float-right"
              onClick={handleCreateEventCountTemplate}
            >
              Create From Template
            </AsyncButton>
          </div>
        </EntityRow>
      </TabActivator>
    </React.Fragment>
  );
};
