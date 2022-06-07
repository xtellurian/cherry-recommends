import React from "react";
import { CreateButtonClassic } from "../../molecules/CreateButton";
import { Spinner } from "../../molecules/Spinner";
import { EmptyList, Navigation } from "../../molecules";
import { Paginator } from "../../molecules/Paginator";
import { useParameterSetCampaigns } from "../../../api-hooks/parameterSetCampaignsApi";
import { EntityRow } from "../../molecules/layout/EntityRow";

import Layout, {
  CreateEntityButton,
} from "../../molecules/layout/EntitySummaryLayout";

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
    <Layout
      header="Parameter Campaigns"
      createButton={
        <CreateEntityButton to="/campaigns/parameter-set-campaigns/create">
          Create a Campaign
        </CreateEntityButton>
      }
      error={parameterSetRecommenders.error}
    >
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
    </Layout>
  );
};
