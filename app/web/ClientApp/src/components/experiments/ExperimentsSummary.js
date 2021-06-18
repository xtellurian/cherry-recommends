import React from "react";
import { Link } from "react-router-dom";
import { useExperiments } from "../../api-hooks/experimentsApi";
import { Spinner } from "../molecules/Spinner";
import { Title } from "../molecules/PageHeadings";
import { CreateButton } from "../molecules/CreateButton";
import { EmptyList } from "../molecules/EmptyList";
import { Paginator } from "../molecules/Paginator";

export const ExperimentsSummary = () => {
  const result = useExperiments();

  return (
    <React.Fragment>
      <div className="float-right">
        <CreateButton to="/experiments/create">
          Create New Experiment
        </CreateButton>
      </div>
      <Title>Experiments</Title>
      <hr />
      <div>
        {result.items && (
          <div>
            {result.items.map((exp) => (
              <div key={exp.id}>
                <Link to={`/experiments/results/${exp.id}`}>
                  <button className="btn btn-outline-primary btn-block">
                    {exp.name}
                  </button>
                </Link>
              </div>
            ))}
          </div>
        )}
        {result.items && result.items.length === 0 && (
          <EmptyList>
            <p>There are no experiments.</p>
            <div>
              <CreateButton to="/experiments/create">
                Create New Experiment
              </CreateButton>
            </div>
          </EmptyList>
        )}
        {result.loading && <Spinner />}
      </div>
      {result && <Paginator {...result.pagination} />}
    </React.Fragment>
  );
};
