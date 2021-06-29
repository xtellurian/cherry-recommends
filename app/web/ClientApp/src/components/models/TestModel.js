import React from "react";
import { useParams } from "react-router-dom";
import { useModelRegistration } from "../../api-hooks/modelRegistrationsApi";
import { TestAzureMLClassifier } from "./testers/TestAzureMLClassifier";
import { GenericModelTester } from "./testers/GenericModelTester";

import "swagger-ui-react/swagger-ui.css";

export const TestModel = () => {
  const { id } = useParams();

  const model = useModelRegistration({ id });
  if (model.modelType === "singleClassClassifier") {
    return <TestAzureMLClassifier model={model} />;
  } else {
    return <GenericModelTester model={model} />;
  }
};
