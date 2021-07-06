import React from "react";
import Prism from "prismjs";

import "./css/prism.css";

export const CodeView = ({ language, text }) => {
  if (!language) {
    language = "javascript";
  }
  React.useEffect(() => {
    Prism.highlightAll();
  }, []);
  return (
    <div>
      <pre>
        <code className={`language-${language}`}>{`${text}`}</code>
      </pre>
    </div>
  );
};
