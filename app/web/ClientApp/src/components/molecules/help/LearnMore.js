import Tippy from "@tippyjs/react";
import React from "react";
import { InfoCircle } from "react-bootstrap-icons";
import { BigPopup } from "../popups/BigPopup";

const LearnMoreTooltip = ({ children }) => {
  return <div className="bg-light border rounded p-3">{children}</div>;
};
export const LearnMore = ({ children, className, tooltip }) => {
  const [isOpen, setIsOpen] = React.useState(false);
  return (
    <React.Fragment>
      <Tippy content={<LearnMoreTooltip>{tooltip}</LearnMoreTooltip>}>
        <InfoCircle
          cursor="pointer"
          size="20px"
          className={className || ""}
          onClick={() => setIsOpen(true)}
        />
      </Tippy>
      <BigPopup isOpen={isOpen} setIsOpen={setIsOpen} header="More Information">
        <div className="m-3">{children}</div>
      </BigPopup>
    </React.Fragment>
  );
};
