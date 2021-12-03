import React from "react";
import {Link} from "react-router-dom"
import { Title } from "../../molecules/layout";
import { CreateButtonClassic } from "../../molecules/CreateButton";
import { Spinner } from "../../molecules/Spinner";
import { ErrorCard } from "../../molecules/ErrorCard";
import { EmptyList } from "../../molecules";
import { Paginator } from "../../molecules/Paginator";
import { useParameterSetRecommenders } from "../../../api-hooks/parameterSetRecommendersApi";
import { ExpandableCard } from "../../molecules/ExpandableCard";
import { CopyableField } from "../../molecules/fields/CopyableField";

const ParameterSetRecommenderRow = ({ recommender }) => {
  return (
    <ExpandableCard label={recommender.name}>
      <CopyableField label="Identifier" value={recommender.commonId} />
      <p>{recommender.description}</p>
      <Link to={`/recommenders/parameter-set-recommenders/detail/${recommender.id}`}>
      <button className="btn btn-primary float-right w-25">
        Detail
      </button>
      </Link>
    </ExpandableCard>
  );
};

export const ParameterSetRecommendersSummary = () => {
  const parameterSetRecommenders = useParameterSetRecommenders();
  return (
    <React.Fragment>
      <CreateButtonClassic
        className="float-right"
        to="/recommenders/parameter-set-recommenders/create"
      >
        Create Recommender
      </CreateButtonClassic>
      <Title>Parameter Set Recommenders</Title>
      <hr />
      {parameterSetRecommenders.error && (
        <ErrorCard error={parameterSetRecommenders.error} />
      )}
      {parameterSetRecommenders.loading && <Spinner />}
      {parameterSetRecommenders.items &&
        parameterSetRecommenders.items.length === 0 && (
          <EmptyList>
            <p>You have not created any Parameter-Set Recommenders. </p>
            <CreateButtonClassic to="/recommenders/parameter-set-recommenders/create">
              Create Parameter-Set Recommender
            </CreateButtonClassic>
          </EmptyList>
        )}
      {parameterSetRecommenders.items &&
        parameterSetRecommenders.items.map((r) => (
          <ParameterSetRecommenderRow recommender={r} key={r.id} />
        ))}
      {parameterSetRecommenders.pagination && (
        <Paginator {...parameterSetRecommenders.pagination} />
      )}
    </React.Fragment>
  );
};
