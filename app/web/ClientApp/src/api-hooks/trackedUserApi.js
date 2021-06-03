import React from "react";
import { usePagination } from "../utility/utility";
import { useAccessToken } from "./token";
import { fetchTrackedUsers, fetchTrackedUser } from "../api/trackedUsersApi";

export const useTrackedUsers = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setResults] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchTrackedUsers({
        success: setResults,
        error: console.log,
        token,
        page,
      });
    }
  }, [token, page]);

  return { result };
};

export const useTrackedUser = ({ id }) => {
  const token = useAccessToken();
  const [result, setResults] = React.useState();
  React.useEffect(() => {
    if (token && id) {
      fetchTrackedUser({
        success: setResults,
        error: console.log,
        token,
        id,
      });
    }
  }, [token, id]);

  return { trackedUser: result };
};
