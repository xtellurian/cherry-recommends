import React from "react";

import "./funloader.css";
const textStyle = {
  position: "fixed",
  bottom: "48px",
};

export const FunloaderContainer = () => {
  return (
    <div className="loader-container">
      <div className="text-center text-white w-100" style={textStyle}>
        <h5>Firing up neurons</h5>
      </div>
      <Funloader color="white" />
    </div>
  );
};
export const Funloader = ({ color }) => {
  if (color === "blue") {
    return (
      <div className="loader m-auto">
        <div className="loader__bar loader__blue"></div>
        <div className="loader__bar loader__blue"></div>
        <div className="loader__bar loader__blue"></div>
        <div className="loader__bar loader__blue"></div>
        <div className="loader__bar loader__blue"></div>
        <div className="loader__ball loader__blue"></div>
      </div>
    );
  } else {
    return (
      <div className="loader m-auto">
        <div className="loader__bar loader__white"></div>
        <div className="loader__bar loader__white"></div>
        <div className="loader__bar loader__white"></div>
        <div className="loader__bar loader__white"></div>
        <div className="loader__bar loader__white"></div>
        <div className="loader__ball loader__white"></div>
      </div>
    );
  }
};
