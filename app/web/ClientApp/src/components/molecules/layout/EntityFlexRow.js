import React from "react";

const EntityFlexRow = ({ children, direction, justify, align }) => {
  if (!direction) {
    direction = "flex-md-row flex-column";
  }
  if (!justify) {
    justify = "justify-content-between";
  }
  if (!align) {
    align = "align-items-center";
  }
  return (
    <div
      className={`shadow-sm bg-body rounded d-flex p-2 ${direction} ${justify} ${align}`}
    >
      {children}
    </div>
  );
};

export default EntityFlexRow;
