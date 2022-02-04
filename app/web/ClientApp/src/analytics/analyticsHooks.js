import Analytics from "analytics";
import React, { createContext, useState, useContext } from "react";

const analyticsContext = createContext();

export const AnalyticsProvider = (props) => {
  const [commonProps, setCommonProps] = useState();
  const [_plugins, setPlugins] = useState([]);
  const [_config, setConfig] = useState({ app: "CherryAI", debug: false });
  const [_analytics, setAnalytics] = useState(Analytics(_config));

  React.useEffect(() => {
    setAnalytics(Analytics(_config));
  }, [_config]);

  const modified = {
    ..._analytics,
    track: (eventName, payload, options, callback) => {
      const newPayload = Object.assign({}, payload, commonProps);
      return _analytics.track(eventName, newPayload, options, callback);
    },
  };
  const addPlugins = (plugins) => {
    const newPlugins = _plugins;
    for (const plugin of plugins) {
      if (!newPlugins.find((p) => p.name === plugin.name)) {
        newPlugins.push(plugin);
      }
    }
    const config = Object.assign({}, _config, {
      plugins: newPlugins,
    });
    setPlugins(newPlugins);
    setConfig(config);
  };
  const value = Object.freeze({
    analytics: modified,
    commonProps,
    setCommonProps,
    plugins: _plugins,
    addPlugins,
  });
  return (
    <analyticsContext.Provider value={value}>
      {props.children}
    </analyticsContext.Provider>
  );
};

export const useAnalytics = () => useContext(analyticsContext);
