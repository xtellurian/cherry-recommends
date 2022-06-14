import "./channel.css";

import {
  setBaseUrl,
  setTenant,
  setDefaultApiKey,
  channels,
  promotionsRecommenders,
  events,
  customers,
} from "cherry.ai";

import { showEmailPopup } from "./templates";
import { generateId } from "./utilities";
import {
  setStorageType,
  getCherryStorageData,
  setCherryStorageData,
} from "./storage";
import { storageKeys } from "./constants";

const srcUrl = new URL(document.getElementById("cherry-channel").src);
const params = new URLSearchParams(srcUrl.search);

const baseUrl = params.get("baseUrl");
const tenant = params.get("tenant");
const apiKey = params.get("apiKey");
const channelId = params.get("channelId");

setBaseUrl(baseUrl);
setTenant(tenant);
setDefaultApiKey(apiKey);

function executeCampaign({ customerId, channelProperties }) {
  const qs = new URLSearchParams(window.location.search);

  const {
    popupDelay = 0,
    popupHeader,
    popupSubheader,
    recommenderIdToInvoke,
  } = channelProperties;

  // logs pageView event and appends UTM parameters of the customers (a.k.a visitors) if it exists,
  events.createEventsAsync({
    events: [
      {
        commonUserId: customerId,
        eventId: generateId(),
        kind: "pageView",
        eventType: "Customer UTM parameters",
        recommendationCorrelatorId: null,
        properties: {
          utm_id: qs.get("utm_id"),
          utm_source: qs.get("utm_source"),
          utm_medium: qs.get("utm_medium"),
          utm_campaign: qs.get("utm_campaign"),
          utm_term: qs.get("utm_term"),
          utm_content: qs.get("utm_content"),
          host: location.host,
          path: location.pathname,
        },
        timestamp: new Date(),
      },
    ],
  });

  // show popup asking for email WITHOUT promotion
  if (!recommenderIdToInvoke) {
    const defaultSubheader = "Don't miss any of our promotions. Subscribe now!";

    setTimeout(() => {
      showEmailPopup({
        header: popupHeader || "",
        subheader: popupSubheader || defaultSubheader,
      });
    }, popupDelay);

    return;
  }

  // show popup asking for email WITH promotion
  if (recommenderIdToInvoke) {
    promotionsRecommenders
      .invokePromotionsRecommenderAsync({
        id: recommenderIdToInvoke,
        input: {
          customerId: customerId,
          arguments: {},
        },
      })
      .then(({ scoredItems, correlatorId }) => {
        const { item } = scoredItems[0];

        const header = popupHeader
          .replace("%promotionName%", item.name)
          .toUpperCase();

        const subheader = popupSubheader.replace(
          "%promotionName%",
          item.name.toLowerCase()
        );

        setTimeout(() => {
          showEmailPopup({
            header: header || "",
            subheader: subheader || item.name,
          });
        }, popupDelay);

        events.createRecommendationConsumedEventAsync({
          customerId: customerId,
          correlatorId: correlatorId,
        });
      });
  }
}

(async function () {
  channels
    .fetchChannelAsync({ id: channelId })
    .then(({ properties }) => {
      const {
        popupAskForEmail: showPopup,
        customerIdPrefix,
        storageType,
      } = properties;

      setStorageType(storageType);

      // don't show popup
      if (!showPopup || getCherryStorageData({ key: storageKeys.HIDDEN })) {
        return;
      }

      // generate customerId if it doesn't exist in sessionStorage or localStorage yet
      if (!getCherryStorageData({ key: storageKeys.ID })) {
        const customerId = customerIdPrefix
          ? `${customerIdPrefix}-${generateId()}`
          : generateId();

        setCherryStorageData({ key: storageKeys.ID, value: customerId });
      }

      // add prefix to the customerId if
      // - customerIdPrefix exists
      // - and customerId doesn't have a prefix yet
      if (
        customerIdPrefix &&
        !getCherryStorageData({ key: storageKeys.ID }).includes("-")
      ) {
        const prefixedCherryId = `${customerIdPrefix}-${getCherryStorageData({
          key: storageKeys.ID,
        })}`;

        setCherryStorageData({ key: storageKeys.ID, value: prefixedCherryId });
      }

      customers
        .fetchCustomerAsync({
          id: getCherryStorageData({ key: storageKeys.ID }),
          useInternalId: false,
        })
        .then(({ customerId }) => {
          executeCampaign({ customerId, channelProperties: properties });
        })
        .catch(({ status }) => {
          if (status === 404) {
            // if customer doesn't exist, create a customer and execute the campaign
            customers
              .createOrUpdateCustomerAsync({
                customer: {
                  customerId: getCherryStorageData({ key: storageKeys.ID }),
                  name: getCherryStorageData({ key: storageKeys.ID }),
                  email: null,
                  integratedSystemReference: null,
                },
              })
              .then(({ customerId }) => {
                executeCampaign({ customerId, channelProperties: properties });
              });
          }
        });
    })
    .catch(() => {
      // TODO: fetchChannelAsync fails
    });
})();
