let storedBaseUrl = "";

export const setBaseUrl = (baseUrl: string): void => {
  storedBaseUrl = baseUrl.substr(-1) === "/" ? baseUrl.slice(0, -1) : baseUrl;
};

export const getBaseUrl = (): string => storedBaseUrl;

export const getUrl = (path: string): string => `${storedBaseUrl}/${path}`;
