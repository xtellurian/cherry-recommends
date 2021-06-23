import React from "react";
import { useAccessToken } from "../../../api-hooks/token";
import { useParameters } from "../../../api-hooks/parametersApi";
import { createParameterSetRecommender } from "../../../api/parameterSetRecommendersApi";
import { Title, Subtitle } from "../../molecules/PageHeadings";
import { ArgumentsEditor } from "../../molecules/ArgumentsEditor";
import { Selector } from "../../molecules/Select";
import { ExpandableCard } from "../../molecules/ExpandableCard";
import { useHistory } from "react-router-dom";
import { ErrorCard } from "../../molecules/ErrorCard";

const BoundRow = ({ bound, onChange }) => {
  if (bound.categoricalBounds) {
    return (
      <ExpandableCard name={`${bound.commonId} (categorical)`}>
        set possible values
      </ExpandableCard>
    );
  } else if (bound.numericBounds) {
    return (
      <ExpandableCard name={`${bound.commonId} (numerical)`}>
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
    createParameterSetRecommender({
      success: (r) =>
        history.push(`/recommenders/parameter-set-recommenders/detail/${r.id}`),
      error: setError,
      token,
      payload: recommender,
    });
  };
  return (
    <React.Fragment>
      <Title>Create Recommender</Title>
      <Subtitle>Parameter Sets</Subtitle>
      <hr />
      {error && <ErrorCard error={error} />}

      <Subtitle>1. Set identifiers.</Subtitle>
      <div className="m-1">
        <div className="input-group">
          <div className="input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              Identifier:
            </span>
          </div>
          <input
            type="text"
            className="form-control"
            placeholder="Common Id"
            value={recommender.commonId}
            onChange={(e) =>
              setRecommender({
                ...recommender,
                commonId: e.target.value,
              })
            }
          />
          <div className="input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              Friendly Name:
            </span>
          </div>
          <input
            type="text"
            className="form-control"
            placeholder="Name"
            value={recommender.name}
            onChange={(e) =>
              setRecommender({
                ...recommender,
                name: e.target.value,
              })
            }
          />
        </div>
      </div>
      <Subtitle>2. Define arguments.</Subtitle>
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
      <hr />
      <button onClick={onSave} className="btn btn-primary w-25 mt-3">
        Save
      </button>
    </React.Fragment>
  );
};
