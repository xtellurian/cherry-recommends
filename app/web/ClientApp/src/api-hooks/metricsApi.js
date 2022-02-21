import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { loadingState } from "./loadingState";
import {
  fetchDestinationsAsync,
  fetchMetricAsync,
  fetchMetricsAsync,
  fetchCustomersMetricsAsync,
  fetchCustomersMetricAsync,
  fetchGeneratorsAsync,
  fetchAggregateMetricValuesNumericAsync,
  fetchAggregateMetricValuesStringAsync,
} from "../api/metricsApi";

export const useMetrics = ({ scope } = {}) => {
  const token = useAccessToken();
  const page = usePagination();
  const [state, setState] = React.useState(loadingState);
  React.useEffect(() => {
    setState(loadingState);
    if (token && page) {
      fetchMetricsAsync({
        token,
        page,
        scope,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page]);

  return state;
};

export const useMetric = ({ id }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState(loadingState);
  React.useEffect(() => {
    setState(loadingState);
    if (token && id) {
      fetchMetricAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return state;
};

export const useMetricGenerators = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState(loadingState);
  React.useEffect(() => {
    setState(loadingState);
    if (token && id) {
      fetchGeneratorsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};

export const useCustomerMetrics = ({ id }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    setState(loadingState);
    if (token && id) {
      fetchCustomersMetricsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return state;
};

export const useCustomersMetrics = ({ id, metricId, version }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    setState(loadingState);
    if (token && id && metricId) {
      fetchCustomersMetricAsync({
        token,
        id,
        metricId,
        version,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, metricId, version]);

  return state;
};

export const useAggregateMetricsNumeric = ({ id }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    setState(loadingState);
    if (token && id) {
      fetchAggregateMetricValuesNumericAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return state;
};

export const useAggregateMetricsString = ({ id }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    setState(loadingState);
    if (token && id) {
      fetchAggregateMetricValuesStringAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return state;
};

export const useAggregateMetrics = ({ id, valueType }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    setState(loadingState);
    if (token && id) {
      if (valueType === "numeric") {
        fetchAggregateMetricValuesNumericAsync({
          token,
          id,
        })
          .then(setState)
          .catch((error) => setState({ error }));
      } else if (valueType === "categorical") {
        fetchAggregateMetricValuesStringAsync({
          token,
          id,
        })
          .then(setState)
          .catch((error) => setState({ error }));
      } else {
        setState({ loading: false });
      }
    }
  }, [token, id, valueType]);

  return state;
};

export const useDestinations = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    setState(loadingState);
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

export const useGlobalStartingMetric = () => {
  return useMetric({ id: 100 });
};
