import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { loadingState } from "./loadingState";
import {
  fetchDestinationsAsync,
  fetchFeatureAsync,
  fetchFeaturesAsync,
  fetchTrackedUserFeaturesAsync,
  fetchTrackedUserFeatureValuesAsync,
  fetchGeneratorsAsync,
} from "../api/featuresApi";

export const useFeatures = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [state, setState] = React.useState(loadingState);
  React.useEffect(() => {
    setState(loadingState);
    if (token && page) {
      fetchFeaturesAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page]);

  return state;
};

export const useFeature = ({ id }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState(loadingState);
  React.useEffect(() => {
    setState(loadingState);
    if (token && id) {
      fetchFeatureAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return state;
};

export const useFeatureGenerators = ({ id, trigger }) => {
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

export const useTrackedUserFeatures = ({ id }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    setState(loadingState);
    if (token && id) {
      fetchTrackedUserFeaturesAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return state;
};

export const useTrackedUserFeatureValues = ({ id, feature, version }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    setState(loadingState);
    if (token && id && feature) {
      fetchTrackedUserFeatureValuesAsync({
        token,
        id,
        feature,
        version,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, feature, version]);

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
