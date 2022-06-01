import "./channel.css";

import {
  setBaseUrl,
  setTenant,
  setDefaultApiKey,
  channels,
  promotionsRecommenders,
  events,
} from "cherry.ai";

import { showEmailPopup } from "./templates";
import { generateId } from "./utilities";

const srcUrl = new URL(document.getElementById("cherry-channel").src);
const params = new URLSearchParams(srcUrl.search);

const baseUrl = params.get("baseUrl");
const tenant = params.get("tenant");
const apiKey = params.get("apiKey");
const channelId = params.get("channelId");

setBaseUrl(baseUrl);
setTenant(tenant);
setDefaultApiKey(apiKey);

if (!sessionStorage.getItem("cherryid")) {
  sessionStorage.setItem("cherryid", generateId());
}

(async function () {
  channels.fetchChannelAsync({ id: channelId }).then(({ properties }) => {
    const {
      popupAskForEmail,
      popupDelay = 0,
      popupHeader,
      popupSubheader,
      recommenderIdToInvoke,
      customerIdPrefix,
    } = properties;

    if (!popupAskForEmail) {
      return;
    }

    const qs = new URLSearchParams(window.location.search);

    // add prefix to the customer id if:
    // 1. customerIdPrefix exists
    // 2. the customer id is not prefixed yet
    if (customerIdPrefix && !sessionStorage.getItem("cherryid").includes("-")) {
      const prefixedCherryId = `${customerIdPrefix}-${sessionStorage.getItem(
        "cherryid"
      )}`;
      sessionStorage.setItem("cherryid", prefixedCherryId);
    }

    // log UTM parameters of the customers (a.k.a visitors) if exist
    events.createEventsAsync({
      events: [
        {
          commonUserId: sessionStorage.getItem("cherryid"),
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
      const defaultSubheader =
        "Don't miss any of our promotions. Subscribe now!";

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
            customerId: sessionStorage.getItem("cherryid"),
            arguments: {},
          },
        })
        .then(({ scoredItems, correlatorId, customerId }) => {
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
  });
})();
