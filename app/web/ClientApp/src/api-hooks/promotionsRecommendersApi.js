import React from "react";
import {
  fetchPromotionsRecommendersAsync,
  fetchPromotionsRecommenderAsync,
  fetchLinkedRegisteredModelAsync,
  fetchInvokationLogsAsync,
  fetchPromotionsRecommendationsAsync,
  fetchTargetVariablesAsync,
  fetchPromotionsAsync,
  fetchStatisticsAsync,
  fetchDestinationsAsync,
  getBaselinePromotionAsync,
  fetchTriggerAsync,
  fetchLearningMetricsAsync,
  fetchReportImageBlobUrlAsync,
  fetchPerformanceAsync,
  fetchAudienceAsync,
  fetchPromotionOptimiserAsync,
  fetchRecommenderChannelsAsync,
  fetchOffersAsync,
  fetchPromotionsRecommendationAsync,
  fetchArgumentsAsync,
} from "../api/promotionsRecommendersApi";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { useEnvironmentReducer } from "./environmentsApi";

export const usePromotionsRecommenders = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment] = useEnvironmentReducer();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchPromotionsRecommendersAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, environment]);
  return result;
};

export const useBaselinePromotion = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      getBaselinePromotionAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return result;
};

export const usePromotionsRecommender = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchPromotionsRecommenderAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return result;
};

export const usePromotionsRecommendations = ({ id, pageSize }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchPromotionsRecommendationsAsync({
        token,
        id,
        page,
        pageSize,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, page, pageSize]);

  return result;
};

export const useInvokationLogs = ({ id }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
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

export const usePromotions = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchPromotionsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

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
    if (token) {
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

// the trigger param is a different trigger
export const useTrigger = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
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

export const useReportImageBlobUrl = ({ id, trigger }) => {
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
  }, [token, id, trigger]);

  return state;
};

export const usePerformance = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchPerformanceAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};

export const useAudience = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchAudienceAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};

export const useOptimiser = ({ id }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchPromotionOptimiserAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return state;
};

export const useRecommenderChannels = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchRecommenderChannelsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};

export const usePromotionsRecommendation = ({ recommendationId, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && recommendationId) {
      fetchPromotionsRecommendationAsync({
        token,
        recommendationId,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, recommendationId, trigger]);

  return state;
};

export const useOffers = ({ id, pageSize, offerState }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment] = useEnvironmentReducer();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchOffersAsync({
        token,
        id,
        page,
        pageSize,
        offerState,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, pageSize, environment]);

  return state;
};
