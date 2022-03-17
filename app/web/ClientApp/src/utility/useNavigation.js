import { useCallback } from "react";
import { useHistory } from "react-router-dom";

export const useNavigation = () => {
  const history = useHistory();

  const appendCurrentURL = useCallback(
    (to) => {
      if (typeof to === "string") {
        return {
          ...history.location,
          pathname: to,
        };
      }

      if (typeof to === "object" && to !== null) {
        return {
          ...history.location,
          ...to,
        };
      }

      return "";
    },
    [history.location]
  );

  const navigate = useCallback(
    (to) => {
      const newURL = appendCurrentURL(to);
      history.push(newURL);
    },
    [history, appendCurrentURL]
  );

  return {
    appendCurrentURL,
    navigate,
  };
};
