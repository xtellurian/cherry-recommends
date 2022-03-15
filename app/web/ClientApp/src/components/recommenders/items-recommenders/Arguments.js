import React from "react";
import { useParams } from "react-router";
import { usePromotionsRecommender } from "../../../api-hooks/promotionsRecommendersApi";
import { setArgumentsAsync } from "../../../api/promotionsRecommendersApi";
import { ErrorCard, Spinner } from "../../molecules";
import { ArgumentsComponentUtil } from "../utils/argumentsComponent";

export const Arguments = () => {
  const { id } = useParams();
  const [trigger, setTrigger] = React.useState();
  const recommender = usePromotionsRecommender({ id, trigger });
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
      <div>
        {error && <ErrorCard error={error} />}
        {recommender.loading && <Spinner />}
        {!recommender.loading && (
          <ArgumentsComponentUtil
            recommender={recommender}
            basePath="/recommenders/promotions-recommenders"
            setArgumentsAsync={handleSet}
          />
        )}
      </div>
    </React.Fragment>
  );
};
