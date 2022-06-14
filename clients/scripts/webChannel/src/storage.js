import { storageKeys } from "./constants";

var currentStorageType = null;

const getStorageType = () => {
  return window[currentStorageType || "localStorage"];
};

// either `localStorage` or `sessiobStorage`
export const setStorageType = (value) => {
  currentStorageType = value;
};

export const getCherryStorageData = ({ key } = {}) => {
  const storage = getStorageType();

  const currentCherrySession = JSON.parse(
    storage.getItem(storageKeys.CHERRY) || "{}"
  );

  // returns the value of the specified key
  if (key) {
    return currentCherrySession?.[key];
  }

  // returns the whole object
  return currentCherrySession;
};

export const setCherryStorageData = ({ key, value } = {}) => {
  const storage = getStorageType();
  const currentCherrySession = getCherryStorageData();

  storage.setItem(
    storageKeys.CHERRY,
    JSON.stringify({ ...currentCherrySession, [key]: value })
  );
};
