import React from "react";
import { Link, useLocation } from "react-router-dom";
import { useTabs, useQuery } from "../../utility/utility";

const TabListItem = ({ active, tab, basePath }) => {
  const qs = useQuery();
  qs.set("tab", tab.id);
  return (
    <li className="nav-item">
      <Link
        className={`nav-link ${active && "active"}`}
        aria-current="page"
        to={`${basePath}?${qs.toString()}`}
      >
        {tab.name || tab.id}
      </Link>
    </li>
  );
};

export const Tabs = ({ tabs, defaultTab }) => {
  const currentTab = useTabs(defaultTab || tabs[0].id);
  const { pathname } = useLocation();
  return (
    <ul className="nav nav-pills nav-fill">
      {tabs.map((t) => (
        <TabListItem
          key={t.id}
          {...t}
          tab={t}
          active={t.id === currentTab}
          basePath={pathname}
        />
      ))}
    </ul>
  );
};

export const TabActivator = ({ tabId, defaultTab, children }) => {
  const tab = useTabs(defaultTab);
  if (tab === tabId) {
    return <React.Fragment>{children}</React.Fragment>;
  } else {
    return <React.Fragment />;
  }
};
