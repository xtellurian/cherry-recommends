const AuthenticatedIA = [
  {
    name: "Setup",
    to: "/",
  },
  {
    name: "Users",
    items: [
      {
        name: "Add User",
        to: "/tracked-users/create",
      },
      {
        name: "All Users",
        to: "/tracked-users",
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
    name: "Catalogue",
    items: [
      {
        name: "Products",
        to: "/products",
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
        name: "Products",
        to: "/recommenders/product-recommenders",
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
  if (scopes.includes("write:features")) {
    ia.splice(1, 0, {
      name: "Admin",
      items: [
        {
          name: "Features",
          to: "/admin/features",
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
];
