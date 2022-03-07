import React from "react";
import {
  fetchPromotionsAsync,
  fetchPromotionAsync,
} from "../api/promotionsApi";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { useEnvironmentReducer } from "./environmentsApi";

export const usePromotions = (p) => {
  const { trigger, searchTerm, benefitType, promotionType, weeksAgo } = p || {}; // ensure this works in the case of p === undefinfed
  const token = useAccessToken();
  const page = usePagination();
  const [environment] = useEnvironmentReducer();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchPromotionsAsync({
        token,
        page,
        searchTerm,
        benefitType,
        promotionType,
        weeksAgo,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, trigger, environment, searchTerm]);

  return result;
};

export const usePromotion = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchPromotionAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return result;
};

export const useGlobalStartingPromotion = (props) => {
  return usePromotion({ id: -1, trigger: props?.trigger });
};
