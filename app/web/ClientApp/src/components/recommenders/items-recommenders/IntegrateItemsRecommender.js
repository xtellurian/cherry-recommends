import React from "react";
import { useParams } from "react-router-dom";
import { useItemsRecommender } from "../../../api-hooks/itemsRecommendersApi";
import {
  BackButton,
  Subtitle,
  Title,
  Spinner,
  ErrorCard,
} from "../../molecules";
import { CodeView } from "../../molecules/CodeView";
import { Tabs, TabActivator } from "../../molecules/Tabs";

const tabs = [
  { id: "api", label: "REST API" },
  { id: "js", label: "Javascript" },
];

const jsIntegrate = ({ id }) => `
import { itemsRecommenders } from "cherry.ai";

itemsRecommenders.invokeItemsRecommenderAsync({
    token: "Your JSON Web Token / access token",
    id: ${id}, // the id of this recommender
    input: {
        commonUserId: "The common Id of the user to recommend for",
        arguments: {
          basketSize: 5 // these can be any value, but not too many of them.
        }
    }
})
.then( recommendation => console.log('success callback'))
.catch( error => alert('error callback') );
`;

const restIntegrate = ({ id, basePath }) => `
# location
POST ${basePath}/api/Recommenders/ItemsRecommenders/${id}/Invoke

# headers
Content-Type: 'application/json'

# content
{
    "commonUserId": "User to recommend to" ,
    "arguments": {
      "basketSize": 5
    }
}
`;

export const IntegrateItemsRecommender = () => {
  const { id } = useParams();
  const recommender = useItemsRecommender({ id });
  const defaultTabId = tabs[0].id;
  const origin = window.location.origin;
  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`/recommenders/items-recommenders/detail/${id}`}
      >
        Recommender
      </BackButton>
      <Title>Integrate Item Recommender</Title>
      <Subtitle>{recommender.name || "..."}</Subtitle>
      <Tabs tabs={tabs} defaultTabId={defaultTabId} />
      <hr />
      {recommender.loading && <Spinner />}
      {recommender.error && <ErrorCard error={recommender.error} />}
      {!recommender.loading && (
        <React.Fragment>
          <TabActivator tabId="api" defaultTabId={defaultTabId}>
            <CodeView
              language="bash"
              text={restIntegrate({ id, basePath: origin })}
            ></CodeView>
          </TabActivator>
          <TabActivator tabId="js" defaultTabId={defaultTabId}>
            {id && (
              <CodeView language="js" text={jsIntegrate({ id })}></CodeView>
            )}
          </TabActivator>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
