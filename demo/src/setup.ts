import {
  apiKeys,
  setBaseUrl,
  setDefaultEnvironmentId,
  environments,
  customers,
  recommendableItems,
  itemsRecommenders,
} from "cherry.ai";
import { newContext, saveContext } from "./context";
import * as env from "./env";
import { randomString } from "./util";

const CREDIT_COST_PER_DAY = 20;

console.log("Setup Cherry DEMO.");
setBaseUrl(env.baseUrl);

const main = async (): Promise<string> => {
  const context = newContext();
  const tokenResponse = await apiKeys.exchangeApiKeyAsync({
    apiKey: env.apiKey,
  });
  const token = tokenResponse.access_token;

  var environment = await environments.createEnvironmentAsync({
    token,
    environment: {
      name: randomString(8),
    },
  });
  context.environment = environment;

  setDefaultEnvironmentId(environment.id);

  const customer = await customers.createOrUpdateCustomerAsync({
    token,
    customer: {
      customerId: `customer_1_${randomString(3)}`,
    },
    user: null,
  });
  context.customer = customer;
  // create some items
  const noExtension = await recommendableItems.createItemAsync({
    token,
    item: {
      commonId: `free_trial_end_${randomString(3)}`,
      description: `Do not extend a customer's free trial`,
      listPrice: 0,
      name: `No extension`,
      properties: {
        days: 0,
      },
    },
  });
  const items = [noExtension];
  for (const days of [1, 2, 3, 4]) {
    const item = await recommendableItems.createItemAsync({
      token,
      item: {
        commonId: `free_trial_extend_${randomString(3)}_${days}days`,
        description: `Extends a customer's free trial by ${days} days`,
        listPrice: days * CREDIT_COST_PER_DAY,
        name: `${days} day extension`,
        properties: {
          days,
        },
      },
    });
    items.push(item);
  }
  context.items = items;

  const recommender = await itemsRecommenders.createItemsRecommenderAsync({
    token,
    useInternalId: true,
    payload: {
      baselineItemId: `${noExtension.id}`,
      itemIds: items.map((_) => _.id.toString()),
      useAutoAi: true,
      commonId: `free_trial_extender_${randomString(3)}`,
      name: "Free Trial Extender",
      numberOfItemsToRecommend: 1,
    },
  });

  context.recommender = recommender;

  saveContext(context);

  return `Created Recommender Id = ${recommender.id}`;
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
