import React from "react";
import { Link } from "react-router-dom";

export const CreateButton = ({ to, children, className }) => {
  return (
    <div className={className}>
      <Link to={to}>
        <button className="btn btn-primary">{children}</button>
      </Link>
    </div>
  );
};
