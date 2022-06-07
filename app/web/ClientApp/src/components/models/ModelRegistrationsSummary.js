import React from "react";
import { useModelRegistrations } from "../../api-hooks/modelRegistrationsApi";
import { deleteModelRegistrationAsync } from "../../api/modelRegistrationsApi";
import { JsonView } from "../molecules/JsonView";
import {
  EmptyList,
  Paginator,
  Spinner,
  ExpandableCard,
  ErrorCard,
} from "../molecules";
import {
  ActionsButton,
  ActionItem,
  ActionItemsGroup,
} from "../molecules/buttons/ActionsButton";
import { ConfirmationPopup } from "../molecules/popups/ConfirmationPopup";
import { useAccessToken } from "../../api-hooks/token";
import Layout, {
  CreateEntityButton,
} from "../molecules/layout/EntitySummaryLayout";

const ModelRow = ({ model, onDeleted }) => {
  const token = useAccessToken();
  const [open, setOpen] = React.useState(false);
  const [error, setError] = React.useState();
  return (
    <ExpandableCard label={model.name}>
      <ActionsButton
        label="Test"
        to={`/admin/models/test/${model.id}`}
        className="float-right"
      >
        <ActionItemsGroup label="Actions">
          <ActionItem onClick={() => setOpen(true)}>Delete</ActionItem>
          <ConfirmationPopup
            isOpen={open}
            setIsOpen={setOpen}
            label="Are you sure you want to delete this model?"
          >
            <div className="m-2">{model.name}</div>
            {error && <ErrorCard error={error} />}
            <div
              className="btn-group"
              role="group"
              aria-label="Delete or cancel buttons"
            >
              <button
                className="btn btn-secondary"
                onClick={() => setOpen(false)}
              >
                Cancel
              </button>
              <button
                className="btn btn-danger"
                onClick={() => {
                  deleteModelRegistrationAsync({
                    success: () => {
                      if (onDeleted) {
                        onDeleted();
                      }
                      setOpen(false);
                    },
                    error: setError,
                    token,
                    id: model.id,
                  }).then(() => {
                    if (onDeleted) {
                      onDeleted();
                    }
                    setOpen(false);
                  });
                }}
              >
                Delete
              </button>
            </div>
          </ConfirmationPopup>
        </ActionItemsGroup>
      </ActionsButton>

      <div>
        <div>Scoring URL: {model.scoringUrl}</div>
        <div>
          <JsonView data={model} />
        </div>
      </div>
    </ExpandableCard>
  );
};
export const ModelRegistrationsSummary = () => {
  const [trigger, setTrigger] = React.useState({});
  const result = useModelRegistrations({ trigger });
  return (
    <Layout
      header="Custom Models"
      createButton={
        <CreateEntityButton to="/admin/models/create">
          Create a Custom Model
        </CreateEntityButton>
      }
    >
      <div>
        {result &&
          result.items &&
          result.items.map((m) => (
            <ModelRow key={m.id} model={m} onDeleted={() => setTrigger({})} />
          ))}
      </div>
      <div>
        {result.items && result.items.length === 0 && (
          <EmptyList>
            <div className="mb-4">There are no models registered.</div>
            <CreateEntityButton to="/admin/models/create">
              Create New Model
            </CreateEntityButton>
          </EmptyList>
        )}
      </div>
      {result.loading && <Spinner />}
      {result.pagination && <Paginator {...result.pagination} />}
    </Layout>
  );
};
