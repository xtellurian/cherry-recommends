import React, { createContext } from "react";
import { setDefaultEnvironmentId } from "../api/environmentsApi";

// Get saved data from sessionStorage
const ENVIRONMENT_KEY = "x-environment";
const initialEnvironmentString = sessionStorage.getItem(ENVIRONMENT_KEY);

let environment = null;

try {
  environment = JSON.parse(initialEnvironmentString);
} catch {
  console.log("failed to parse environment information from session storage");
  sessionStorage.removeItem(ENVIRONMENT_KEY);
}

const initialState = {
  environment,
};
if (environment && environment.id) {
  // initialise the SDK
  setDefaultEnvironmentId(environment.id);
}

const Reducer = (state, action) => {
  console.log({ state, action });
  switch (action.type) {
    case "SET_ENVIRONMENT":
      console.log(`Setting environment id to ${action.environment?.id}`);
      setDefaultEnvironmentId(action.environment.id);
      // Save data to sessionStorage
      sessionStorage.setItem(
        ENVIRONMENT_KEY,
        JSON.stringify(action.environment)
      );

      return {
        ...state,
        environment: action.environment,
      };
    case "RESET_ENVIRONMENT":
      setDefaultEnvironmentId(null);
      sessionStorage.removeItem(ENVIRONMENT_KEY);
      return {
        ...state,
        environment: null,
      };
    default:
      return state;
  }
};

const Store = ({ children }) => {
  const [state, dispatch] = React.useReducer(Reducer, initialState);
  return (
    <Context.Provider value={[state, dispatch]}>{children}</Context.Provider>
  );
};

export const Context = createContext(initialState);
export default Store;
