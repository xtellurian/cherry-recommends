import React from "react";

// this is stored in the dev docs host
const imgSrc =
  "https://docshost1920c670.blob.core.windows.net/content/cherry-temp/cherry-cupcake-temp.png";
export const CherrySecret = () => {
  return (
    <div
      className="text-center p-5"
      style={{
        backgroundColor: "#250938",
        height: "100vh",
        width: "100vw",
      }}
    >
      <div
        style={{
          color: "white",
          marginTop: "10vh",
        }}
      >
        <div>
          <img src={imgSrc} />
        </div>
      </div>
    </div>
  );
};
