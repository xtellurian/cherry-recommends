import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { fetchSegments, fetchSegment } from "../api/segmentsApi";

export const useSegments = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchSegments({
        success: setState,
        error: console.log,
        token,
        page,
      });
    }
  }, [token, page]);

  return { result };
};

export const useSegment = ({ id }) => {
  const token = useAccessToken();
  const [segment, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchSegment({
        success: setState,
        error: console.log,
        token,
        id,
      });
    }
  }, [token, id]);

  return { segment };
};
