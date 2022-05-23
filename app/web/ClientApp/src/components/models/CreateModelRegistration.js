import React from "react";

import {
  MoveUpHierarchyPrimaryButton,
  PageHeading,
  Selector,
} from "../molecules";
import { NoteBox } from "../molecules/NoteBox";
import { AzureMLModelRegistration } from "./AzureMLModelRegistration";
import { AzurePersonalizerModelRegistration } from "./AzurePersonalizerModelRegistration";
import CreatePageLayout from "../molecules/layout/CreatePageLayout";

const modelTypes = [
  { label: "Promotions Recommender", value: "ItemsRecommenderV1" },
  { label: "Parameter Set Recommender", value: "ParameterSetRecommenderV1" },
  { label: "Classifier", value: "SingleClassClassifier" },
];

const hostingTypes = [
  { label: "Azure ML Container", value: "AzureMLContainerInstance" },
  { label: "Azure Personalizer", value: "AzurePersonalizer" },
];

export const CreateModelRegistration = () => {
  const [hostingType, setHostingType] = React.useState(hostingTypes[0]);
  const [modelType, setModelType] = React.useState(modelTypes[0]);

  return (
    <React.Fragment>
      <CreatePageLayout
        backButton={
          <MoveUpHierarchyPrimaryButton to="/admin/models">
            Back to Models
          </MoveUpHierarchyPrimaryButton>
        }
        header={<PageHeading title="Register a Model" />}
      >
        <NoteBox className="mx-auto mb-4 w-50" label="Warning">
          This area is for administrators only.
        </NoteBox>

        <Selector
          label="Model Type"
          value={modelType}
          options={modelTypes}
          onChange={(value) => setModelType(value)}
        />

        <Selector
          label="Hosting Type"
          value={hostingType}
          options={hostingTypes}
          onChange={(value) => setHostingType(value)}
        />

        {hostingType.value === "AzureMLContainerInstance" ? (
          <AzureMLModelRegistration
            hostingType={hostingType}
            modelType={modelType}
          />
        ) : null}
        {hostingType.value === "AzurePersonalizer" ? (
          <AzurePersonalizerModelRegistration
            hostingType={hostingType}
            modelType={modelType}
          />
        ) : null}
      </CreatePageLayout>
    </React.Fragment>
  );
};
