import React from "react";
import Tippy from "@tippyjs/react";

import { Typography } from "../../components/molecules";

import "./FieldLabel.css";

const HintTippy = ({ children, value }) => {
  return (
    <Tippy
      placement="top-start"
      content={
        value ? (
          <div className="bg-white text-center border rounded px-2 py-1 field-label-popup shadow-sm">
            {value}
          </div>
        ) : null
      }
    >
      {children}
    </Tippy>
  );
};

export const FieldLabel = ({
  children,
  type,
  label,
  hint,
  required,
  optional,
  className,
  inline = true,
  labelPosition = "center",
}) => {
  const types = {
    checkbox: "mb-0",
  };

  const labelPositions = {
    center: "align-items-md-center",
    top: "align-items-md-top",
  };

  return (
    <div
      className={`field-label-wrapper ${inline ? "d-md-flex" : ""} ${
        types[type] || "mb-4"
      } ${labelPositions[labelPosition]} ${className}`}
    >
      {label ? (
        <HintTippy value={hint}>
          <div className="field-label mb-1 mb-md-0">
            <Typography
              variant="label"
              className={`semi-bold mr-0 ${
                inline ? "text-md-right mr-md-4" : "mb-1"
              }`}
            >
              {label}{" "}
              {required ? <span className="text-danger">{`*`}</span> : null}
              {optional ? (
                <span
                  className="font-italic"
                  style={{ fontWeight: 400 }}
                >{`(optional)`}</span>
              ) : null}
            </Typography>
          </div>
        </HintTippy>
      ) : null}

      {children}
    </div>
  );
};
