import React from "react";
import { useFlags } from "launchdarkly-react-client-sdk";

import { routes } from "../../utility/routes";

const createURL = (props) => {
  return { search: "", hash: "", ...props };
};

const AuthenticatedIA = [
  {
    id: routes.gettingStarted,
    name: "Get Started",
    icon: "/icons/rocket.svg",
    to: createURL({ pathname: "/getting-started" }),
    activeIcon: false,
    items: [],
  },
  {
    id: routes.promotions,
    name: "Promotions",
    icon: "/icons/tag.svg",
    items: [
      {
        id: routes.promotions,
        name: "Promotions",
        to: createURL({
          pathname: "/promotions/promotions",
        }),
      },
      {
        id: "promotions-recommenders",
        name: "Recommenders",
        to: createURL({
          pathname: "/recommenders/promotions-recommenders",
        }),
      },
    ],
  },
  {
    id: routes.customers,
    name: "Customers",
    icon: "/icons/customer.svg",
    items: [
      {
        id: routes.customers,
        name: "Customers",
        to: createURL({
          pathname: "/customers/customers",
        }),
      },
      {
        id: routes.businesses,
        name: "Businesses",
        to: createURL({
          pathname: "/customers/businesses",
        }),
      },
      {
        id: routes.segments,
        name: "Segments",
        to: createURL({
          pathname: "/customers/segments",
        }),
      },
      {
        id: routes.dataview,
        name: "Events Overview",
        to: createURL({
          pathname: "/customers/dataview",
        }),
      },
    ],
  },
  {
    id: routes.metrics,
    name: "Metrics",
    icon: "/icons/graph-up.svg",
    items: [
      {
        id: routes.metrics,
        name: "Metrics",
        to: createURL({
          pathname: "/metrics/metrics",
        }),
      },
    ],
  },
  {
    id: routes.parameters,
    name: "Parameters",
    icon: "/icons/beta.svg",
    items: [
      {
        id: routes.parameters,
        name: "Parameters",
        to: createURL({
          pathname: "/parameters/parameters",
        }),
      },
      {
        id: "parameter-set-recommenders",
        name: "Recommenders",
        to: createURL({
          pathname: "/recommenders/parameter-set-recommenders",
        }),
      },
    ],
  },
  {
    id: routes.integrations,
    name: "Integrations",
    icon: "/icons/integrations.svg",
    items: [
      {
        id: "connections",
        name: "Connections",
        to: createURL({
          pathname: "/settings/integrations",
          hash: "connections",
        }),
      },
      {
        id: "data-sources",
        name: "Data Sources",
        to: createURL({
          pathname: "/settings/integrations/data-sources",
          hash: "datasources",
        }),
      },
      {
        id: routes.channels,
        name: "Channels",
        to: createURL({
          pathname: "/integrations/channels",
        }),
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
      id: routes.admin,
      name: "Admin",
      icon: "/icons/metric.svg",
      items: [
        {
          id: routes.models,
          name: "Models",
          to: createURL({
            pathname: "/admin/models",
          }),
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
