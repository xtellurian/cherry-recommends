import React from "react";
import { RedocStandalone } from "redoc";

export const ApiDocs = () => {
  return <RedocStandalone specUrl="/api/docs/v1/spec.json" />;
};
