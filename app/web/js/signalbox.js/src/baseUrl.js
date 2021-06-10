let storedBaseUrl = "";

export const setBaseUrl = (baseUrl) => {
  storedBaseUrl = baseUrl.trim("/");
};

export const getUrl = (path) => `${storedBaseUrl}/${path}`;
