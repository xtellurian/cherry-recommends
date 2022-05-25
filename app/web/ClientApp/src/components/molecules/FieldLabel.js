import React from "react";
import Tippy from "@tippyjs/react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleInfo } from "@fortawesome/free-solid-svg-icons";

import { Typography } from "../../components/molecules";

import "./FieldLabel.css";

export const HintTippy = ({ children, value }) => {
  return (
    <Tippy
      duration={[null, 0]}
      placement="top-start"
      content={
        value ? (
          <Typography
            variant="label"
            className="text-white text-center border rounded px-3 py-2 field-label-popup shadow"
            style={{ backgroundColor: "#212121" }}
          >
            {value}
          </Typography>
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
          <div
            className={`field-label mb-1 mb-md-0 ${
              hint ? "cursor-pointer" : ""
            }`}
          >
            <Typography
              variant="label"
              className={`semi-bold mr-0 ${
                inline ? "text-md-right mr-md-4" : "mb-1"
              }`}
            >
              {label}{" "}
              {required ? (
                <span className="text-danger mr-1">{`*`}</span>
              ) : null}
              {optional ? (
                <span
                  className="font-italic mr-1"
                  style={{ fontWeight: 400 }}
                >{`(optional)`}</span>
              ) : null}
              {hint ? (
                <FontAwesomeIcon
                  icon={faCircleInfo}
                  className="text-secondary ml-1 mt-1"
                  fontSize={12}
                />
              ) : null}
            </Typography>
          </div>
        </HintTippy>
      ) : null}

      {children}
    </div>
  );
};
