export const internalId = (useInternalId) => {
  if (useInternalId === null || useInternalId === undefined) {
    return "";
  } else if (useInternalId === true) {
    return "useInternalId=true";
  } else if (useInternalId === false) {
    return "useInternalId=false";
  } else {
    return "";
  }
};
