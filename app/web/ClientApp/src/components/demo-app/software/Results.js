import React from "react";
import { useLocation } from "react-router-dom";
import { DropdownComponent, DropdownItem } from "../../molecules/Dropdown";
import { Chart } from "./Chart";
import { getResults } from "./resultsData";
import "./timeline.css";

function useQuery() {
  return new URLSearchParams(useLocation().search);
}

const experiments = ["new", "infrequent", "power"];

const Timeline = ({ children }) => {
  return <ul className="timeline">{children}</ul>;
};

const TimelineEntry = ({ name, date, children }) => {
  return (
    <li>
      <button className="btn btn-link">{name}</button>
      <button className="btn btn-link float-right">{date}</button>
      {children}
    </li>
  );
};

export const Results = () => {
  const query = useQuery();
  const [experimentName, setExperimentName] = React.useState(
    query.get("experiment") || "new"
  );

  const [results, setResults] = React.useState(getResults(experimentName));

  React.useEffect(() => {
    setResults(getResults(experimentName));
  }, [experimentName]);

  return (
    <div>
      <div className="float-right">
        <DropdownComponent title={experimentName}>
          {experiments.map((e, i) => (
            <DropdownItem key={i} onClick={() => setExperimentName(e)}>
              {e}
            </DropdownItem>
          ))}
        </DropdownComponent>
      </div>
      <h3>Experiment Results | {experimentName}</h3>
      <hr />
      <div className="row">
        <div className="col">
          <Timeline>
            {results
              .slice()
              .reverse()
              .map((i) => (
                <TimelineEntry
                  key={i.order}
                  name={`Iteration ${i.order}`}
                  date={i.startDate}
                >
                  <p>
                    {i.offersMade} offers made with mean income ${i.offerIncome}
                  </p>
                </TimelineEntry>
              ))}
          </Timeline>
        </div>
        <div className="col">
          <Chart iterations={results} />
        </div>
      </div>
    </div>
  );
};
