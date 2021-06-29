export const AuthenticatedIA = [
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
        name: "Touchpoints",
        to: "/touchpoints",
      },
      {
        name: "Parameters",
        to: "/parameters",
      },
      {
        name: "Offers",
        to: "/offers",
      },
    ],
  },
  {
    name: "Recommenders",
    items: [
      {
        name: "Parameter Sets",
        to: "/recommenders/parameter-set-recommenders",
      },
      {
        name: "Experiments",
        to: "/experiments",
      },
    ],
  },
];

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
    name: "Models",
    to: "/models",
  },
];
