import React from "react";
import { Title } from "../../molecules/layout";
import { CreateButtonClassic } from "../../molecules/CreateButton";
import { Spinner } from "../../molecules/Spinner";
import { ErrorCard } from "../../molecules/ErrorCard";
import { EmptyList, Navigation } from "../../molecules";
import { Paginator } from "../../molecules/Paginator";
import { useParameterSetCampaigns } from "../../../api-hooks/parameterSetCampaignsApi";
import { EntityRow } from "../../molecules/layout/EntityRow";

const ParameterSetCampaignRow = ({ recommender }) => {
  return (
    <EntityRow>
      <div className="col">
        <h5>{recommender.name}</h5>
      </div>
      <div className="col-3">
        <Navigation
          to={`/campaigns/parameter-set-campaigns/detail/${recommender.id}`}
        >
          <button className="btn btn-outline-primary btn-block">Detail</button>
        </Navigation>
      </div>
    </EntityRow>
  );
};

export const ParameterSetCampaignsSummary = () => {
  const parameterSetRecommenders = useParameterSetCampaigns();
  return (
    <React.Fragment>
      <CreateButtonClassic
        className="float-right"
        to="/campaigns/parameter-set-campaigns/create"
      >
        Create Parameter Campaign
      </CreateButtonClassic>
      <Title>Parameter Set Campaigns</Title>
      <hr />
      {parameterSetRecommenders.error && (
        <ErrorCard error={parameterSetRecommenders.error} />
      )}
      {parameterSetRecommenders.loading && <Spinner />}
      {parameterSetRecommenders.items &&
        parameterSetRecommenders.items.length === 0 && (
          <EmptyList>
            <p>You have not created any Parameter-Set Campaigns. </p>
            <CreateButtonClassic to="/campaigns/parameter-set-campaigns/create">
              Create Parameter-Set Campaign
            </CreateButtonClassic>
          </EmptyList>
        )}
      {parameterSetRecommenders.items &&
        parameterSetRecommenders.items.map((r) => (
          <ParameterSetCampaignRow recommender={r} key={r.id} />
        ))}
      {parameterSetRecommenders.pagination && (
        <Paginator {...parameterSetRecommenders.pagination} />
      )}
    </React.Fragment>
  );
};
