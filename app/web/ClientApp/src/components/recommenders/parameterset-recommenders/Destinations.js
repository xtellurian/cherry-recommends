import React from "react";
import { useParams } from "react-router-dom";
import {
  useParameterSetRecommender,
  useDestinations,
} from "../../../api-hooks/parameterSetRecommendersApi";
import {
  createDestinationAsync,
  removeDestinationAsync,
} from "../../../api/parameterSetRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import { Spinner } from "../../molecules";
import { LoadingPopup } from "../../molecules/popups/LoadingPopup";
import { DestinationsUtil } from "../utils/destinationsUtil";

export const Destinations = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const [trigger, setTrigger] = React.useState();
  const [error, setError] = React.useState();
  const recommender = useParameterSetRecommender({ id });
  const destinations = useDestinations({ id, trigger });

  const [handlingCreate, setHandlingCreate] = React.useState(false);
  const [handlingRemove, setHandlingRemove] = React.useState(false);

  const handleCreate = (destination) => {
    setHandlingCreate(true);
    createDestinationAsync({ id, token, destination })
      .then(setTrigger)
      .catch(setError)
      .finally(() => setHandlingCreate(false));
  };

  const handleRemove = (id, destinationId) => {
    setHandlingRemove(true);
    removeDestinationAsync({ id, token, destinationId })
      .then(setTrigger)
      .catch(setError)
      .finally(() => setHandlingRemove(false));
  };
  if (destinations.loading) {
    return <Spinner />;
  }

  return (
    <React.Fragment>
      <LoadingPopup loading={handlingCreate} label="Creating Destination" />
      <LoadingPopup loading={handlingRemove} label="Removing Destination" />
      {!handlingCreate && !handlingRemove && (
        <DestinationsUtil
          error={error}
          recommender={recommender}
          destinations={destinations}
          createDestination={handleCreate}
          removeDestination={handleRemove}
          rootPath="/recommenders/parameter-set-recommenders"
        />
      )}
    </React.Fragment>
  );
};
