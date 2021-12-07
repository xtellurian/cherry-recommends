import React from "react";
import { useParams } from "react-router";
import { useParameterSetRecommender } from "../../../api-hooks/parameterSetRecommendersApi";
import { setArgumentsAsync } from "../../../api/parameterSetRecommendersApi";
import { ErrorCard, Spinner } from "../../molecules";
import { ArgumentsComponentUtil } from "../utils/argumentsComponent";
import { ParameterSetRecommenderLayout } from "./ParameterSetRecommenderLayout";

export const Arguments = () => {
  const { id } = useParams();
  const [trigger, setTrigger] = React.useState();
  const recommender = useParameterSetRecommender({ id, trigger });
  return <ArgumentsSection recommender={recommender} setTrigger={setTrigger} />;
};

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
      <ParameterSetRecommenderLayout>
        {recommender.loading && <Spinner />}
        {!recommender.loading && (
          <div>
            {error && <ErrorCard error={error} />}
            <ArgumentsComponentUtil
              recommender={recommender}
              basePath="/recommenders/parameter-set-recommenders"
              setArgumentsAsync={handleSet}
            />
          </div>
        )}
      </ParameterSetRecommenderLayout>
    </React.Fragment>
  );
};
