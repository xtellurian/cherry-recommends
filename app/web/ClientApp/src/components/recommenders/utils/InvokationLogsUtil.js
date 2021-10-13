import React from "react";
import { toShortDate } from "../../../utility/utility";
import {
  EmptyList,
  ErrorCard,
  ExpandableCard,
  Paginator,
} from "../../molecules";
import { JsonView } from "../../molecules/JsonView";

const InvokationRow = ({ invokation }) => {
  const d = toShortDate(invokation.created);
  const label =
    (invokation.status || invokation.message || "Invoked") + ` ${d}`;
  return (
    <ExpandableCard
      label={label}
      headerClassName={invokation.success ? "bg-success" : "bg-danger"}
    >
      <JsonView data={invokation} />
    </ExpandableCard>
  );
};
export const InvokationLogsUtil = ({ invokations }) => {
  return (
    <React.Fragment>
      {invokations.error && <ErrorCard error={invokations.error} />}
      {invokations.items &&
        invokations.items.map((i) => (
          <InvokationRow key={i.id} invokation={i} />
        ))}
      {invokations.items.length === 0 && <EmptyList>No invokations.</EmptyList>}

      <Paginator {...invokations.pagination} />
    </React.Fragment>
  );
};
