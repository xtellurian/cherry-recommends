import React from "react";
import { useParams } from "react-router-dom";
import { useParameterSetRecommender } from "../../../api-hooks/parameterSetRecommendersApi";
import {
  Subtitle,
  Title,
  Spinner,
  ErrorCard,
  Navigation,
  MoveUpHierarchyButton,
} from "../../molecules";
import { CodeView } from "../../molecules/CodeView";
import { Tabs, TabActivator } from "../../molecules/layout/Tabs";

const tabs = [
  { id: "api", label: "REST API" },
  { id: "js", label: "Javascript" },
  { id: "hubspot", label: "Hubspot CRM Card" },
];

const jsIntegrate = ({ id, argumentsExample }) => `
import { parameterSetRecommenders } from "cherry.ai";

parameterSetRecommenders
  .invokeParameterSetRecommenderAsync({
    token: "Your JSON Web Token / access token",
    id: 1, // the id of this recommender
    input: {
      commonUserId: "The common Id of the user to recommend for",
      arguments: {
        p_age: null,
        favourite: "gheklo",
        cats: "cats",
      },
    },
  })
  .then((recommendation) => console.log(recommendation))
  .catch((error) => alert("error callback"));

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

export const JsIntegrate = ({ id }) => {
  const recommender = useParameterSetRecommender({ id });

  const argumentsExample = recommender.oldArguments
    ? mapArgumentsToObject(recommender.oldArguments)
    : [];
  return (
    <CodeView
      language="js"
      text={jsIntegrate({ id, argumentsExample })}
    ></CodeView>
  );
};

export const RESTIntegrate = ({ id }) => {
  const recommender = useParameterSetRecommender({ id });
  const argumentsExample = recommender.oldArguments
    ? mapArgumentsToObject(recommender.oldArguments)
    : [];
  return (
    <CodeView
      language="bash"
      text={restIntegrate({ id, argumentsExample, basePath: origin })}
    ></CodeView>
  );
};

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

  const argumentsExample = recommender.oldArguments
    ? mapArgumentsToObject(recommender.oldArguments)
    : [];

  return (
    <React.Fragment>
      <MoveUpHierarchyButton
        className="float-right"
        to={`/recommenders/parameter-set-recommenders/detail/${id}`}
      >
        Recommender
      </MoveUpHierarchyButton>
      <Title>Integrate Parameter-Set Recommender</Title>
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
              text={restIntegrate({ id, argumentsExample, basePath: origin })}
            ></CodeView>
          </TabActivator>
          <TabActivator tabId="js" defaultTabId={defaultTabId}>
            {id && (
              <CodeView
                language="js"
                text={jsIntegrate({ id, argumentsExample })}
              ></CodeView>
            )}
          </TabActivator>
          <TabActivator tabId="hubspot" defaultTabId={defaultTabId}>
            {id && (
              <div>
                <h5>To connect this recommender to your Hubspot CRM card:</h5>
                <ol class="list-group list-group-numbered">
                  <li class="list-group-item">
                    <Navigation to="/settings/integrations">
                      <button className="btn btn-primary mr-3">
                        View Integrations
                      </button>
                    </Navigation>
                    Click the button below to view your existing integrations.
                  </li>

                  <li class="list-group-item">
                    View the details of your chosen Hubspot integration, and
                    click "More Options"
                  </li>
                  <li class="list-group-item">
                    Go to the CRM Card tab, and choose this recommender (
                    {recommender.name})
                  </li>
                </ol>
              </div>
            )}
          </TabActivator>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
