import React, { Suspense } from "react";
import { Spinner } from "../molecules/Spinner";

const RedocStandalone = React.lazy(() =>
  import("redoc").then((module) => ({ default: module.RedocStandalone }))
);

export const ApiDocs = () => {
  return (
    <Suspense fallback={<Spinner />}>
      <RedocStandalone specUrl="/api/docs/v1/spec.json" />
    </Suspense>
  );
};
