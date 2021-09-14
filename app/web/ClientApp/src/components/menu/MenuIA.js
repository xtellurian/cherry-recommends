const AuthenticatedIA = [
  {
    name: "Setup",
    to: "/",
  },
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
  //   {
  //     name: "Segmentation",
  //     items: [
  //       {
  //         name: "Segments",
  //         to: "/segments",
  //       },
  //       {
  //         name: "Models",
  //         to: "/models",
  //       },
  //     ],
  //   },
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
      {
        name: "Products",
        to: "/products",
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
      {
        name: "Products",
        to: "/recommenders/product-recommenders",
      },
    ],
  },
];

export const getAuthenticatedIA = (scopes) => {
  let ia = AuthenticatedIA;
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
        {
          name: "Touchpoints",
          to: "/touchpoints",
        },
        {
          name: "Offers",
          to: "/offers",
        },
        {
          name: "Experiments",
          to: "/experiments",
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
