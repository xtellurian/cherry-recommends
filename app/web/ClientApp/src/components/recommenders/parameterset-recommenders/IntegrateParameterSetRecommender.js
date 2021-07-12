import React from "react";
import { useParams } from "react-router-dom";
import { useParameterSetRecommender } from "../../../api-hooks/parameterSetRecommendersApi";
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

const jsIntegrate = ({ id, argumentsExample }) => `
import { parameterSetRecommenders } from "signalbox.js";

parameterSetRecommenders.invokeParameterSetRecommender({
  success: (recommendation) => console.log('Success callback'),
  error: (error) => alert('Error callback'),
  token: "Your JSON Web Token / access token",
  id: ${id}, // the id of this recommender
  input: {
    commonUserId: "The common Id of the user to recommend for",
    arguments: ${JSON.stringify(argumentsExample, null, 2)},
  }
});
`;

const restIntegrate = ({ id, basePath, argumentsExample }) => `
# location
POST ${basePath}/api/Recommenders/ParameterSetRecommenders/${id}/Invoke

# headers
Content-Type: 'application/json'

# content
{
    "commonUserId": "User to recommend to",
    "arguments": ${JSON.stringify(argumentsExample, null, 2)}
}
`;

const mapArgumentsToObject = (args) => {
  const example = {};
  for (const a of args) {
    example[a.commonId] = a.defaultValue ? a.defaultValue.value : 0;
  }
  return example;
};

export const IntegrateParameterSetRecommender = () => {
  const { id } = useParams();
  const recommender = useParameterSetRecommender({ id });
  const defaultTabId = tabs[0].id;
  const origin = window.location.origin;

  const argumentsExample = recommender.arguments
    ? mapArgumentsToObject(recommender.arguments)
    : [];

  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`/recommenders/parameter-set-recommenders/detail/${id}`}
      >
        Recommender
      </BackButton>
      <Title>Integrate Parameter-Set Recommender</Title>
      <Subtitle>{recommender.name || "..."}</Subtitle>
      <Tabs tabs={tabs} defaultTab={defaultTabId} />
      <hr />
      {recommender.loading && <Spinner />}
      {recommender.error && <ErrorCard error={recommender.error} />}
      {!recommender.loading && (
        <React.Fragment>
          <TabActivator tabId="api" defaultTab={defaultTabId}>
            <CodeView
              language="bash"
              text={restIntegrate({ id, argumentsExample, basePath: origin })}
            ></CodeView>
          </TabActivator>
          <TabActivator tabId="js" defaultTab={defaultTabId}>
            {id && (
              <CodeView
                language="js"
                text={jsIntegrate({ id, argumentsExample })}
              ></CodeView>
            )}
          </TabActivator>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};