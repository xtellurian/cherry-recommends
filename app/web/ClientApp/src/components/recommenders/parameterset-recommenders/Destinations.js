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
import { ParameterSetRecommenderLayout } from "./ParameterSetRecommenderLayout";
import { RESTIntegrate, JsIntegrate } from "./Integrate";
import { Tabs, TabActivator } from "../../molecules/layout/Tabs";

const tabs = [
  {
    id: "integrated-systems",
    label: "Integrated Systems",
  },
  {
    id: "js",
    label: "Javsascript SDK",
  },
  { id: "rest-API", label: "REST API" },
];
const defaultTabId = tabs[0].id;

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

  return (
    <React.Fragment>
      <ParameterSetRecommenderLayout>
        <Tabs tabs={tabs} defaultTabId={defaultTabId} />
        {destinations.loading && <Spinner />}
        <TabActivator tabId="integrated-systems" defaultTabId={defaultTabId}>
          <LoadingPopup loading={handlingCreate} label="Creating Destination" />
          <LoadingPopup loading={handlingRemove} label="Removing Destination" />
          {!destinations.loading && !handlingCreate && !handlingRemove && (
            <DestinationsUtil
              error={error}
              recommender={recommender}
              destinations={destinations}
              createDestination={handleCreate}
              removeDestination={handleRemove}
              rootPath="/recommenders/parameter-set-recommenders"
            />
          )}
        </TabActivator>
        <TabActivator tabId="js" defaultTabId={defaultTabId}>
          <JsIntegrate id={id} />
        </TabActivator>
        <TabActivator tabId="rest-API" defaultTabId={defaultTabId}>
          <RESTIntegrate id={id} />
        </TabActivator>
      </ParameterSetRecommenderLayout>
    </React.Fragment>
  );
};
