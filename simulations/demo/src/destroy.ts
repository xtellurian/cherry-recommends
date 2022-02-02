import {
  apiKeys,
  setBaseUrl,
  itemsRecommenders,
  environments,
  recommendableItems,
  customers,
  setDefaultEnvironmentId,
} from "cherry.ai";

import { loadContext, destroyContext } from "./context";
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

  await itemsRecommenders.deleteItemsRecommenderAsync({
    token,
    id: context.recommender.id,
    useInternalId: true,
  });

  await environments.deleteEnvironmentAsync({
    token,
    id: context.environment.id,
  });

  await destroyContext(context);

  return "Destroyed";
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
