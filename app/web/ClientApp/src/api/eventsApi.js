// const defaultHeaders = { "Content-Type": "application/json" };
// import { getAccessToken } from "./auth";
// export const trackOfferAccepted = async ({ success, error, offerId }) => {
//   if (offerId) {
//     await trackEvent({
//       success,
//       error,
//       events: [
//         {
//           key: "Offer Accepted",
//           logicalValue: offerId,
//         },
//       ],
//     });
//   } else {
//     error("offer has no identifier");
//   }
// };

// export const trackEvent = async ({ success, error, events }) => {
//   const user = await authService.getUser();
//   events = events.map((e) => {
//     return { ...e, trackedUserExternalId: user.name };
//   });

//   const token = await authService.getAccessToken();
//   const response = await fetch("api/events", {
//     headers: !token
//       ? defaultHeaders
//       : { ...defaultHeaders, Authorization: `Bearer ${token}` },
//     method: "post",
//     body: JSON.stringify(events),
//   });
//   if (response.ok) {
//     const results = await response.json();
//     success(results);
//   } else {
//     error(response.statusText);
//   }
// };
