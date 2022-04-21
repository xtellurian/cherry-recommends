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
import { generateId } from "./utilities";

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

apiKeys.exchangeApiKeyAsync({ apiKey }).then(({ access_token }) => {
  channels
    .fetchChannelAsync({ id: channelId, token: access_token })
    .then(({ properties }) => {
      const {
        popupAskForEmail,
        popupDelay = 0,
        popupHeader,
        popupSubheader,
        recommenderIdToInvoke,
      } = properties;

      if (!popupAskForEmail) {
        return;
      }

      if (!recommenderIdToInvoke) {
        setTimeout(() => {
          showEmailPopup({
            token: access_token,
            header: popupHeader || "",
            subheader: popupSubheader || "Don't miss any of our promotions. Subscribe now!",
          });
        }, popupDelay);
        return;
      }

      if (recommenderIdToInvoke) {
        promotionsRecommenders
          .invokePromotionsRecommenderAsync({
            token: access_token,
            id: recommenderIdToInvoke,
            input: {
              customerId: sessionStorage.getItem("cherryid"),
              arguments: {},
            },
          })
          .then(({ scoredItems, correlatorId, customerId }) => {
            const { item } = scoredItems[0];

            // const benefitLabels = {
            //   percent: `${item.benefitValue}%`,
            //   fixed: `$${item.benefitValue}`,
            // };

            setTimeout(() => {
              showEmailPopup({
                token: access_token,
                header: popupHeader.replace("%promotionName%", item.name).toUpperCase() || "",
                subheader: popupSubheader.replace("%promotionName%", item.name.toLowerCase()) || item.name,
              });
            }, popupDelay);

            events.createRecommendationConsumedEventAsync({
              token: access_token,
              customerId: customerId,
              correlatorId: correlatorId,
            });
          });
      }
    });
});
