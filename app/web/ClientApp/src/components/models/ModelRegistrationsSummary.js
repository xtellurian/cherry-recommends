import React from "react";
import { Link, useRouteMatch } from "react-router-dom";
import { useModelRegistrations } from "../../api-hooks/modelRegistrationsApi";
import { Title } from "../molecules/PageHeadings";
import { JsonView } from "../molecules/JsonView";
import { ExpandableCard } from "../molecules/ExpandableCard";
import { Spinner } from "../molecules/Spinner";
import { CreateButton } from "../molecules/CreateButton";
import { Paginator } from "../molecules/Paginator";

const ModelRow = ({ model }) => {
  return (
    <ExpandableCard name={model.name}>
      <Link to={`/models/invoke/${model.id}`} className="float-right">
        <button className="btn btn-secondary">Test</button>
      </Link>
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
  const { result } = useModelRegistrations();
  return (
    <React.Fragment>
      <div>
        <CreateButton to={`${path}/create`} className="float-right">
          Register New Model
        </CreateButton>

        <Title>Models</Title>
      </div>
      <hr />
      <div>
        {result &&
          result.items.length > 0 &&
          result.items.map((m) => <ModelRow key={m.id} model={m} />)}
      </div>
      <div>
        {result && result.items.length === 0 && (
          <div className="text-center p-5">
            <div>There are no models registered.</div>
            <CreateButton to={`${path}/create`} className="mt-4">
              Create New Model
            </CreateButton>
          </div>
        )}
      </div>
      {!result && <Spinner />}
      {result && <Paginator {...result.pagination} />}
    </React.Fragment>
  );
};
