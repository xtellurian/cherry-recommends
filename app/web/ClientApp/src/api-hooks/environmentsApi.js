import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { fetchEnvironmentsAsync } from "../api/environmentsApi";
import { Context } from "../contexts/EnvironmentStore";

export const useEnvironment = () => {
  const [state, dispatch] = React.useContext(Context);
  const setEnvironment = (environment) => {
    if (environment && environment.id !== state?.environment?.id) {
      dispatch({ type: "SET_ENVIRONMENT", environment });
    } else if (environment === null) {
      dispatch({ type: "RESET_ENVIRONMENT" });
    }
  };

  return [state.environment, setEnvironment];
};

const addCurrentBool = (result, currentId) => {
  if (result && result.items) {
    for (const element of result.items) {
      element.current = element.id === currentId;
    }
  }
  return result;
};

const defaultEnvironment = {
  name: "Default",
  id: null,
};
const addDefault = (state) => {
  state.items = [defaultEnvironment, ...state.items];
  return state;
};

export const useEnvironments = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });

  const [environment, _] = useEnvironment();
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchEnvironmentsAsync({
        token,
        page,
      })
        .then(addDefault)
        .then((s) => addCurrentBool(s, environment?.id))
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, environment]);

  return result;
};
