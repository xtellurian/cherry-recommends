import React from "react";
import { Link } from "react-router-dom";
import { useExperiments } from "../../api-hooks/experimentsApi";
import { Spinner } from "../molecules/Spinner";
import { CreateButton } from "../molecules/CreateButton";
import { Paginator } from "../molecules/Paginator";

export const ExperimentsSummary = () => {
  const { result } = useExperiments();

  return (
    <React.Fragment>
      <div className="text-right">
        <CreateButton to="/experiments/create" className="mt-4">
          Create New Experiment
        </CreateButton>
      </div>
      <div>
        <h3>Experiments Summary</h3>
        {result && (
          <div>
            There are {result.items.length} experiments
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
        {!result && <Spinner />}
      </div>
      {result && <Paginator {...result.pagination} />}
    </React.Fragment>
  );
};
