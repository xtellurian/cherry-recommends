export const getResults = (name) => {
  switch (name) {
    case "infrequent":
      return resultsData.infrequent;
    case "power":
      return resultsData.power;
    case "new":
    default:
      return resultsData.new;
  }
};

const origin = new Date();
origin.setDate(origin.getDate() - 10 * 7); // 10 weeks ago
const generateDateSinceOrigin = (fortnight) => {
  const d = new Date(origin + fortnight * 12096e5); // 14 days in MS
  d.setDate(origin.getDate() + 14 * fortnight);
  return d.toDateString();
};

export const resultsData = {
  new: [
    {
      order: 1,
      offersMade: 55,
      offerIncome: 9.7,
      startDate: generateDateSinceOrigin(1),
    },
    {
      order: 2,
      offersMade: 61,
      offerIncome: 12.6,
      startDate: generateDateSinceOrigin(2),
    },
    {
      order: 3,
      offersMade: 47,
      offerIncome: 11.8,
      startDate: generateDateSinceOrigin(3),
    },
    {
      order: 4,
      offersMade: 51,
      offerIncome: 14.3,
      startDate: generateDateSinceOrigin(4),
    },
  ],
  infrequent: [
    {
      order: 1,
      offersMade: 87,
      offerIncome: 5.2,
      startDate: generateDateSinceOrigin(1),
    },
    {
      order: 2,
      offersMade: 91,
      offerIncome: 5.3,
      startDate: generateDateSinceOrigin(2),
    },
    {
      order: 3,
      offersMade: 81,
      offerIncome: 6.4,
      startDate: generateDateSinceOrigin(3),
    },
    {
      order: 4,
      offersMade: 79,
      offerIncome: 6.8,
      startDate:generateDateSinceOrigin(4),
    },
  ],
  power: [
    {
      order: 1,
      offersMade: 25,
      offerIncome: 20.2,
      startDate: generateDateSinceOrigin(1),
    },
    {
      order: 2,
      offersMade: 21,
      offerIncome: 22.9,
      startDate: generateDateSinceOrigin(2),
    },
    {
      order: 3,
      offersMade: 24,
      offerIncome: 28.4,
      startDate: generateDateSinceOrigin(3),
    },
    {
      order: 4,
      offersMade: 21,
      offerIncome: 31.4,
      startDate: generateDateSinceOrigin(4),
    },
  ],
};

export const iterations = [
  {
    order: 1,
    offersMade: 164,
    offerIncome: 9.2,
    startDate: "18 Nov, 2020",
  },
  {
    order: 2,
    offersMade: 172,
    offerIncome: 8.9,
    startDate: "11 Nov, 2020",
  },
  {
    order: 3,
    offersMade: 198,
    offerIncome: 9.4,
    startDate: "04 Nov, 2020",
  },
  {
    order: 4,
    offersMade: 121,
    offerIncome: 10.4,
    startDate: "28 Oct, 2020",
  },
  {
    order: 5,
    offersMade: 144,
    offerIncome: 11.5,
    startDate: "21 Oct, 2020",
  },
];
