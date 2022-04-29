import { executeFetch } from "./client/apiClient";

export const fetchEventSummaryAsync = async ({ token }) => {
  return await executeFetch({
    path: "api/datasummary/events",
    token,
  });
};

export const fetchEventKindNamesAsync = async ({ token }) => {
  return await executeFetch({
    path: `api/datasummary/event-kind-names`,
    token,
  });
};
export const fetchEventKindSummaryAsync = async ({ token, kind }) => {
  return await executeFetch({
    path: `api/datasummary/event-kind/${kind}`,
    token,
  });
};

export const fetchEventTimelineAsync = async ({ token, kind, eventType }) => {
  return await executeFetch({
    path: `api/datasummary/events/timeline/${kind}/${eventType}`,
    token,
  });
};

export const fetchGeneralSummaryAsync = async ({ token }) => {
  return await executeFetch({
    path: "api/DataSummary/GeneralSummary",
    token,
  });
};

export const fetchLatestActionsAsync = async ({ token }) => {
  return await executeFetch({
    path: "api/datasummary/actions",
    token,
  });
};
