import React from "react";

export const NoteBox = ({
  children,
  label,
  className,
  cardBodyClassName,
  cardTitleClassName,
}) => {
  return (
    <div className={className || ""}>
      <div className="card text-center">
        <div className={`card-body ${cardBodyClassName || ""}`}>
          <h5 className={`card-title ${cardTitleClassName || ""}`}>{label}</h5>
          <p className="card-text">{children}</p>
        </div>
      </div>
    </div>
  );
};
