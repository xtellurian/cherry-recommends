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
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchTrackedUser({
        success: setState,
        error: (e) => setState({ error: e }),
        token,
        id,
      });
    }
  }, [token, id]);

  return result;
};
