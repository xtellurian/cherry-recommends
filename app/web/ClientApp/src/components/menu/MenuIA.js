const AuthenticatedIA = [
  {
    name: "Items",
    icon: "/icons/items.svg",
    to: { pathname: "/", hash: "#items" },
    items: [
      {
        name: "Library",
        to: "/recommendable-items",
      },
      {
        name: "Recommenders",
        to: "/recommenders/items-recommenders",
      },
    ],
  },
  {
    name: "Parameters",
    icon: "/icons/parameters.svg",
    to: { pathname: "/", hash: "#parameters" },
    items: [
      {
        name: "Library",
        to: "/parameters",
      },
      {
        name: "Recommenders",
        to: "/recommenders/parameter-set-recommenders",
      },
    ],
  },
  {
    name: "Customers",
    icon: "/icons/customer.svg",
    to: { pathname: "/customers", hash: "#customers" },
    items: [
      {
        name: "Add a Customer",
        to: "/customers/create",
      },
      {
        name: "Metrics",
        to: "/metrics/",
      },
      {
        name: "Events Overview",
        to: "/dataview",
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
