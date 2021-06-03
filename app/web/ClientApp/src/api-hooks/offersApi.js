import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { fetchOffers, fetchOffer } from "../api/offersApi";

export const useOffers = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchOffers({
        success: setState,
        error: console.log,
        token,
        page,
      });
    }
  }, [token, page]);

  return { result };
};

export const useOffer = ({ id }) => {
  const token = useAccessToken();
  const [offer, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchOffer({
        success: setState,
        error: console.log,
        token,
        id,
      });
    }
  }, [token, id]);

  return { offer };
};
