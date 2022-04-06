import React, { useCallback, useMemo } from "react";
import { useLocation, Link } from "react-router-dom";

import { useAuth } from "../../utility/useAuth";
import { useTokenScopes } from "../../api-hooks/token";
import { useIntegratedSystems } from "../../api-hooks/integratedSystemsApi";

import { useAuthenticatedIA, hash as hashes } from "./MenuIA";

import "./SideNavMenu.css";
import { Navigation } from "../molecules";

const MenuItem = ({ active, label, icon }) => {
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
      <img src={activeIconSrc} alt="Angle right" className="icon" />
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
  const integratedSystems = useIntegratedSystems();
  const authIA = useAuthenticatedIA(scopes);

  const isActive = useCallback(
    ({ hash }) => {
      return location.hash.includes(hash);
    },
    [location.hash]
  );

  const authenticatedIA = useMemo(
    () =>
      authIA.map((ia) => {
        // add integrated systems as subitems of integration menu
        if (ia.name === "Integrations") {
          const _integratedSystems =
            integratedSystems?.items?.length > 0 ? integratedSystems.items : [];

          const newItems = _integratedSystems.reduce((acc, curr) => {
            return [
              ...acc,
              {
                name: curr.name,
                to: {
                  pathname: `/settings/integrations/detail/${curr.id}`,
                  hash: hashes.integration,
                },
              },
            ];
          }, []);

          return { ...ia, items: newItems };
        }

        return ia;
      }),
    [authIA, integratedSystems.items]
  );

  return (
    <nav className="sidebar disable-select bg-white box-shadow">
      {isAuthenticated &&
        authenticatedIA.map((item) => (
          <div key={item.name} className="border-bottom">
            <Navigation
              to={isActive({ hash: item.to.hash }) ? item.to.pathname : item.to}
            >
              <MenuItem
                active={isActive({ hash: item.to.hash })}
                label={item.name}
                icon={item.icon}
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
        ))}
    </nav>
  );
};
