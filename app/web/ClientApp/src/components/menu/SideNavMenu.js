import React, { useCallback, useMemo } from "react";
import { useLocation } from "react-router-dom";

import { useAuth } from "../../utility/useAuth";
import { useTokenScopes } from "../../api-hooks/token";
import { useAuthenticatedIA } from "./MenuIA";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { Navigation } from "../molecules";
import { useMemberships } from "../tenants/MembershipsProvider";

import "./SideNavMenu.css";

const MenuItem = ({ active, label, icon, activeIcon = true }) => {
  const activeClassName = active ? "active border-bottom-0" : "";
  const activeIconSrc = active
    ? "/icons/angle-down.svg"
    : "/icons/angle-right.svg";

  return (
    <div
      className={`menu-item selectable p-3 d-flex align-items-center justify-content-between ${activeClassName}`}
    >
      <div className="d-flex align-items-center">
        <div className="icon-wrapper mr-3">
          {icon ? <img src={icon} alt={label} className="icon" /> : null}
        </div>
        {label}
      </div>
      {activeIcon ? (
        <img src={activeIconSrc} alt="Angle right" className="icon" />
      ) : null}
    </div>
  );
};

const SubMenuItem = ({ active, label }) => (
  <div className={`menu-subitem selectable bg-light ${active ? "active" : ""}`}>
    <div className="p-3 d-flex align-items-center">
      <div className="icon-wrapper mr-3" />
      {label}
    </div>
  </div>
);

export const SideNavMenu = () => {
  const location = useLocation();
  const { isAuthenticated } = useAuth();
  const scopes = useTokenScopes();
  const authIA = useAuthenticatedIA(scopes);
  const memberships = useMemberships();

  const isActive = useCallback(
    ({ hash }) => {
      return location.hash.includes(hash);
    },
    [location.hash]
  );

  const authenticatedIA = useMemo(
    () =>
      authIA.map((ia) => {
        return ia;
      }),
    [authIA]
  );

  return (
    <nav className="sidebar disable-select bg-white box-shadow">
      {isAuthenticated
        ? authenticatedIA.map((item) => (
            <div key={item.name} className="border-bottom">
              <Navigation
                to={
                  isActive({ hash: item.to.hash }) ? item.to.pathname : item.to
                }
              >
                <MenuItem
                  active={isActive({ hash: item.to.hash })}
                  label={item.name}
                  icon={item.icon}
                  activeIcon={item.activeIcon}
                />
              </Navigation>
              {isActive({ hash: item.to.hash }) &&
                item.items.map((subitem) => (
                  <Navigation key={subitem.name} to={subitem.to}>
                    <SubMenuItem
                      active={location.hash === subitem.to.hash}
                      label={subitem.name}
                    />
                  </Navigation>
                ))}
            </div>
          ))
        : null}

      <div className="sticky-bottom">
        {memberships.length > 1 ? (
          <Navigation to="/" escapeTenant={true}>
            <div
              className={`menu-item selectable p-3 d-flex align-items-center justify-content-between `}
            >
              <div className="d-flex align-items-center">
                <div className="icon-wrapper mr-3">
                  <FontAwesomeIcon icon={faArrowLeft} />
                </div>
                All Tenants
              </div>
            </div>
          </Navigation>
        ) : null}
      </div>
    </nav>
  );
};
