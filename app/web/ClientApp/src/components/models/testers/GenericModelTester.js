import React from "react";

import {
  AsyncButton,
  ErrorCard,
  ExpandableCard,
  Subtitle,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
} from "../../molecules";
import { invokeGenericModelAsync } from "../../../api/modelsApi";
import { JsonView } from "../../molecules/JsonView";
import { useAccessToken } from "../../../api-hooks/token";
import { getSampleInput } from "./sampleModelInputs";

function isJson(str) {
  try {
    JSON.parse(str);
  } catch (e) {
    return false;
  }
  return true;
}
export const GenericModelTester = ({ model }) => {
  const token = useAccessToken();
  const [input, setInput] = React.useState(
    JSON.stringify(getSampleInput(model), null, 2)
  );

  // reload when model loads
  React.useEffect(() => {
    setInput(JSON.stringify(getSampleInput(model), null, 2));
  }, [model]);

  const handleFormat = () => {
    if (isInputJson) {
      setInput(JSON.stringify(JSON.parse(input), null, 2));
    }
  };

  const [isInputJson, setIsInputJson] = React.useState(isJson(input));

  React.useEffect(() => {
    setIsInputJson(isJson(input));
  }, [input]);

  const [loading, setLoading] = React.useState(false);
  const [modelOutput, setModelOutput] = React.useState();
  const [error, setError] = React.useState();
  const handleInvoke = () => {
    setLoading(true);
    if (isInputJson) {
      invokeGenericModelAsync({
        token,
        id: model.id,
        input: JSON.parse(input),
      })
        .then(setModelOutput)
        .catch(setError)
        .finally(() => setLoading(false));
    } else {
      setError({ title: "Input must be JSON Serialisable" });
    }
  };
  return (
    <React.Fragment>
      <MoveUpHierarchyPrimaryButton to="/admin/models">
        Back to Models
      </MoveUpHierarchyPrimaryButton>
      <PageHeading title={model.name || "..."} subtitle="Model Tester" />
      <ExpandableCard label="Model Information">
        <JsonView data={model} />
      </ExpandableCard>
      {error && <ErrorCard error={error} />}
      <div className="row mt-2">
        <div className="col">
          <Subtitle>Input</Subtitle>
          <button
            className="btn btn-secondary float-right"
            onClick={handleFormat}
          >
            Format
          </button>
          <div
            className={
              isInputJson
                ? "p-2 form-group bg-success"
                : "p-2 form-group bg-danger"
            }
          >
            <label htmlFor="jsonInput">Input JSON</label>
            <textarea
              value={input}
              onChange={(a) => setInput(a.target.value)}
              className={
                isInputJson
                  ? "form-control border border-success"
                  : "form-control border border-danger"
              }
              id="jsonInput"
              rows="12"
            ></textarea>
          </div>
        </div>
        <div className="col">
          <Subtitle>Output</Subtitle>
          {isJson(modelOutput) && <JsonView data={JSON.parse(modelOutput)} />}
        </div>
      </div>
      <AsyncButton
        className="btn btn-primary btn-block"
        loading={loading || model.loading}
        disabled={!isInputJson}
        onClick={handleInvoke}
      >
        Invoke
      </AsyncButton>
    </React.Fragment>
  );
};
