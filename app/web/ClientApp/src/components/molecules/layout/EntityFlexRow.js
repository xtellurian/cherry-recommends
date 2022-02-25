import React from "react";

const EntityFlexRow = ({
  children,
  className,
  direction,
  justify,
  align,
  onClick,
  style,
}) => {
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
      onClick={onClick}
      style={style}
      className={`${
        className || ""
      } shadow-sm bg-body rounded d-flex p-2 ${direction} ${justify} ${align}`}
    >
      {children}
    </div>
  );
};

export default EntityFlexRow;
