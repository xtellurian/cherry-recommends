const defaultHeaders = { "Content-Type": "application/json" };

const path = "api/reactConfig/auth0";
let config = null;
export const fetchAuth0Configuration = async () => {
  if (!config) {
    console.log("fetching auth0 from server...");
    const result = await fetch(path, {
      headers: defaultHeaders,
    });
    config = await result.json()
    console.log(config)
  }
  return config;
};
