import React from "react";
import { useAccessToken } from "./token";
import {
  fetchExperiments,
  fetchExperimentResults,
} from "../api/experimentsApi";

export const useExperiments = () => {
  const token = useAccessToken();
  const [experiments, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchExperiments({
        success: setState,
        error: console.log,
        token,
      });
    }
  }, [token]);

  return { experiments };
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
  }, [token]);

  return { experimentResults };
};
