import React from "react";
import { useHosting, managementSubdomain } from "../../api-hooks/tenantsApi";

export const TenantNotFound = ({ error }) => {
  const hosting = useHosting();
  console.error(error);
  const redirectToManagement = () => {
    console.info("redirecting...");
    window.location = `https://${managementSubdomain}.${hosting.canonicalRootDomain}?autoSignIn=true`;
  };
  // if (!hosting.loading && hosting.canonicalRootDomain) {
  //   setTimeout(redirectToManagement, 5000);
  // }

  return (
    <div className="text-center">
      <h3>Tenant Not Found</h3>
      <hr />
      <p>{error.title}</p>

      <button
        className="btn btn-outline-primary"
        onClick={redirectToManagement}
      >
        View All
      </button>
    </div>
  );
};
