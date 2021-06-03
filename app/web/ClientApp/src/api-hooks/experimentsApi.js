import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import {
  fetchExperiments,
  fetchExperimentResults,
} from "../api/experimentsApi";

export const useExperiments = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchExperiments({
        success: setState,
        error: console.log,
        token,
        page,
      });
    }
  }, [token, page]);

  return { result };
};

export const useExperimentResults = ({ id }) => {
  const token = useAccessToken();
  const [experimentResults, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchExperimentResults({
        success: setState,
        error: console.log,
        token,
        id,
      });
    }
  }, [token, id]);

  return { experimentResults };
};
