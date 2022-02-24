const AuthenticatedIA = [
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
    name: "Promotions",
    icon: "/icons/items.svg",
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

export const getAuthenticatedIA = (scopes) => {
  let ia = AuthenticatedIA;
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
