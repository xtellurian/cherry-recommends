import React from "react";
import { useHistory } from "react-router-dom";
import { trackEvent } from "../.././../api/eventsApi";

const StateCard = ({ state }) => {
  const history = useHistory();
  const handleSelectState = () => {
    trackEvent({
      success: () => history.push("beer/subscribe"),
      error: (e) => alert(e),
      events: [{
        key: "geo.state",
        logicalValue: state,
      }],
    });
  };
  return (
    <div className="card">
      <button className="btn btn-primary btn-block" onClick={handleSelectState}>
        {state}
      </button>
    </div>
  );
};

const states = ["NSW", "VIC", "QLD", "ACT"];
export const ChooseState = () => {
  return (
    <React.Fragment>
      <div>
        <h2>Where do you live?</h2>
      </div>
      <div className="row">
        {states.map((s) => (
          <div key={s} className="col">
            <StateCard state={s} />
          </div>
        ))}
      </div>
    </React.Fragment>
  );
};
