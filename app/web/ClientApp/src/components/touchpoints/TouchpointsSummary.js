import React from "react";
import { useTouchpoints } from "../../api-hooks/touchpointsApi";
import { Spinner } from "../molecules/Spinner";
import { Title } from "../molecules/PageHeadings";
import { EmptyList } from "../molecules/EmptyList";
import { CreateButton } from "../molecules/CreateButton";
import { Paginator } from "../molecules/Paginator";

const TouchpointRow = ({ touchpoint }) => {
  return (
    <div className="card">
      <div className="row card-body">
        <div className="col">{touchpoint.commonId}</div>
        <div className="col">{touchpoint.name}</div>
      </div>
    </div>
  );
};
const Top = () => {
  return (
    <React.Fragment>
      <CreateButton className="float-right" to="/touchpoints/create">
        Create a Touchpoint
      </CreateButton>
      <Title>Touchpoints</Title>
      <hr />
    </React.Fragment>
  );
};
export const TouchpointsSummary = () => {
  const { result } = useTouchpoints();
  if (!result || result.loading) {
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
      {result.items.map((t) => (
        <TouchpointRow key={t.id} touchpoint={t} />
      ))}
      {result.items.length === 0 && (
        <EmptyList>
          There are no Touchpoints yet.
          <CreateButton className="mt-3" to="/touchpoints/create">
            Create a Touchpoint
          </CreateButton>
        </EmptyList>
      )}
      <Paginator  {...result.pagination}/>
    </React.Fragment>
  );
};
