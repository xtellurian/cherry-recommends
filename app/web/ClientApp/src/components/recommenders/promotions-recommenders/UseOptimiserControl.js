import React from "react";
import { setUseOptimiserAsync } from "../../../api/promotionsRecommendersApi";
import { ToggleSwitch } from "../../molecules/ToggleSwitch";
import { useAccessToken } from "../../../api-hooks/token";
import { ErrorCard, Spinner } from "../../molecules";

export const UseOptimiserControl = ({ recommender, onUpdated }) => {
  const token = useAccessToken();
  const [error, setError] = React.useState();

  const handleUseOptimiser = (useOptimiser) => {
    console.log("handling useOptimiser");
    setError();
    setUseOptimiserAsync({ token, id: recommender.id, useOptimiser })
      .then(onUpdated)
      .catch(setError);
  };
  if (recommender.loading) {
    return <Spinner />;
  }
  return (
    <div className="pt-2 pb-2">
      {error ? <ErrorCard error={error} /> : null}
      <ToggleSwitch
        name="Use Optimiser"
        id="enable-optimiser"
        optionLabels={["On", "Off"]}
        checked={recommender.useOptimiser}
        onChange={(v) => handleUseOptimiser(v)}
      />
    </div>
  );
};
