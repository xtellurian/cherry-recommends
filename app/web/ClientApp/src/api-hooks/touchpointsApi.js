import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import {
  fetchTouchpointsAsync,
  fetchTouchpointAsync,
  fetchTrackedUsersInTouchpointAsync,
  fetchTrackedUserTouchpointsAsync,
  fetchTrackedUserTouchpointValuesAsync,
} from "../api/touchpointsApi";

export const useTouchpoints = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchTouchpointsAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page]);

  return result;
};

export const useTouchpoint = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchTouchpointAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};

export const useTrackedUserTouchpoints = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchTrackedUserTouchpointsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};

export const useTrackedUserTouchpointValues = ({
  id,
  touchpointCommonId,
  version,
}) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id && touchpointCommonId) {
      fetchTrackedUserTouchpointValuesAsync({
        id,
        touchpointCommonId,
        version: version,
        token,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, touchpointCommonId, version]);

  return result;
};

export const useTrackedUsersInTouchpoint = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchTrackedUsersInTouchpointAsync({
        id,
        token,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};
