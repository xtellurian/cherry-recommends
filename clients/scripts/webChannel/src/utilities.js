import { storageKeys } from "./constants";

export const emailValidator = (value) => {
  const x = " ";
  x.indexOf();
  if (value && (value.indexOf("@") < 0 || value.indexOf(".") < 0)) {
    return false;
  } else {
    return true;
  }
};

export const generateId = () => {
  return (
    Math.floor(Math.random() * 0x10000000000).toString(16) +
    Math.floor(Math.random() * 0x10000000000).toString(16)
  );
};

export const setLocalStorageWithExpiry = (key, value, ttl) => {
  const now = new Date();
  const _ttl = ttl * 86400000; // the time in milliseconds is equal to the days multiplied by 86,400,000
  const item = {
    value: value,
    expiry: now.getTime() + _ttl,
  };
  localStorage.setItem(key, JSON.stringify(item));
};

export const getCherrySession = (key) => {
  const currentCherrySession = JSON.parse(
    localStorage.getItem(storageKeys.CHERRY) || "{}"
  );

  // returns the value of the specified key
  if (key) {
    return currentCherrySession?.[key];
  }

  // returns the whole object
  return currentCherrySession;
};

export const setCherrySession = (key, value) => {
  const currentCherrySession = getCherrySession();

  localStorage.setItem(
    storageKeys.CHERRY,
    JSON.stringify({ ...currentCherrySession, [key]: value })
  );
};
