import React from "react";
import { useParams } from "react-router-dom";

import {
  usePromotionsRecommender,
  useDestinations,
} from "../../../api-hooks/promotionsRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import {
  createDestinationAsync,
  removeDestinationAsync,
} from "../../../api/promotionsRecommendersApi";
import { Spinner } from "../../molecules";
import { LoadingPopup } from "../../molecules/popups/LoadingPopup";
import { DestinationsUtil } from "../utils/destinationsUtil";
import { JsIntegrate, RESTIntegrate } from "./Integrate";
import {
  StatefulTabs,
  TabActivator,
} from "../../molecules/layout/StatefulTabs";

export const Destinations = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const [trigger, setTrigger] = React.useState();
  const [error, setError] = React.useState();
  const recommender = usePromotionsRecommender({ id });
  const destinations = useDestinations({ id, trigger });

  const [handlingCreate, setHandlingCreate] = React.useState(false);
  const [handlingRemove, setHandlingRemove] = React.useState(false);
  const [currentTabId, setCurrentTabId] = React.useState();

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

  const tabs = [
    {
      id: "integrated-systems",
      label: "Integrated Systems",
      render: () => (
        <React.Fragment>
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
        </React.Fragment>
      ),
    },
    {
      id: "js",
      label: "Javsascript SDK",
      render: () => <JsIntegrate id={id} />,
    },
    {
      id: "rest-API",
      label: "REST API",
      render: () => <RESTIntegrate id={id} />,
    },
  ];

  React.useEffect(() => {
    setCurrentTabId(tabs[0].id);
  }, []);

  return (
    <React.Fragment>
      <StatefulTabs
        tabs={tabs}
        currentTabId={currentTabId}
        setCurrentTabId={setCurrentTabId}
      />

      {tabs.map((tab) => (
        <TabActivator key={tab.id} currentTabId={currentTabId} tabId={tab.id}>
          {tab?.render()}
        </TabActivator>
      ))}
    </React.Fragment>
  );
};
