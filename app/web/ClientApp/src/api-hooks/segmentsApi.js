import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { fetchSegmentsAsync, fetchSegmentAsync } from "../api/segmentsApi";

export const useSegments = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchSegmentsAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page]);

  return { result };
};

export const useSegment = ({ id }) => {
  const token = useAccessToken();
  const [segment, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchSegmentAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return { segment };
};
