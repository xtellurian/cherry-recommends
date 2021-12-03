import React from "react";
import { Link, useLocation } from "react-router-dom";
import { useTabs, useQuery } from "../../../utility/utility";

const QueryStringTabListItem = ({ active, tab, basePath }) => {
  const qs = useQuery();
  qs.set("tab", tab.id);
  return (
    <li className="nav-item">
      <Link
        className={`nav-link ${active && "active"}`}
        aria-current="page"
        to={`${basePath}?${qs.toString()}`}
      >
        {tab.label || tab.name || tab.id}
      </Link>
    </li>
  );
};

export const Tabs = ({ tabs, defaultTabId }) => {
  const currentTab = useTabs(defaultTabId || tabs[0].id);
  const { pathname } = useLocation();
  return (
    <ul className="nav nav-tabs nav-fill mb-2">
      {tabs.map((t) => (
        <QueryStringTabListItem
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

export const TabActivator = ({ tabId, defaultTabId, children }) => {
  const tab = useTabs(defaultTabId);
  if (tab === tabId) {
    return <React.Fragment>{children}</React.Fragment>;
  } else {
    return <React.Fragment />;
  }
};
