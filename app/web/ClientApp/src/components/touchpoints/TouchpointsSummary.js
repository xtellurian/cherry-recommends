import React from "react";
import { useTouchpoints } from "../../api-hooks/touchpointsApi";
import { ActionsButton } from "../molecules/buttons/ActionsButton";
import { Title, EmptyList, Spinner } from "../molecules";
import { CreateButtonClassic } from "../molecules/CreateButton";
import { Paginator } from "../molecules/Paginator";

const TouchpointRow = ({ touchpoint }) => {
  return (
    <div className="card">
      <div className="row card-body">
        <div className="col">{touchpoint.commonId}</div>
        <div className="col">
          <ActionsButton
            className="float-right"
            to={`/touchpoints/users-in-touchpoint/${touchpoint.id}`}
            label="Tracked Users"
          />
          {touchpoint.name}
        </div>
      </div>
    </div>
  );
};
const Top = () => {
  return (
    <React.Fragment>
      <CreateButtonClassic className="float-right" to="/touchpoints/create">
        Create a Touchpoint
      </CreateButtonClassic>
      <Title>Touchpoints</Title>
      <hr />
    </React.Fragment>
  );
};
export const TouchpointsSummary = () => {
  const touchpoints = useTouchpoints();
  if (touchpoints.loading) {
    return (
      <React.Fragment>
        <Top />
        <Spinner>Loading Touchpoints</Spinner>
      </React.Fragment>
    );
  }
  return (
    <React.Fragment>
      <Top />
      {touchpoints.items.map((t) => (
        <TouchpointRow key={t.id} touchpoint={t} />
      ))}
      {touchpoints.items.length === 0 && (
        <EmptyList>
          There are no Touchpoints yet.
          <CreateButtonClassic className="mt-3" to="/touchpoints/create">
            Create a Touchpoint
          </CreateButtonClassic>
        </EmptyList>
      )}
      <Paginator {...touchpoints.pagination} />
    </React.Fragment>
  );
};
