import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useLocation } from "react-router-dom";

import { useAuth } from "../../utility/useAuth";
import { useTokenScopes } from "../../api-hooks/token";
import { useAuthenticatedIA } from "./MenuIA";
import { useMemberships } from "../tenants/MembershipsProvider";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
import { Navigation } from "../molecules";
import {
  Animation,
  SliderAnimation,
  RotateAnimation,
} from "../molecules/Animations";

import "./SideNavMenu.css";

const MenuItem = ({ active, label, icon, activeIcon = true }) => {
  return (
    <Animation
      className="menu-item selectable p-3 d-flex align-items-center justify-content-between"
      animatedStyles={{
        borderRight: active ? "4px solid #e5008a" : "0px solid white",
      }}
    >
      <div className="d-flex align-items-center">
        <div className="icon-wrapper mr-3">
          {icon ? <img src={icon} alt={label} className="icon" /> : null}
        </div>
        {label}
      </div>

      {activeIcon ? (
        <RotateAnimation rotateZ={active ? 90 : 0}>
          <img src="/icons/angle-right.svg" alt="Angle icon" className="icon" />
        </RotateAnimation>
      ) : null}
    </Animation>
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

// Note: The `/settings/integrations` and `/recommenders` weren't updated yet to follow the route format
// thus, need to have a customized condition

export const SideNavMenu = ({ multitenant }) => {
  const location = useLocation();
  const { isAuthenticated } = useAuth();
  const scopes = useTokenScopes();
  const authIA = useAuthenticatedIA(scopes);
  const memberships = useMemberships();

  const [activeMenu, setActiveMenu] = useState(null);
  const [activeMenuItem, setActiveMenuItem] = useState(null);

  const handleMenuChange = useCallback((newActiveMenu) => {
    setActiveMenu((oldActiveMenu) => {
      // Note: this is a special condition for old routes
      if (oldActiveMenu === "settings") {
        return newActiveMenu !== "integrations" ? newActiveMenu : null;
      }

      if (oldActiveMenu === "campaigns") {
        // list of menu where recommendation route exist
        const recommenderMenu = ["promotions", "parameters"];
        return !recommenderMenu.includes(newActiveMenu) ? newActiveMenu : null;
      }

      return newActiveMenu !== oldActiveMenu ? newActiveMenu : null;
    });
  }, []);

  const isActiveMenu = useCallback(
    (menu) => {
      // Note: this is a special condition for old routes
      if (activeMenu === "campaigns") {
        return activeMenuItem.includes(menu.id.slice(0, -1));
      }

      // Note: this is a special condition for old routes
      if (activeMenu === "settings" && activeMenuItem === "integrations") {
        return menu.id === "integrations" || menu.id === "settings";
      }

      return activeMenu === menu.id;
    },
    [activeMenu, activeMenuItem]
  );

  const isActiveMenuItem = useCallback(
    (menuItem) => {
      // Note: this is a special condition for old routes
      if (activeMenuItem === "integrations") {
        return menuItem.to.hash === location.hash.slice(1);
      }

      return activeMenuItem === menuItem.id;
    },
    [activeMenuItem, location.hash]
  );

  const authenticatedIA = useMemo(
    () =>
      authIA.map((ia) => {
        return ia;
      }),
    [authIA]
  );

  useEffect(() => {
    const pathname = location.pathname.slice(1).split("/");

    // in multitenant the pathname[0] is the tenant name
    if (multitenant) {
      setActiveMenu(pathname[1]);
      setActiveMenuItem(pathname[2]);
      return;
    }

    setActiveMenu(pathname[0]);
    setActiveMenuItem(pathname[1]);
  }, [location, multitenant]);

  return (
    <nav className="sidebar disable-select bg-white box-shadow">
      {isAuthenticated
        ? authenticatedIA.map((item) => (
            <div key={item.id} className="border-bottom">
              {item.id === "getting-started" ? (
                <Navigation to={item.to.pathname}>
                  <MenuItem
                    active={activeMenu === item.id}
                    label={item.name}
                    icon={item.icon}
                    activeIcon={item.activeIcon}
                  />
                </Navigation>
              ) : (
                <div onClick={() => handleMenuChange(item.id)}>
                  <MenuItem
                    active={isActiveMenu(item)}
                    label={item.name}
                    icon={item.icon}
                    activeIcon={item.activeIcon}
                  />
                </div>
              )}

              <SliderAnimation show={isActiveMenu(item)}>
                {item.items.map((menuItem) => (
                  <Navigation key={menuItem.id} to={menuItem.to}>
                    <SubMenuItem
                      active={isActiveMenuItem(menuItem)}
                      label={menuItem.name}
                    />
                  </Navigation>
                ))}
              </SliderAnimation>
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
