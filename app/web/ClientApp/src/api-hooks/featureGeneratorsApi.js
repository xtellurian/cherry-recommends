import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { fetchFeatureGeneratorsAsync } from "../api/featureGeneratorsApi";

export const useFeatureGenerators = ({ trigger }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [state, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && page) {
      fetchFeatureGeneratorsAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, trigger]);

  return state;
};
