import {
  apiKeys,
  setBaseUrl,
  itemsRecommenders,
  environments,
  recommendableItems,
  customers,
  setDefaultEnvironmentId,
} from "cherry.ai";

import { loadContext } from "./context";
import * as env from "./env";

console.log("Destroy Cherry DEMO.");
setBaseUrl(env.baseUrl);

const main = async (): Promise<string> => {
  const context = await loadContext();

  const tokenResponse = await apiKeys.exchangeApiKeyAsync({
    apiKey: env.apiKey,
  });
  const token = tokenResponse.access_token;
  setDefaultEnvironmentId(context.environment.id);

  const recommendation = await itemsRecommenders.invokeItemsRecommenderAsync({
    token,
    id: context.recommender.id,
    input: {
      commonUserId: context.customer.customerId,
      arguments: {},
    },
    useInternalId: true,
  });

  console.log(recommendation);

  return "Invoked";
};

main().then(
  (text) => {
    console.log(text);
  },
  (err) => {
    // Deal with the fact the chain failed
    console.log("Something broke");
    console.log(err);
  }
);
