import React from "react";
import {
  fetchPromotionsCampaignsAsync,
  fetchPromotionsCampaignAsync,
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
  fetchCampaignChannelsAsync,
  fetchOffersAsync,
  fetchPromotionsRecommendationAsync,
  fetchArgumentsAsync,
  fetchChoosePromotionArgumentRulesAsync,
  fetchChooseSegmentArgumentRulesAsync,
  fetchARPOReportAsync,
  fetchOfferConversionRateReportAsync,
  fetchAPVReportAsync,
} from "../api/promotionsCampaignsApi";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { useEnvironmentReducer } from "./environmentsApi";

export const usePromotionsCampaigns = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment] = useEnvironmentReducer();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchPromotionsCampaignsAsync({
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
    if (token && id) {
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

export const usePromotionsCampaign = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchPromotionsCampaignAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return result;
};

export const usePromotionsRecommendations = ({ id, pageSize, trigger }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchPromotionsRecommendationsAsync({
        token,
        id,
        page,
        pageSize,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, page, pageSize, trigger]);

  return result;
};

export const useInvokationLogs = ({ id, trigger, pageSize }) => {
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
        pageSize,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, page, pageSize, trigger]);

  return result;
};

export const useLinkedRegisteredModel = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
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
    if (token && id) {
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
    if (token && id) {
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
    if (token && id) {
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

export const useChoosePromotionArgumentRules = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchChoosePromotionArgumentRulesAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};

export const useChooseSegmentArgumentRules = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchChooseSegmentArgumentRulesAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};

export const useArgumentRules = ({ id, trigger }) => {
  const promoRules = useChoosePromotionArgumentRules({ id, trigger });
  const segmentRules = useChooseSegmentArgumentRules({ id, trigger });
  if (promoRules.loading || segmentRules.loading) {
    return { loading: true };
  } else {
    return [...promoRules, ...segmentRules].sort((a, b) => a.id - b.id);
  }
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

// the trigger param is a different trigger
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
    if (token && id) {
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
    if (token && id) {
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
    if (token && id) {
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
    if (token && id) {
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
    if (token && id) {
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

export const useCampaignChannels = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchCampaignChannelsAsync({
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
    if (token && id) {
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
  }, [token, id, page, pageSize, environment]);

  return state;
};

export const useARPOReport = ({ id }) => {
  const token = useAccessToken();
  const [environment] = useEnvironmentReducer();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchARPOReportAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, environment]);

  return state;
};

export const useAPVReport = ({ id }) => {
  const token = useAccessToken();
  const [environment] = useEnvironmentReducer();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchAPVReportAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, environment]);

  return state;
};

export const useOfferConversionRateReport = ({ id }) => {
  const token = useAccessToken();
  const [environment] = useEnvironmentReducer();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchOfferConversionRateReportAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, environment]);

  return state;
};
