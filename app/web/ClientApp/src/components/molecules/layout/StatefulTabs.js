import React from "react";

const StateTabListItem = ({ active, tab, onClick }) => {
  return (
    <li className="nav-item">
      <div
        onClick={onClick}
        className={`nav-link ${active && "active"}`}
        style={{ cursor: "pointer" }}
        aria-current="page"
      >
        {tab.label || tab.name || tab.id}
      </div>
    </li>
  );
};

export const StatefulTabs = ({ currentTabId, setCurrentTabId, tabs }) => {
  return (
    <ul className="nav nav-tabs nav-fill mb-2">
      {tabs.map((t) => (
        <StateTabListItem
          key={t.id}
          {...t}
          tab={t}
          active={t.id === currentTabId}
          onClick={() => setCurrentTabId(t.id)}
        />
      ))}
    </ul>
  );
};

export const TabActivator = ({ currentTabId, tabId, children }) => {
  if (currentTabId === tabId) {
    return <React.Fragment>{children}</React.Fragment>;
  } else {
    return <React.Fragment />;
  }
};
