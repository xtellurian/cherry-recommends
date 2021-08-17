import React from "react";
import { useAccessToken } from "../../../api-hooks/token";
import { useParameters } from "../../../api-hooks/parametersApi";
import { createParameterSetRecommenderAsync } from "../../../api/parameterSetRecommendersApi";
import {
  BackButton,
  Title,
  Subtitle,
  Selector,
  ExpandableCard,
  ErrorCard,
} from "../../molecules";
import {
  InputGroup,
  TextInput,
  commonIdValidator,
} from "../../molecules/TextInput";
import { ToggleSwitch } from "../../molecules/ToggleSwitch";
import { ArgumentsEditor } from "../../molecules/ArgumentsEditor";
import { useHistory } from "react-router-dom";

const BoundRow = ({ bound, onChange }) => {
  if (bound.categoricalBounds) {
    return (
      <ExpandableCard label={`${bound.commonId} (categorical)`}>
        set possible values
      </ExpandableCard>
    );
  } else if (bound.numericBounds) {
    return (
      <ExpandableCard label={`${bound.commonId} (numerical)`}>
        <div className="row">
          <div className="col">
            <div className="input-group">
              <div className="input-group-prepend ml-1">
                <span className="input-group-text" id="basic-addon3">
                  Min:
                </span>
              </div>
              <input
                type="number"
                className="form-control"
                placeholder="Minimum"
                value={bound.numericBounds.min || 0}
                onChange={(e) =>
                  onChange({
                    ...bound,
                    numericBounds: {
                      min: parseFloat(e.target.value),
                      max: bound.numericBounds.max,
                    },
                  })
                }
              />
            </div>
          </div>

          <div className="col">
            <div className="input-group">
              <div className="input-group-prepend ml-1">
                <span className="input-group-text" id="basic-addon3">
                  Max:
                </span>
              </div>
              <input
                type="number"
                className="form-control"
                placeholder="Maximum"
                value={bound.numericBounds.max || 0}
                onChange={(e) =>
                  onChange({
                    ...bound,
                    numericBounds: {
                      min: bound.numericBounds.min,
                      max: parseFloat(e.target.value),
                    },
                  })
                }
              />
            </div>
          </div>
        </div>
      </ExpandableCard>
    );
  } else {
    return <div>Unknown Bound Type</div>;
  }
};
export const CreateParameterSetRecommender = () => {
  const token = useAccessToken();
  const history = useHistory();
  const [parameterToAdd, setParameterToAdd] = React.useState();
  const [availableParameters, setAvailableParameters] = React.useState([]);
  const [args, setArgs] = React.useState({});
  const [error, setError] = React.useState();
  const parameters = useParameters();
  React.useEffect(() => {
    if (parameters.items && parameters.items.length > 0) {
      setAvailableParameters(
        parameters.items.map((p) => ({
          label: `${p.name} (${p.parameterType})`,
          value: p,
        }))
      );
    }
  }, [parameters]);
  const [recommender, setRecommender] = React.useState({
    name: "",
    commonId: "",
    throwOnBadInput: false,
    parameters: [],
    bounds: [],
    arguments: [],
  });

  React.useEffect(() => {
    if (args) {
      setRecommender({
        ...recommender,
        arguments: Object.values(args),
      });
    }
  }, [args]); // TODO: fix this warning. Be careful with the re-renders.

  const handleAddParameter = () => {
    if (
      parameterToAdd &&
      !recommender.parameters.find((_) => _ === parameterToAdd.value.commonId)
    ) {
      const newBounds = {
        commonId: parameterToAdd.value.commonId,
      };

      if (parameterToAdd.value.parameterType === "numerical") {
        newBounds.numericBounds = {
          min: 0,
          max: 100,
        };
      } else {
        newBounds.categoricalBounds = { categories: [] };
      }
      setRecommender({
        ...recommender,
        parameters: [...recommender.parameters, parameterToAdd.value.commonId],
        bounds: [...recommender.bounds, newBounds],
      });
    }
  };
  const onBoundChange = (bound) => {
    setRecommender({
      ...recommender,
      bounds: [
        ...recommender.bounds.filter((_) => _.commonId !== bound.commonId),
        bound,
      ],
    });
  };
  const onSave = () => {
    createParameterSetRecommenderAsync({
      token,
      payload: recommender,
    })
      .then((r) =>
        history.push(`/recommenders/parameter-set-recommenders/detail/${r.id}`)
      )
      .catch(setError);
  };
  return (
    <React.Fragment>
      <BackButton
        to="/recommenders/parameter-set-recommenders"
        className="float-right"
      >
        Parameter Set Recommenders
      </BackButton>
      <Title>Create Recommender</Title>
      <Subtitle>Parameter Sets</Subtitle>
      <hr />
      {error && <ErrorCard error={error} />}
      <Subtitle>1. Set an ID and name.</Subtitle>
      <div className="m-1">
        <InputGroup>
          <TextInput
            validator={commonIdValidator}
            value={recommender.commonId}
            placeholder="Common Id"
            label="ID"
            onChange={(e) =>
              setRecommender({
                ...recommender,
                commonId: e.target.value,
              })
            }
          />

          <TextInput
            value={recommender.name}
            placeholder="Friendly Name"
            label="Name"
            onChange={(e) =>
              setRecommender({
                ...recommender,
                name: e.target.value,
              })
            }
          />
        </InputGroup>
      </div>
      <Subtitle>2. Define arguments</Subtitle>
      <div className="m-1">
        <ArgumentsEditor onArgumentsChanged={setArgs} initialArguments={args} />
      </div>
      <Subtitle>3. Choose parameters.</Subtitle>
      <div className="row">
        <div className="col">
          <Selector
            isSearchable
            placeholder="Select parameter"
            noOptionsMessage={(inputValue) => "No Parameters Available"}
            defaultValue={parameterToAdd}
            onChange={setParameterToAdd}
            options={availableParameters}
          />
        </div>
        <div className="col-3 text-center">
          <button
            className="btn btn-outline-primary btn-block"
            onClick={handleAddParameter}
          >
            Add
          </button>
        </div>
      </div>
      <div>
        {recommender.bounds.map((b) => (
          <BoundRow key={b.commonId} bound={b} onChange={onBoundChange} />
        ))}
      </div>
      <div className="mt-1">
        <Subtitle>4. Set error behaviour</Subtitle>
        Throw an error on bad inputs? (Choose Yes for testing)
        <ToggleSwitch
          className="ml-5"
          name="Throw on bad input"
          id="throw-on-error-toggle"
          checked={recommender.throwOnBadInput}
          onChange={(v) =>
            setRecommender({ ...recommender, throwOnBadInput: v })
          }
        />
      </div>

      <hr />
      <button onClick={onSave} className="btn btn-primary w-25 mt-3">
        Save
      </button>
    </React.Fragment>
  );
};
