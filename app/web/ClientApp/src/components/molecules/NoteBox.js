import React from "react";

export const NoteBox = ({ children, label, className }) => {
  return (
    <div className={className || ""}>
      <div className="card text-center">
        <div className="card-body">
          <h5 className="card-title">{label}</h5>
          <p className="card-text">{children}</p>
        </div>
      </div>
    </div>
  );
};
