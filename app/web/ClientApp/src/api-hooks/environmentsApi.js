import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { fetchEnvironmentsAsync } from "../api/environmentsApi";
import { Context } from "../contexts/EnvironmentStore";

export const useEnvironmentReducer = () => {
  const [state, dispatch] = React.useContext(Context);
  const setEnvironment = (environment) => {
    if (environment && environment.id !== state?.environment?.id) {
      dispatch({ type: "SET_ENVIRONMENT", environment });
    } else if (!environment) {
      dispatch({ type: "RESET_ENVIRONMENT" });
    }
  };

  return [state.environment, setEnvironment];
};

const addCurrentBool = (result, currentId) => {
  if (result && result.items) {
    for (const env of result.items) {
      env.current = env.id === currentId;
      if (!env.id && !currentId) {
        env.current = true; // the default environment
      }
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

export const useEnvironments = (props) => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });

  const [environment] = useEnvironmentReducer();
  React.useEffect(() => {
    let mounted = true;
    if (token) {
      fetchEnvironmentsAsync({
        token,
        page,
      })
        .then((s) => {
          if (mounted) {
            addDefault(s);
            const environments = addCurrentBool(s, environment?.id);
            setState(environments);
          }
        })
        .catch((error) => {
          if (mounted) {
            setState({ error });
          }
        });
    }
    return () => (mounted = false);
  }, [token, page, environment, props?.trigger]);

  return result;
};
