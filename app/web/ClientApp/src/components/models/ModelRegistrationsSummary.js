import React from "react";
import { useRouteMatch } from "react-router-dom";
import { useModelRegistrations } from "../../api-hooks/modelRegistrationsApi";
import { deleteModelRegistrationAsync } from "../../api/modelRegistrationsApi";
import { Title } from "../molecules/layout";
import { JsonView } from "../molecules/JsonView";
import { CreateButtonClassic } from "../molecules/CreateButton";
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
  let { path } = useRouteMatch();
  const [trigger, setTrigger] = React.useState({});
  const result = useModelRegistrations({ trigger });
  return (
    <React.Fragment>
      <div>
        <CreateButtonClassic to={`${path}/create`} className="float-right">
          Register New Model
        </CreateButtonClassic>

        <Title>Models</Title>
      </div>
      <hr />
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
            <div>There are no models registered.</div>
            <CreateButtonClassic to={`${path}/create`} className="mt-4">
              Create New Model
            </CreateButtonClassic>
          </EmptyList>
        )}
      </div>
      {result.loading && <Spinner />}
      {result.pagination && <Paginator {...result.pagination} />}
    </React.Fragment>
  );
};
