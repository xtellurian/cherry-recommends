import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import {
  fetchModelRegistration,
  fetchModelRegistrations,
} from "../api/modelRegistrationsApi";

export const useModelRegistrations = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchModelRegistrations({
        success: setState,
        error: console.log,
        token,
        page,
      });
    }
  }, [token, page]);
  return { result };
};

export const useModelRegistration = ({ id }) => {
  const token = useAccessToken();
  const [model, setState] = React.useState();
  React.useEffect(() => {
    if (token && id) {
      fetchModelRegistration({
        success: setState,
        error: console.log,
        token,
        id,
      });
    }
  }, [token, id]);

  return { model };
};
