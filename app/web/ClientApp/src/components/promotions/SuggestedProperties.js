import React, { useMemo } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCirclePlus } from "@fortawesome/free-solid-svg-icons";

import { Typography } from "../molecules";
import { HintTippy } from "../molecules/FieldLabel";

import "./SuggestedProperties.css";

export const suggestedPromotionProperties = [
  {
    key: "coupon_code",
    description: "Redeemable in your billing platform",
  },
  {
    key: "email_subject",
    description: "Specific text for the email subject",
  },
  {
    key: "email_preview",
    description: "Specific text for the email preview",
  },
  {
    key: "email_body_text_1",
    description: "Specific text for the email body",
  },
  {
    key: "minimum_spend_threshold",
    description: "Minimum amount in checkout before promotion applies",
  },
];

export const SuggestedProperties = ({
  suggestions,
  currentProperties,
  addProperty,
}) => {
  const currentPropertiesKeys = useMemo(
    () => currentProperties.reduce((acc, curr) => [...acc, curr.key], []),
    [currentProperties]
  );

  const currentSuggestions = useMemo(
    () => suggestions.filter((el) => !currentPropertiesKeys.includes(el.key)),
    [currentPropertiesKeys, suggestions]
  );

  if (currentSuggestions.length === 0) {
    return null;
  }

  return (
    <div className="d-flex flex-wrap mb-4">
      <Typography variant="label" component="span" className="py-1 mr-3 mb-2">
        Suggestions:
      </Typography>
      {currentSuggestions.map((el) => (
        <React.Fragment key={el.key}>
          <HintTippy value={el.description}>
            <span
              className="d-flex cursor-pointer disable-select rounded-pill text-white px-2 py-1 mr-2 mb-2 suggestion-pill align-items-center"
              style={{
                backgroundColor: "#616161", // grey-600
              }}
              onClick={(e) => addProperty(e, { ...el, isSuggested: true })}
            >
              <Typography variant="label" className="text-white ml-1">
                {el.key}
              </Typography>
              <FontAwesomeIcon icon={faCirclePlus} className="ml-2" />
            </span>
          </HintTippy>
        </React.Fragment>
      ))}
    </div>
  );
};
