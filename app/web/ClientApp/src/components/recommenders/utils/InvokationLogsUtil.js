import React from "react";
import {
  BackButton,
  EmptyList,
  ExpandableCard,
  Paginator,
  Subtitle,
  Title,
} from "../../molecules";
import { JsonView } from "../../molecules/JsonView";

const InvokationRow = ({ invokation }) => {
  return (
    <ExpandableCard
      label={invokation.message}
      headerClassName={invokation.success ? "bg-success" : "bg-danger"}
    >
      <JsonView data={invokation} />
    </ExpandableCard>
  );
};
export const InvokationLogsUtil = ({ recommender, invokations, rootPath }) => {
  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`${rootPath}/detail/${recommender.id}`}
      >
        Back to Recommender
      </BackButton>
      <Title>Invokation Logs</Title>
      <Subtitle>{recommender.name}</Subtitle>
      <hr />
      {invokations.items.map((i) => (
        <InvokationRow key={i.id} invokation={i} />
      ))}
      {invokations.items.length === 0 && <EmptyList>No invokations.</EmptyList>}

      <Paginator {...invokations.pagination} />
    </React.Fragment>
  );
};
