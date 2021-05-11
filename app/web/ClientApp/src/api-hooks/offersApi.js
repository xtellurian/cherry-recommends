import React from "react";
import { useAccessToken } from "./token";
import { fetchOffers, fetchOffer } from "../api/offersApi";

export const useOffers = () => {
  const token = useAccessToken();
  const [offers, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchOffers({
        success: setState,
        error: console.log,
        token,
      });
    }
  }, [token]);

  return { offers };
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
  }, [token]);

  return { offer };
};
