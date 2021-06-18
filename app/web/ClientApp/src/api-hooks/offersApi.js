import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { fetchOffers, fetchOffer } from "../api/offersApi";

export const useOffers = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchOffers({
        success: setState,
        error: (error) => setState({ error }),
        token,
        page,
      });
    }
  }, [token, page]);

  return result;
};

export const useOffer = ({ id }) => {
  const token = useAccessToken();
  const [offer, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchOffer({
        success: setState,
        error: (error) => setState({ error }),
        token,
        id,
      });
    }
  }, [token, id]);

  return offer;
};
