import React, { Suspense } from "react";
import { Spinner } from "./Spinner";

const ReactMarkdown = React.lazy(() => import("react-markdown"));

export const Markdown = ({ children }) => {
  return (
    <React.Fragment>
      <Suspense fallback={<Spinner>Loading Markdown</Spinner>}>
        <ReactMarkdown>{children}</ReactMarkdown>
      </Suspense>
    </React.Fragment>
  );
};
