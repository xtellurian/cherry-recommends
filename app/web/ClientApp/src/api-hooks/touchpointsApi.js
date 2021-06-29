import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import {
  fetchTouchpoints,
  fetchTouchpoint,
  fetchTrackedUsersInTouchpoint,
  fetchTrackedUserTouchpoints,
  fetchTrackedUserTouchpointValues,
} from "../api/touchpointsApi";

export const useTouchpoints = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchTouchpoints({
        success: setState,
        error: (e) => setState({ error: e }),
        token,
        page,
      });
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
      fetchTouchpoint({
        success: setState,
        error: (e) => setState({ error: e }),
        token,
        id,
      });
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
      fetchTrackedUserTouchpoints({
        success: setState,
        error: (e) => setState({ error: e }),
        token,
        id,
      });
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
      fetchTrackedUserTouchpointValues({
        success: setState,
        error: (e) => setState({ error: e }),
        id,
        touchpointCommonId,
        version: version,
        token,
      });
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
      fetchTrackedUsersInTouchpoint({
        success: setState,
        error: (e) => setState({ error: e }),
        id,
        token,
      });
    }
  }, [token, id]);

  return result;
};
