import React from "react";
import { useHosting, managementSubdomain } from "../../api-hooks/tenantsApi";
import { Spinner } from "../molecules";

export const TenantNotFound = ({ error }) => {
  const hosting = useHosting();
  if (!hosting.loading && hosting.canonicalRootDomain) {
    const onTimedown = () => {
      console.log("redirecting...");
      window.location = `https://${managementSubdomain}.${hosting.canonicalRootDomain}`;
    };
    setTimeout(onTimedown, 5000);
  }

  return (
    <div className="text-center">
      <h3>Tenant Not Found</h3>
      <hr />
      <p>{error.title}</p>

      <Spinner>Page will redirect</Spinner>
    </div>
  );
};
