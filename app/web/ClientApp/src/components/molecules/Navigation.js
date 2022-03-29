import React from "react";
import { Link as ReactRouterDOMLink } from "react-router-dom";

import { useNavigation } from "../../utility/useNavigation";

// This works similar to Link but retains the other URL parts aside from the one being changed.
// `to` can be a string or an object with props `{ pathname, search, hash, state }`
// Reference https://v5.reactrouter.com/web/api/Link
export const Navigation = ({ to, ...restProps }) => {
  const { appendCurrentURL } = useNavigation();
  return <ReactRouterDOMLink to={appendCurrentURL(to)} {...restProps} />;
};
