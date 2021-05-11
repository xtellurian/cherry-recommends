import React from "react";
import { useAccessToken } from "./token";
import { fetchSegments, fetchSegment } from "../api/segmentsApi";

export const useSegments = () => {
  const token = useAccessToken();
  const [segments, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchSegments({
        success: setState,
        error: console.log,
        token,
      });
    }
  }, [token]);

  return { segments };
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
  }, [token]);

  return { segment };
};
