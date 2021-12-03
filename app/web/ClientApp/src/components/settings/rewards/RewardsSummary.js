import React from "react";
import { Link } from "react-router-dom";
import { useRewardSelectors } from "../../../api-hooks/rewardSelectorsApi";
import {
  Subtitle,
  Title,
  ErrorCard,
  Spinner,
  EmptyList,
  Paginator,
} from "../../molecules";
import { CreateButtonClassic } from "../../molecules/CreateButton";

const SelectorRow = ({ selector }) => {
  return (
    <div className="row m-2">
      <div className="col">Property: {selector.actionName}</div>
      <div className="col-3">Type: {selector.selectorType}</div>
      <div className="col-2">
        <Link to={`/settings/rewards/reward-selector/${selector.id}`}>
          <button className="btn btn-secondary">View</button>
        </Link>
      </div>
    </div>
  );
};
export const RewardsSummary = () => {
  const selectors = useRewardSelectors();
  return (
    <React.Fragment>
      <CreateButtonClassic
        className="float-right"
        to="/settings/rewards/create-selector"
      >
        Define a new Reward
      </CreateButtonClassic>
      <Title>Rewards</Title>
      <Subtitle>Reward Definitions</Subtitle>

      <hr />

      {selectors.loading && <Spinner />}
      {selectors.error && <ErrorCard error={selectors.error} />}
      {selectors.items && selectors.items.length === 0 && (
        <EmptyList>There are no Rewards yet.</EmptyList>
      )}
      {selectors.items &&
        selectors.items.map((s) => <SelectorRow key={s.id} selector={s} />)}

      {selectors.pagination && <Paginator {...selectors.pagination} />}
    </React.Fragment>
  );
};
