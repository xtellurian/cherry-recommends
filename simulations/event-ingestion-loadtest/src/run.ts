import * as env from "./env";
import {
  apiKeys,
  setBaseUrl,
  environments,
  setDefaultEnvironmentId,
  events,
} from "cherry.ai";
import { randomString, getDaysArray } from "./util";

const totalDays = 365;
const today = new Date();
const start = new Date();
start.setDate(today.getDate() - totalDays);
const allDays = getDaysArray(start, today);

const EVENT_BATCH_SIZE = 50;
setBaseUrl(env.baseUrl);

const main = async (): Promise<string> => {
  const tokenResponse = await apiKeys.exchangeApiKeyAsync({
    apiKey: env.apiKey,
  });
  const token = tokenResponse.access_token;

  const environment = await environments.createEnvironmentAsync({
    token,
    environment: {
      name: randomString(5),
    },
  });
  setDefaultEnvironmentId(environment.id);

  let nRequests = 0;
  let totalEvents = 0;
  let startTime = new Date();
  try {
    for (let n = 0; n <= 1000; n++) {
      for (let m = 0; m <= EVENT_BATCH_SIZE; m++) {
        const randomSeed = Math.round(Math.random() * 90);

        const customerEvents = [];
        for (let d = 0; d < totalDays; d++) {
          const day = allDays[d];
          if (d % randomSeed === 0) {
            // then generate an event
            customerEvents.push({
              customerId: randomString(3),
              eventId: randomString(10),
              kind: "Behaviour",
              eventType: "Activity",
              properties: {
                randomSeed,
                past: true,
              },
              timestamp: day.toISOString(),
            });
          } else {
            customerEvents.push({
              customerId: randomString(3),
              eventId: randomString(10),
              kind: "Behaviour",
              eventType: "Activity",
              properties: {
                randomSeed,
                past: false,
              },
            });
          }
        }
        const e = await events.createEventsAsync({
          token,
          events: customerEvents as any,
        });
        totalEvents += customerEvents.length;
        nRequests += 1;

        console.log(e);
      }
    }
  } catch (ex) {
    console.log("An error occurred");
    console.log(ex);
  }
  const finishTime = new Date();
  const seconds = Math.round((finishTime.getTime() - startTime.getTime()) / 1000);

  console.log(`Total Requests: ${nRequests} with total events ${totalEvents} took ${seconds} seconds`);
  console.log(`${nRequests / seconds} req/s`)
  console.log(`${totalEvents / seconds} event/s`)

  await environments.deleteEnvironmentAsync({ token, id: environment.id });
  return "Done";
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
