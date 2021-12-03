import React from "react";
import { useFeatureGenerators } from "../../api-hooks/featuresApi";
import { deleteFeatureGeneratorAsync } from "../../api/featureGeneratorsApi";
import { BigPopup } from "../molecules/popups/BigPopup";
import { Spinner, ErrorCard } from "../molecules";
import { ConfirmDeletePopup } from "../molecules/popups/ConfirmDeletePopup";
import { CreateOrEditFilterSelectAggregateFeatureGenerator } from "./feature-generators/CreateFeatureGenerator";
import { useAccessToken } from "../../api-hooks/token";
import { SectionHeading } from "../molecules/layout";
import { EmptyList, EmptyStateText } from "../molecules/empty";
import { GeneratorDetail } from "./feature-generators/GeneratorDetail";
import { EntityRow } from "../molecules/layout/EntityRow";
import { DeleteButton } from "../molecules/buttons/DeleteButton";
import { DateTimeField } from "../molecules/DateTimeField";
// import { CreateButton } from "../molecules/CreateButton";

const GeneratorTableRow = ({ generator, onDeleted, requestReload }) => {
  const token = useAccessToken();
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [generatorDetailOpen, setGeneratorDetailOpen] = React.useState(false);
  const [error, setError] = React.useState(false);

  const handleDelete = () => {
    deleteFeatureGeneratorAsync({ token, id: generator.id })
      .then(() => {
        setDeleteOpen(false);
        onDeleted({});
      })
      .catch(setError);
  };
  return (
    <>
      <EntityRow>
        <div className="col">
          <button
            className="btn btn-link btn-block"
            onClick={() => setGeneratorDetailOpen(true)}
          >
            Generator for {generator.feature.name}
          </button>
          <div>
            {generator.lastCompleted && (
              <DateTimeField
                label="Last Completed"
                date={generator.lastCompleted}
              />
            )}
            {generator.lastEnqueued && (
              <DateTimeField
                label="Last Enqueued"
                date={generator.lastEnqueued}
              />
            )}
          </div>
        </div>

        <div className="col-2 text-right">
          <DeleteButton onClick={() => setDeleteOpen(true)} />
        </div>
      </EntityRow>
      <ConfirmDeletePopup
        entity={generator}
        open={deleteOpen}
        setOpen={setDeleteOpen}
        error={error}
        handleDelete={handleDelete}
      />
      <BigPopup isOpen={generatorDetailOpen} setIsOpen={setGeneratorDetailOpen}>
        {generator.loading && <Spinner />}
        {generator.id && (
          <GeneratorDetail
            generator={generator}
            requestClose={() => {
              requestReload();
              setGeneratorDetailOpen(false);
            }}
          />
        )}
      </BigPopup>
    </>
  );
};
export const FeatureGenerators = ({ feature }) => {
  const [trigger, setTrigger] = React.useState({});
  const generators = useFeatureGenerators({ trigger, id: feature.id });

  const [generatorPopupOpen, setGeneratorPopupOpen] = React.useState(false);

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

      <BigPopup isOpen={generatorPopupOpen} setIsOpen={setGeneratorPopupOpen}>
        <div style={{ minHeight: "65vh" }}>
          {feature.loading && <Spinner />}
          {!feature.loading && (
            <CreateOrEditFilterSelectAggregateFeatureGenerator
              feature={feature}
              onCreated={(r) => {
                setTrigger(r);
                setGeneratorPopupOpen(false);
              }}
            />
          )}
        </div>
      </BigPopup>
    </React.Fragment>
  );
};
