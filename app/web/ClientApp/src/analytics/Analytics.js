import React from "react";
import { useLocation } from "react-router-dom";
import { useAnalytics } from "./analyticsHooks";
import { SegmentPlugin } from "./analyticsPlugins";
import { useDeploymentConfiguration } from "../api-hooks/deploymentApi";
import { fetchConfigurationAsync } from "../api/reactConfigApi";
import { useAuth } from "../utility/useAuth";

const Analytics = ({ children }) => {
  const { analytics, plugins, addPlugins, commonProps, setCommonProps } =
    useAnalytics();
  const location = useLocation();
  const { user, isAuthenticated } = useAuth();
  const deploymentInfo = useDeploymentConfiguration();
  const stack = deploymentInfo.stack;
  React.useEffect(() => {
    fetchConfigurationAsync().then((config) => {
      const plugins = [];
      plugins.push(SegmentPlugin(config.segment));
      addPlugins(plugins);
    });
  }, []);
  React.useEffect(() => {
    if (plugins.length) {
      // track page views
      analytics.page(location.pathname);
    }
  }, [analytics, plugins, location]);
  React.useEffect(() => {
    if (user && isAuthenticated) {
      if (!commonProps && stack) {
        setCommonProps({
          ...commonProps,
          stack: deploymentInfo.stack,
        });
      }
      if (plugins.length && commonProps) {
        // identify logged-in user
        analytics.identify(user.sub);
      }
    }
  }, [analytics, plugins, user, isAuthenticated, commonProps, stack]);
  return <>{children}</>;
};

export default Analytics;
