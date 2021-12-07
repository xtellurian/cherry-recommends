import React from "react";
import { Link, useRouteMatch } from "react-router-dom";

import "../css/nav.css";

export const PrimaryNavigationMenu = ({ children }) => {
  return (
    <>
      <ul className="list-group list-group-flush list-group-horizontal">
        {children}
      </ul>
      <hr />
    </>
  );
};

export const NavListItem = ({ to, children }) => {
  const { url } = useRouteMatch();
  const active = to === url;

  return (
    <li className="list-group-item primary-nav-item" style={{ border: "none" }}>
      <Link className={`primary-nav-link ${active ? "active" : ""}`} to={to}>
        {children}
      </Link>
    </li>
  );
};
