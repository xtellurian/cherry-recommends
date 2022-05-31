import React from "react";
import { useLocation } from "react-router-dom";
import { Navigation } from "../Navigation";

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
  const location = useLocation();
  const active = to === location.pathname;

  return (
    <li className="list-group-item primary-nav-item" style={{ border: "none" }}>
      <Navigation
        className={`primary-nav-link ${active ? "active" : ""}`}
        to={to}
      >
        {children}
      </Navigation>
    </li>
  );
};
