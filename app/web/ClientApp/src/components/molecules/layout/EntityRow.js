import React from "react";
import Tippy from "@tippyjs/react";

const EntityRowToolip = ({ children }) => {
  return <div className="bg-light border rounded p-3">{children}</div>;
};

export const EntityRow = ({ children, size, tooltip }) => {
  if (size === "sm") {
    return (
      <Tippy duration={[null, 0]} content={<EntityRowToolip>{`${tooltip}`}</EntityRowToolip>}>
        <div className="border-bottom p-1 text-truncate">{children}</div>
      </Tippy>
    );
  } else {
    return (
      <div className="row mt-1 mb-2 p-3 shadow-sm bg-body rounded">
        {children}
      </div>
    );
  }
};
