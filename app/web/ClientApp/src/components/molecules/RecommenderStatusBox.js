import React from "react";

import "./css/recommender-status-box.css";
import { Spinner } from ".";
import { ToggleSwitch } from "./ToggleSwitch";
import Tippy from "@tippyjs/react";

const EnabledToggle = ({ recommender, setEnabled }) => {
  const enabled = !!recommender?.settings?.enabled;
  const handleToggle = (value) => {
    if (typeof setEnabled === "function") {
      setEnabled(value);
    }
  };
  return (
    <React.Fragment>
      <Tippy
        placement="bottom-start"
        content={
          <div className="cherry-tooltip">Enable or Disable the campaign</div>
        }
      >
        <div id="d-flex justify-content-between">
          <ToggleSwitch
            id="recommender-enabled-toggle"
            checked={enabled}
            onChange={handleToggle}
            optionLabels={["On", ""]}
          />
          <span>{recommender?.settings?.enabled ? "Enabled" : "Disabled"}</span>
        </div>
      </Tippy>
    </React.Fragment>
  );
};

export const RecommenderStatusBox = ({ recommender, setEnabled }) => {
  if (recommender.loading) {
    return (
      <div className="float-right">
        <Spinner />
      </div>
    );
  } else if (recommender.useOptimiser) {
    return (
      <div className="float-right text-muted">
        <span>Using Internal Optimiser</span>
        <EnabledToggle recommender={recommender} setEnabled={setEnabled} />
      </div>
    );
  } else if (recommender.modelRegistrationId) {
    return (
      <div className="float-right text-muted">
        <span>Using Custom Model</span>
        <EnabledToggle recommender={recommender} setEnabled={setEnabled} />
      </div>
    );
  } else
    return (
      <div className="float-right text-muted">
        <span>Using Random Choices</span>
        <EnabledToggle recommender={recommender} setEnabled={setEnabled} />
      </div>
    );
};
