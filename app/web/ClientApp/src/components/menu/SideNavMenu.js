import React from "react";
import { useLocation, Link } from "react-router-dom";

import { useAuth } from "../../utility/useAuth";
import { useTokenScopes } from "../../api-hooks/token";
import { useIntegratedSystems } from "../../api-hooks/integratedSystemsApi";

import { getAuthenticatedIA } from "./MenuIA";

import "./SideNavMenu.css";

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
          {icon ? (
            <img src={icon} alt={label} role="img" className="icon" />
          ) : null}
        </div>
        {label}
      </div>
      <img src={activeIconSrc} role="img" alt="Angle right" className="icon" />
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

  const authenticatedIA = getAuthenticatedIA(scopes).map((ia) => {
    // add integrated systems as subitems of integration menu
    if (ia.name === "Integrations") {
      const _integratedSystems =
        integratedSystems?.items?.length > 0 ? integratedSystems.items : [];

      const newItems = _integratedSystems.reduce((acc, curr) => {
        return [
          ...acc,
          {
            name: curr.name,
            to: `/settings/integrations/detail/${curr.id}`,
          },
        ];
      }, []);

      return { ...ia, items: newItems };
    }

    return ia;
  });

  return (
    <nav className="sidebar disable-select bg-white box-shadow">
      {isAuthenticated &&
        authenticatedIA.map((item) => (
          <div key={item.name} className="border-bottom">
            <Link
              to={location.hash === item.to.hash ? item.to.pathname : item.to}
            >
              <MenuItem
                active={location.hash === item.to.hash}
                label={item.name}
                icon={item.icon}
              />
            </Link>
            {location.hash === item.to.hash &&
              item.items.map((subitem) => (
                <Link
                  key={subitem.to}
                  to={(location) => ({ ...location, pathname: subitem.to })}
                >
                  <SubMenuItem
                    active={location.pathname === subitem.to}
                    label={subitem.name}
                  />
                </Link>
              ))}
          </div>
        ))}
    </nav>
  );
};
