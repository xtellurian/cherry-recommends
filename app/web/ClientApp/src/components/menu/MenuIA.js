import React from "react";
import { useFlags } from "launchdarkly-react-client-sdk";

const AuthenticatedIA = [
  {
    name: "Promotions",
    icon: "/icons/tag.svg",
    to: { pathname: "/promotions/", hash: "#promotions" },
    items: [
      {
        name: "All Promotions",
        to: "/promotions/",
      },
      {
        name: "Recommenders",
        to: "/recommenders/promotions-recommenders",
      },
    ],
  },
  {
    name: "Customers",
    icon: "/icons/customer.svg",
    to: { pathname: "/customers", hash: "#customers" },
    items: [
      {
        name: "All Customers",
        to: "/customers",
      },
      {
        name: "Add a Customer",
        to: "/customers/create",
      },
      {
        name: "All Businesses",
        to: "/businesses",
      },
      {
        name: "All Segments",
        to: "/segments",
      },
      {
        name: "Events Overview",
        to: "/dataview",
      },
    ],
  },
  {
    name: "Metrics",
    icon: "/icons/graph-up.svg",
    to: { pathname: "/metrics/", hash: "#metrics" },
    items: [
      {
        name: "All Metrics",
        to: "/metrics/",
      },
      {
        name: "Create a Metric",
        to: "/metrics/create",
      },
    ],
  },
  {
    name: "Parameters",
    icon: "/icons/beta.svg",
    to: { pathname: "/parameters/", hash: "#parameters" },
    items: [
      {
        name: "All Parameters",
        to: "/parameters/",
      },
      {
        name: "Recommenders",
        to: "/recommenders/parameter-set-recommenders",
      },
    ],
  },
  {
    name: "Integrations",
    icon: "/icons/integrations.svg",
    to: { pathname: "/settings/integrations", hash: "#integrations" },
    items: [],
  },
];

export const useAuthenticatedIA = (scopes) => {
  const [ia, setIa] = React.useState([]);
  const flags = useFlags();
  React.useEffect(() => {
    setIa(getAuthenticatedIA(scopes, flags));
  }, [scopes, flags]);

  return ia;
};

const getAuthenticatedIA = (scopes, flags) => {
  let ia = AuthenticatedIA;
  if (!flags?.parameterRecommendations) {
    ia = ia.filter((_) => _.name != "Parameters");
  }
  if (!flags?.segments) {
    ia = ia.map((_) => {
      if (_.name === "Customers") {
        const _items = _.items.filter((i) => i.name != "All Segments");
        return { ..._, items: _items };
      }
      return _;
    });
  }
  if (ia.filter((_) => _.name === "Admin").length > 0) {
    // already in there
    return ia;
  }
  if (scopes.includes("write:metrics")) {
    ia.splice(0, 0, {
      name: "Admin",
      icon: "/icons/metric.svg",
      to: { pathname: "/", hash: "#admin" },
      items: [
        {
          name: "Models",
          to: "/models",
        },
      ],
    });
  }
  return ia;
};

export const settingsItems = [
  {
    name: "API Docs",
    to: "/docs/api",
  },
  {
    name: "API Keys",
    to: "/settings/api-keys",
  },
  {
    name: "Tenant Settings",
    to: "/tenant-settings",
  },
  {
    name: "Deployment Information",
    to: "/settings/info",
  },
];
