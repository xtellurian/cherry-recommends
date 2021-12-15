let storedBaseUrl = "";
export const setBaseUrl = (baseUrl) => {
    storedBaseUrl = baseUrl.substr(-1) === "/" ? baseUrl.slice(0, -1) : baseUrl;
};
export const getUrl = (path) => `${storedBaseUrl}/${path}`;
