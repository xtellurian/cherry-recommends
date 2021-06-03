import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { useAuth0AccessToken } from "../../api-hooks/token";
import { useAuth0Config } from "../../api-hooks/reactConfigApi";

export const Profile = () => {
  const config = useAuth0Config();
  const { user, isAuthenticated } = useAuth0();
  const [userMetadata, setUserMetadata] = React.useState();
  const token = useAuth0AccessToken();
  React.useEffect(() => {
    if (token && user && config && config.domain) {
      const userDetailsByIdUrl = `https://${config.domain}/api/v2/users/${user.sub}`;
      fetch(userDetailsByIdUrl, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
        .then((m) => m.json())
        .then((res) => setUserMetadata(res))
        .catch(() => alert("broke"));
    }
  }, [token, user, config]);

  return (
    isAuthenticated && (
      <div>
        <img src={user.picture} alt={user.name} />
        <h2>{user.name}</h2>
        <p>{user.email}</p>
        <h3>User Metadata</h3>
        {userMetadata ? (
          <pre>{JSON.stringify(userMetadata, null, 2)}</pre>
        ) : (
          "No user metadata defined"
        )}
      </div>
    )
  );
};
