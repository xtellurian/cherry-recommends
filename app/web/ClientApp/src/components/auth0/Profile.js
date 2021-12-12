import React from "react";
import { useAccessToken } from "../../api-hooks/token";
import { useMetadataDirectly } from "../../api-hooks/profileApi";
import { setMetadataAsync } from "../../api/profileApi";
import { ExpandableCard } from "../molecules/ExpandableCard";
import { ErrorCard } from "../molecules/ErrorCard";
import { JsonView } from "../molecules/JsonView";
import { useAuth } from "../../utility/useAuth";

export const Profile = () => {
  const { user, isAuthenticated } = useAuth();
  const token = useAccessToken();
  const [trigger, setTrigger] = React.useState({});
  const [error, setError] = React.useState();

  const metadata = useMetadataDirectly({ trigger });

  return (
    isAuthenticated && (
      <div>
        <img src={user.picture} alt={user.name} />
        <h2>{user.name}</h2>
        <p>{user.email}</p>
        <h3>User Metadata</h3>
        <ExpandableCard label="User Metadata">
          <div>
            <ErrorCard error={error} />
            <button
              className="btn btn-danger float-right"
              onClick={() =>
                setMetadataAsync({
                  token,
                  metadata: {},
                })
                  .then(setTrigger)
                  .catch(setError)
              }
            >
              Reset Metadata
            </button>
          </div>
          <JsonView data={metadata} />
        </ExpandableCard>
        <hr />
        <ExpandableCard label="Access Token">
          <div>{token}</div>
        </ExpandableCard>
      </div>
    )
  );
};
