import React from "react";
import { Title, BackButton } from "../molecules";
import { NoteBox } from "../molecules/NoteBox";
import { DropdownComponent, DropdownItem } from "../molecules/Dropdown";
import { AzureMLModelRegistration } from "./AzureMLModelRegistration";
import { AzurePersonalizerModelRegistration } from "./AzurePersonalizerModelRegistration";
const modelTypes = [
  { label: "Items Recommender", value: "ItemsRecommenderV1" },
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
      <BackButton className="float-right" to="/models">
        Models
      </BackButton>
      <Title>Register Model</Title>
      <hr />
      <NoteBox className="m-auto w-50" label="Warning">
        This area is for administrators only.
      </NoteBox>

      <div className="row">
        <div className="col">
          <label className="form-label">Model Type</label>
          <DropdownComponent title={modelType.label}>
            {modelTypes.map((c) => (
              <DropdownItem key={c.value}>
                <div onClick={() => setModelType(c)}>{c.label}</div>
              </DropdownItem>
            ))}
          </DropdownComponent>
        </div>

        <div className="col">
          <label className="form-label">Hosting Type</label>
          <DropdownComponent title={hostingType.label}>
            {hostingTypes.map((c) => (
              <DropdownItem key={c.value}>
                <div onClick={() => setHostingType(c)}>{c.label}</div>
              </DropdownItem>
            ))}
          </DropdownComponent>
        </div>
      </div>

      {hostingType.value === "AzureMLContainerInstance" && (
        <AzureMLModelRegistration
          hostingType={hostingType}
          modelType={modelType}
        />
      )}
      {hostingType.value === "AzurePersonalizer" && (
        <AzurePersonalizerModelRegistration
          hostingType={hostingType}
          modelType={modelType}
        />
      )}
    </React.Fragment>
  );
};
