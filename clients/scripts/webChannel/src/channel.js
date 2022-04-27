import "./channel.css";

import {
  setBaseUrl,
  setTenant,
  apiKeys,
  channels,
  promotionsRecommenders,
  events,
} from "cherry.ai";

import { showEmailPopup } from "./templates";
import { generateId, setLocalStorageWithExpiry } from "./utilities";

const srcUrl = new URL(document.getElementById("cherry-channel").src);
const params = new URLSearchParams(srcUrl.search);

const baseUrl = params.get("baseUrl");
const tenant = params.get("tenant");
const apiKey = params.get("apiKey");
const channelId = params.get("channelId");

setBaseUrl(baseUrl);
setTenant(tenant);

if (!sessionStorage.getItem("cherryid")) {
  sessionStorage.setItem("cherryid", generateId());
}

(async function () {
  // check for accessToken in localStorage
  let accessToken = JSON.parse(localStorage.getItem("cherry_token")) || {};
  const now = new Date();

  // if accessToken is null, exchange apiKey for token and store it in localStorage
  if (!accessToken.value || accessToken.expiry <= now.getTime()) {
    const payload = await apiKeys.exchangeApiKeyAsync({ apiKey });

    if (payload.access_token) {
      setLocalStorageWithExpiry("cherry_token", payload.access_token, 30);
      accessToken = { value: payload.access_token };
    }
  }

  channels
    .fetchChannelAsync({ id: channelId, token: accessToken.value })
    .then(({ properties }) => {
      const {
        popupAskForEmail,
        popupDelay = 0,
        popupHeader,
        popupSubheader,
        recommenderIdToInvoke,
        customerIdPrefix,
      } = properties;
      const qs = new URLSearchParams(window.location.search);

      // add prefix to the customer id if:
      // 1. customerIdPrefix exists
      // 2. the customer id is not prefixed yet
      if (
        customerIdPrefix &&
        !sessionStorage.getItem("cherryid").includes("-")
      ) {
        const prefixedCherryId = `${customerIdPrefix}-${sessionStorage.getItem(
          "cherryid"
        )}`;
        sessionStorage.setItem("cherryid", prefixedCherryId);
      }

      // log UTM parameters of the customers (a.k.a visitors) if exist
      events.createEventsAsync({
        token: accessToken.value,
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
            },
            timestamp: new Date(),
          },
        ],
      });

      if (!popupAskForEmail) {
        return;
      }

      // show popup asking for email WITHOUT promotion
      if (!recommenderIdToInvoke) {
        const defaultSubheader =
          "Don't miss any of our promotions. Subscribe now!";

        setTimeout(() => {
          showEmailPopup({
            token: accessToken.value,
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
            token: accessToken.value,
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
                token: accessToken.value,
                header: header || "",
                subheader: subheader || item.name,
              });
            }, popupDelay);

            events.createRecommendationConsumedEventAsync({
              token: accessToken.value,
              customerId: customerId,
              correlatorId: correlatorId,
            });
          });
      }
    });
})();
