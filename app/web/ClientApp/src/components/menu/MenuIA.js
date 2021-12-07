const AuthenticatedIA = [
  {
    name: "Users",
    items: [
      {
        name: "Tracked Users",
        to: "/tracked-users",
      },
      {
        name: "Add a Customer",
        to: "/tracked-users/create",
      },
      {
        name: "Features",
        to: "/features/",
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
        name: "Items",
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
        name: "Parameters",
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
    name: "Tenant Settings",
    to: "/tenant-settings",
  },
  {
    name: "Deployment Information",
    to: "/settings/info",
  },
];
