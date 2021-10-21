import React from "react";
import { setArgumentsAsync } from "../../../api/itemsRecommendersApi";
import { ErrorCard, Spinner } from "../../molecules";
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
  if (recommender.loading) {
    return (
      <React.Fragment>
        <Spinner />
      </React.Fragment>
    );
  }
  return (
    <React.Fragment>
      <div>
        {error && <ErrorCard error={error} />}
        <ArgumentsComponentUtil
          recommender={recommender}
          basePath="/recommenders/items-recommenders"
          setArgumentsAsync={handleSet}
        />
      </div>
    </React.Fragment>
  );
};
