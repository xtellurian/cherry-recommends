import React from "react";
import { useFlags } from "launchdarkly-react-client-sdk";

export const hash = {
  gettingStarted: "#gettingStarted",
  admin: "#admin",
  promotions: "#promotions",
  customers: "#customers",
  metrics: "#metrics",
  parameters: "#parameters",
  integration: "#integrations",
  promotionsRecommenders: "#promotions_recommenders",
};

export const metricsHash = {
  create: `${hash.metrics}_create`,
};

const AuthenticatedIA = [
  {
    name: "Get Started",
    icon: "/icons/rocket.svg",
    to: { pathname: "/getting-started", search: "", hash: hash.gettingStarted },
    activeIcon: false,
    items: [],
  },
  {
    name: "Promotions",
    icon: "/icons/tag.svg",
    to: { pathname: "/promotions", search: "", hash: hash.promotions },
    items: [
      {
        name: "Promotions",
        to: {
          pathname: "/promotions/",
          search: "",
          hash: hash.promotions,
        },
      },
      {
        name: "Recommenders",
        to: {
          pathname: "/recommenders/promotions-recommenders",
          search: "",
          hash: `${hash.promotions}_recommenders`,
        },
      },
    ],
  },
  {
    name: "Customers",
    icon: "/icons/customer.svg",
    to: { pathname: "/customers", search: "", hash: hash.customers },
    items: [
      {
        name: "Customers",
        to: {
          pathname: "/customers",
          search: "",
          hash: hash.customers,
        },
      },
      {
        name: "Businesses",
        to: {
          pathname: "/businesses",
          search: "",
          hash: `${hash.customers}_businesses`,
        },
      },
      {
        name: "Segments",
        to: {
          pathname: "/segments",
          search: "",
          hash: `${hash.customers}_segments`,
        },
      },
      {
        name: "Events Overview",
        to: {
          pathname: "/dataview",
          search: "",
          hash: `${hash.customers}_dataview`,
        },
      },
    ],
  },
  {
    name: "Metrics",
    icon: "/icons/graph-up.svg",
    to: { pathname: "/metrics", search: "", hash: hash.metrics },
    items: [
      {
        name: "Metrics",
        to: {
          pathname: "/metrics/",
          search: "",
          hash: hash.metrics,
        },
      },
    ],
  },
  {
    name: "Parameters",
    icon: "/icons/beta.svg",
    to: { pathname: "/parameters", search: "", hash: hash.parameters },
    items: [
      {
        name: "Parameters",
        to: {
          pathname: "/parameters/",
          search: "",
          hash: hash.parameters,
        },
      },
      {
        name: "Recommenders",
        to: {
          pathname: "/recommenders/parameter-set-recommenders",
          search: "",
          hash: `${hash.parameters}_recommenders`,
        },
      },
    ],
  },
  {
    name: "Integrations",
    icon: "/icons/integrations.svg",
    to: {
      pathname: "/settings/integrations",
      search: "",
      hash: hash.integration,
    },
    items: [
      {
        name: "Connections",
        to: {
          pathname: "/settings/integrations",
          search: "",
          hash: `${hash.integration}`,
        },
      },
      {
        name: "Data Sources",
        to: {
          pathname: "/settings/integrations/data-sources",
          search: "",
          hash: `${hash.integration}_datasources`,
        },
      },
      {
        name: "Channels",
        to: {
          pathname: "/channels",
          search: "",
          hash: `${hash.integration}_channels`,
        },
      },
    ],
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
    ia = ia.filter((_) => _.name !== "Parameters");
  }
  if (!flags?.segments) {
    ia = ia.map((_) => {
      if (_.name === "Customers") {
        const _items = _.items.filter((i) => i.name !== "Segments");
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
    ia.splice(1, 0, {
      name: "Admin",
      icon: "/icons/metric.svg",
      to: { pathname: "/", hash: hash.admin },
      items: [
        {
          name: "Models",
          to: {
            pathname: "/models",
            hash: `${hash.admin}_models`,
          },
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
