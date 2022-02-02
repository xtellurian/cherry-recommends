import * as env from "./env";
import {
  apiKeys,
  setBaseUrl,
  customers,
  setDefaultEnvironmentId,
  events,
  itemsRecommenders,
} from "cherry.ai";
import { randomString, getDaysArray } from "./util";

import { loadContext } from "./context";

const nCustomers = 10;
const totalDays = 365;
const today = new Date();
const start = new Date();
start.setDate(today.getDate() - totalDays);
const allDays = getDaysArray(start, today);

setBaseUrl(env.baseUrl);

const main = async (): Promise<string> => {
  const context = await loadContext();

  const tokenResponse = await apiKeys.exchangeApiKeyAsync({
    apiKey: env.apiKey,
  });
  const token = tokenResponse.access_token;
  setDefaultEnvironmentId(context.environment.id);

  context.customers = [];
  for (let customerId = 1; customerId <= nCustomers; customerId++) {
    const customer = await customers.createOrUpdateCustomerAsync({
      token,
      customer: {
        customerId: `${customerId}`,
      },
      user: null,
    });
    context.customers.push(customer);
  }

  for (const c of context.customers) {
    const randomSeed = Math.round(Math.random() * 90);
    let recommended = false;
    const customerEvents = [];
    for (let d = 0; d < totalDays; d++) {
      const day = allDays[d];
      if (d % randomSeed === 0) {
        // then generate an event
        customerEvents.push({
          customerId: c.customerId,
          eventId: randomString(10),
          kind: "Behaviour",
          eventType: "Activity",
          properties: {
            randomSeed,
          },
          timestamp: day.toISOString(),
        });
      }

      if (!recommended && d % (2 * randomSeed) === 0) {
        recommended = true;
        await itemsRecommenders.invokeItemsRecommenderAsync({
          token,
          id: context.recommender.id,
          useInternalId: true,
          input: {
            commonUserId: c.customerId,
            arguments: {},
          },
        });
      }
    }
    const e = await events.createEventsAsync({
      token,
      events: customerEvents as any,
    });

    console.log(e);
  }

  // const recommendation = await itemsRecommenders.invokeItemsRecommenderAsync({
  //   token,
  //   id: context.recommender.id,
  //   input: {
  //     commonUserId: context.customer.customerId,
  //     arguments: {},
  //   },
  //   useInternalId: true,
  // });

  // console.log(recommendation);

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
