import React from "react";
import { useAccessToken } from "./token";
import { fetchRule, fetchRules } from "../api/rulesApi";

export const useRules = ({ segmentId }) => {
  const token = useAccessToken();
  const [rules, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchRules({
        success: setState,
        error: console.log,
        token,
        segmentId,
      });
    }
  }, [token, segmentId]);

  return { rules };
};
export const useRule = ({ ruleId }) => {
  const token = useAccessToken();
  const [rules, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchRule({
        success: setState,
        error: console.log,
        token,
        ruleId,
      });
    }
  }, [token, ruleId]);

  return { rules };
};
