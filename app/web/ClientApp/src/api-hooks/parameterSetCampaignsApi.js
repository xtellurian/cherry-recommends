import React from "react";
import {
  fetchParameterSetCampaignsAsync,
  fetchParameterSetCampaignAsync,
  fetchParameterSetRecommendationsAsync,
  fetchLinkedRegisteredModelAsync,
  fetchInvokationLogsAsync,
  fetchTargetVariablesAsync,
  fetchDestinationsAsync,
  fetchTriggerAsync,
  fetchLearningMetricsAsync,
  fetchStatisticsAsync,
  fetchReportImageBlobUrlAsync,
  fetchArgumentsAsync,
} from "../api/parameterSetCampaignsApi";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { useEnvironmentReducer } from "./environmentsApi";

export const useParameterSetCampaigns = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment] = useEnvironmentReducer();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchParameterSetCampaignsAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, environment]);

  return result;
};

export const useParameterSetCampaign = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (id && token) {
      fetchParameterSetCampaignAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return result;
};

export const useParameterSetRecommendations = ({ id, pageSize }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchParameterSetRecommendationsAsync({
        token,
        id,
        pageSize,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};

export const useInvokationLogs = ({ id }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchInvokationLogsAsync({
        token,
        id,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, page]);

  return result;
};

export const useLinkedRegisteredModel = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchLinkedRegisteredModelAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};

export const useTargetVariables = ({ id, name }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchTargetVariablesAsync({
        token,
        id,
        name,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, name]);

  return state;
};

export const useArguments = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchArgumentsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};

export const useDestinations = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchDestinationsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};

export const useTrigger = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchTriggerAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};

export const useLearningMetrics = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchLearningMetricsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};

export const useStatistics = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchStatisticsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};

export const useReportImageBlobUrl = ({ id }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchReportImageBlobUrlAsync({
        token,
        id,
      })
        .then((url) => setState({ url }))
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return state;
};
