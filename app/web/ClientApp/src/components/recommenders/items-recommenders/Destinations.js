import React from "react";
import { useParams } from "react-router-dom";
import {
  useItemsRecommender,
  useDestinations,
} from "../../../api-hooks/itemsRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import {
  createDestinationAsync,
  removeDestinationAsync,
} from "../../../api/itemsRecommendersApi";
import { Spinner } from "../../molecules";
import { LoadingPopup } from "../../molecules/popups/LoadingPopup";
import { DestinationsUtil } from "../utils/destinationsUtil";
import { Tabs, TabActivator } from "../../molecules/layout/Tabs";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";
import { JsIntegrate, RESTIntegrate } from "./Integrate";

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
  const recommender = useItemsRecommender({ id });
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
      <ItemRecommenderLayout>
        <Tabs tabs={tabs} defaultTabId={defaultTabId} />
        <TabActivator tabId="integrated-systems" defaultTabId={defaultTabId}>
          <LoadingPopup loading={handlingCreate} label="Creating Destination" />
          <LoadingPopup loading={handlingRemove} label="Removing Destination" />
          {destinations.loading && <Spinner />}
          {!handlingCreate && !destinations.loading && (
            <DestinationsUtil
              error={error}
              recommender={recommender}
              destinations={destinations}
              createDestination={handleCreate}
              removeDestination={handleRemove}
              rootPath="/recommenders/promotions-recommenders"
            />
          )}
        </TabActivator>
        <TabActivator tabId="js" defaultTabId={defaultTabId}>
          <JsIntegrate id={id} />
        </TabActivator>
        <TabActivator tabId="rest-API" defaultTabId={defaultTabId}>
          <RESTIntegrate id={id} />
        </TabActivator>
      </ItemRecommenderLayout>
    </React.Fragment>
  );
};
