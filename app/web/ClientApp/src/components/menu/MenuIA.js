const AuthenticatedIA = [
  {
    name: "Users",
    items: [
      {
        name: "Tracked Users",
        to: "/tracked-users",
      },
      {
        name: "Track a new user",
        to: "/tracked-users/create",
      },
      {
        name: "Events Overview",
        to: "/dataview",
      },
    ],
  },
  {
    name: "Library",
    items: [
      {
        name: "Recommendable Items",
        to: "/recommendable-items",
      },
      {
        name: "Parameters",
        to: "/parameters",
      },
    ],
  },
  {
    name: "Recommenders",
    items: [
      {
        name: "Items",
        to: "/recommenders/items-recommenders",
      },
      {
        name: "Parameter Sets",
        to: "/recommenders/parameter-set-recommenders",
      },
    ],
  },
];

export const getAuthenticatedIA = (scopes) => {
  let ia = AuthenticatedIA;
  if (ia.filter((_) => _.name === "Admin").length > 0) {
    // already in there
    return ia;
  }
  if (scopes.includes("write:features")) {
    ia.splice(1, 0, {
      name: "Admin",
      items: [
        {
          name: "Features",
          to: "/admin/features",
        },
        {
          name: "Feature Generators",
          to: "/admin/feature-generators",
        },
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
    name: "Deployment Information",
    to: "/settings/info",
  },
  {
    name: "API Docs",
    to: "/docs/api",
  },
  {
    name: "API Keys",
    to: "/settings/api-keys",
  },
  {
    name: "Integrations",
    to: "/settings/integrations",
  },
  {
    name: "Reward Settings",
    to: "/settings/rewards",
  },
];
