import React from "react";
import { setArgumentsAsync } from "../../../api/parameterSetRecommendersApi";
import { ErrorCard } from "../../molecules";
import { ArgumentsComponentUtil } from "../utils/argumentsComponent";
export const ArgumentsSection = ({ recommender, setTrigger }) => {
  const [error, setError] = React.useState();
  const handleSet = async (args) => {
    setError(null);
    try {
      await setArgumentsAsync(args);
    } catch (er) {
      setError(er);
    }
    setTrigger({});
  };
  return (
    <React.Fragment>
      <div>
        {error && <ErrorCard error={error} />}
        <ArgumentsComponentUtil
          recommender={recommender}
          basePath="/recommenders/parameter-set-recommenders"
          setArgumentsAsync={handleSet}
        />
      </div>
    </React.Fragment>
  );
};
