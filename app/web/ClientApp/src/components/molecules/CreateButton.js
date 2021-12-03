import React from "react";
import Tippy from "@tippyjs/react";
import { Link } from "react-router-dom";

export const CreateButtonClassic = ({ to, children, className }) => {
  return (
    <div className={className}>
      <Link to={to}>
        <button className="btn btn-primary">{children}</button>
      </Link>
    </div>
  );
};

export const CreateButton = ({
  children,
  tooltip,
  className,
  buttonClassName,
  onClick,
}) => {
  return (
    <div className={className || "float-right"}>
      <>
        <div className="d-block d-sm-none d-md-none">
          <Tippy content={tooltip} placement="left">
            <button
              onClick={onClick}
              className={buttonClassName || "btn btn-outline-primary"}
            >
              {children}
            </button>
          </Tippy>
        </div>
        <div className="d-block d-sm-block d-md-block">
          <Tippy content={tooltip} placement="left">
            <button
              onClick={onClick}
              className={buttonClassName || "btn btn-outline-primary"}
            >
              +
            </button>
          </Tippy>
        </div>
      </>
    </div>
  );
};
