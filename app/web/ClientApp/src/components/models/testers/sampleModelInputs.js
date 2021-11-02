const inputWrapper = (payload) => {
  return {
    version: "testing",
    payload,
  };
};

export const getSampleInput = (model) => {
  if (model.modelType === "itemsRecommenderV1") {
    return inputWrapper({
      commonUserId: "1234",
      arguments: {
        one: 1,
        two: "two",
      },
    });
  } else if (model.modelType === "parameterSetRecommenderV1") {
    return inputWrapper({
      arguments: {
        one: "one",
        two: 2,
      },
      parameterBounds: {},
    });
  } else {
    return inputWrapper(null);
  }
};
