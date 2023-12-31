import React from "react";
import { useMetricGenerators } from "../../api-hooks/metricsApi";
import { deleteMetricGeneratorAsync } from "../../api/metricGeneratorsApi";
import { BigPopup } from "../molecules/popups/BigPopup";
import { Spinner, ErrorCard } from "../molecules";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import { CreateOrEditFilterSelectAggregateGenerator } from "./metric-generators/CreateCustomerScopeMetricGenerator";
import { useAccessToken } from "../../api-hooks/token";
import { SectionHeading } from "../molecules/layout";
import { EmptyList, EmptyStateText } from "../molecules/empty";
import { GeneratorDetail } from "./metric-generators/GeneratorDetail";
import EntityRow from "../molecules/layout/EntityFlexRow";
import { DeleteButton } from "../molecules/buttons/DeleteButton";
import { CreateGlobalScopeMetricGenerator } from "./metric-generators/CreateGlobalScopeMetricGenerator";
// import { CreateButton } from "../molecules/CreateButton";

const GeneratorTableRow = ({ generator, onDeleted, requestReload }) => {
  const token = useAccessToken();
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [error, setError] = React.useState(false);

  const handleDelete = () => {
    deleteMetricGeneratorAsync({ token, id: generator.id })
      .then(() => {
        setDeleteOpen(false);
        onDeleted({});
      })
      .catch(setError);
  };
  return (
    <>
      <EntityRow>
        <div className="flex-grow-1">
          <DeleteButton
            onClick={() => setDeleteOpen(true)}
            className="float-right"
          />

          <GeneratorDetail generator={generator} setTrigger={requestReload} />
        </div>
      </EntityRow>
      <ConfirmDeletePopup
        entity={generator}
        open={deleteOpen}
        setOpen={setDeleteOpen}
        error={error}
        handleDelete={handleDelete}
      />
    </>
  );
};
export const MetricGenerators = ({ metric }) => {
  const [trigger, setTrigger] = React.useState({});
  const generators = useMetricGenerators({ trigger, id: metric.id });

  const [generatorPopupOpen, setGeneratorPopupOpen] = React.useState(false);
  const isCustomerOrBusiness =
    metric.scope === "customer" || metric.scope === "business";

  return (
    <React.Fragment>
      <div className="mt-3 mb-2">
        <div className="mb-3">
          {/* Don't show create button - there should generally only be one */}
          {/* <CreateButton
            tooltip="Create a Generator"
            onClick={() => setGeneratorPopupOpen(true)}
          >
            Create a Generator
          </CreateButton> */}
          <SectionHeading>Generator</SectionHeading>
        </div>
        {generators.loading && <Spinner>Loading Generators</Spinner>}
        {generators.error && <ErrorCard error={generators.error} />}
        {generators.items && generators.items.length === 0 && (
          <EmptyList>
            <EmptyStateText>There are no generators</EmptyStateText>
            <button
              onClick={() => setGeneratorPopupOpen(true)}
              className="btn btn-outline-primary"
            >
              Create a Generator
            </button>
          </EmptyList>
        )}
        {generators.items &&
          generators.items.length > 0 &&
          generators.items.map((g) => (
            <GeneratorTableRow
              key={g.id}
              generator={g}
              onDeleted={setTrigger}
              requestReload={() => setTrigger({})}
            />
          ))}
      </div>

      <BigPopup
        isOpen={generatorPopupOpen}
        setIsOpen={setGeneratorPopupOpen}
        header="Define new Metric Generator"
        headerDivider
      >
        <div style={{ minHeight: "65vh" }}>
          {metric.loading && <Spinner />}
          {!metric.loading && isCustomerOrBusiness && (
            <CreateOrEditFilterSelectAggregateGenerator
              metric={metric}
              onCreated={setTrigger}
            />
          )}
          {!metric.loading && metric.scope == "global" && (
            <CreateGlobalScopeMetricGenerator
              metric={metric}
              onCreated={setTrigger}
            />
          )}
        </div>
      </BigPopup>
    </React.Fragment>
  );
};
