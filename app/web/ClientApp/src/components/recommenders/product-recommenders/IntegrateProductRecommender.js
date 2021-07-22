import React from "react";
import { useParams } from "react-router-dom";
import { useProductRecommender } from "../../../api-hooks/productRecommendersApi";
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
import { productRecommenders } from "signalbox.js";

productRecommenders.invokeProductRecommender({
    success: (recommendation) => console.log('Success callback'),
    error: (error) => alert('Error callback'),
    token: "Your JSON Web Token / access token",
    id: ${id}, // the id of this recommender
    input: {
        commonUserId: "The common Id of the user to recommend for"
    }
});
`;

const restIntegrate = ({ id, basePath }) => `
# location
POST ${basePath}/api/Recommenders/ProductRecommenders/${id}/Invoke

# headers
Content-Type: 'application/json'

# content
{
    "commonUserId": "User to recommend to" 
}
`;

export const IntegrateProductRecommender = () => {
  const { id } = useParams();
  const recommender = useProductRecommender({ id });
  const defaultTabId = tabs[0].id;
  const origin = window.location.origin;
  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`/recommenders/product-recommenders/detail/${id}`}
      >
        Recommender
      </BackButton>
      <Title>Integrate Product Recommender</Title>
      <Subtitle>{recommender.name || "..."}</Subtitle>
      <Tabs tabs={tabs} defaultTabId={defaultTabId} />
      <hr />
      {recommender.loading && <Spinner />}
      {recommender.error && <ErrorCard error={recommender.error} />}
      {!recommender.loading && (
        <React.Fragment>
          <TabActivator tabId="api" defaultTab={defaultTabId}>
            <CodeView
              language="bash"
              text={restIntegrate({ id, basePath: origin })}
            ></CodeView>
          </TabActivator>
          <TabActivator tabId="js" defaultTab={defaultTabId}>
            {id && (
              <CodeView language="js" text={jsIntegrate({ id })}></CodeView>
            )}
          </TabActivator>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
