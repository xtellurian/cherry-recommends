import React from "react";
import { Link } from "react-router-dom";
import { Navigation } from "../molecules";

export const TenantNotFound = ({ error }) => {
  console.error(error);

  return (
    <div className="text-center">
      <h3>Tenant Not Found</h3>
      <hr />
      <p>{error.title}</p>
      <Navigation to="/" escapeTenant={true}>
        <button className="btn btn-outline-primary">Home</button>
      </Navigation>
    </div>
  );
};
