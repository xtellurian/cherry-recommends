import React from "react";
import { useParams } from "react-router";
import { useItemsRecommender } from "../../../api-hooks/itemsRecommendersApi";
import { setArgumentsAsync } from "../../../api/itemsRecommendersApi";
import { ErrorCard, Spinner } from "../../molecules";
import { ArgumentsComponentUtil } from "../utils/argumentsComponent";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";

export const Arguments = () => {
  const { id } = useParams();
  const [trigger, setTrigger] = React.useState();
  const recommender = useItemsRecommender({ id, trigger });
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
      <ItemRecommenderLayout>
        <div>
          {error && <ErrorCard error={error} />}
          {recommender.loading && <Spinner />}
          {!recommender.loading && (
            <ArgumentsComponentUtil
              recommender={recommender}
              basePath="/recommenders/items-recommenders"
              setArgumentsAsync={handleSet}
            />
          )}
        </div>
      </ItemRecommenderLayout>
    </React.Fragment>
  );
};
