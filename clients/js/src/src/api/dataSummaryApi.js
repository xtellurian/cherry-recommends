import { executeFetch } from "./client/apiClient";

export const fetchEventSummaryAsync = async ({ token }) => {
  return await executeFetch({
    path: "api/datasummary/events",
    token,
  });
};

export const fetchEventTimelineAsync = async ({ token, kind, eventType }) => {
  return await executeFetch({
    path: `api/datasummary/events/timeline/${kind}/${eventType}`,
    token,
  });
};

export const fetchDashboardAsync = async ({ token, scope }) => {
  return await executeFetch({
    path: "api/datasummary/dashboard",
    token,
    query: { scope },
  });
};

export const fetchLatestActionsAsync = async ({ token }) => {
  return await executeFetch({
    path: "api/datasummary/actions",
    token,
  });
};
