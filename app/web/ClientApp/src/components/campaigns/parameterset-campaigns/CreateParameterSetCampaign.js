import React from "react";
import { useAccessToken } from "../../../api-hooks/token";
import { useParameters } from "../../../api-hooks/parametersApi";
import { createParameterSetCampaignAsync } from "../../../api/parameterSetCampaignsApi";
import {
  Subtitle,
  Selector,
  ExpandableCard,
  AsyncButton,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
} from "../../molecules";
import {
  InputGroup,
  TextInput,
  commonIdValidator,
} from "../../molecules/TextInput";
import { ToggleSwitch } from "../../molecules/ToggleSwitch";
import { ArgumentsEditor } from "../../molecules/ArgumentsEditor";
import { useAnalytics } from "../../../analytics/analyticsHooks";
import { useCommonId } from "../../../utility/utility";
import CreatePageLayout from "../../molecules/layout/CreatePageLayout";
import { useNavigation } from "../../../utility/useNavigation";

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
export const CreateParameterSetCampaign = () => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const [parameterToAdd, setParameterToAdd] = React.useState();
  const [availableParameters, setAvailableParameters] = React.useState([]);
  const [args, setArgs] = React.useState({});
  const [error, setError] = React.useState();
  const parameters = useParameters();
  const { analytics } = useAnalytics();
  const { generateCommonId } = useCommonId();
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
  const [campaign, setCampaign] = React.useState({
    name: "",
    commonId: "",
    throwOnBadInput: false,
    parameters: [],
    bounds: [],
    arguments: [],
  });

  React.useEffect(() => {
    if (args) {
      setCampaign({
        ...campaign,
        arguments: Object.values(args),
      });
    }
  }, [args]); // TODO: fix this warning. Be careful with the re-renders.

  const handleAddParameter = () => {
    if (
      parameterToAdd &&
      !campaign.parameters.find((_) => _ === parameterToAdd.value.commonId)
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
      setCampaign({
        ...campaign,
        parameters: [...campaign.parameters, parameterToAdd.value.commonId],
        bounds: [...campaign.bounds, newBounds],
      });
    }
  };
  const onBoundChange = (bound) => {
    setCampaign({
      ...campaign,
      bounds: [
        ...campaign.bounds.filter((_) => _.commonId !== bound.commonId),
        bound,
      ],
    });
  };
  const [loading, setLoading] = React.useState(false);
  const onSave = () => {
    setLoading(true);
    createParameterSetCampaignAsync({
      token,
      payload: campaign,
    })
      .then((r) => {
        analytics.track("site:parameterSetCampaign_create_success");
        navigate(`/campaigns/parameter-set-campaigns/detail/${r.id}`);
      })
      .catch((e) => {
        analytics.track("site:parameterSetCampaign_create_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };

  React.useEffect(() => {
    setCampaign({
      ...campaign,
      commonId: generateCommonId(campaign.name),
    });
  }, [campaign.name]);

  return (
    <CreatePageLayout
      createButton={
        <AsyncButton
          loading={loading}
          onClick={onSave}
          className="btn btn-primary"
        >
          Create
        </AsyncButton>
      }
      backButton={
        <MoveUpHierarchyPrimaryButton to="/campaigns/parameter-set-campaigns">
          Parameter Set Campaigns
        </MoveUpHierarchyPrimaryButton>
      }
      header={<PageHeading title="Create Campaign" subtitle="Parameter Sets" />}
      error={error}
    >
      <Subtitle>1. Set an ID and name.</Subtitle>
      <div className="m-1">
        <InputGroup>
          <TextInput
            validator={commonIdValidator}
            value={campaign.commonId}
            placeholder="Common Id"
            label="ID"
            onChange={(e) =>
              setCampaign({
                ...campaign,
                commonId: e.target.value,
              })
            }
          />

          <TextInput
            value={campaign.name}
            placeholder="Friendly Name"
            label="Name"
            onChange={(e) =>
              setCampaign({
                ...campaign,
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
        {campaign.bounds.map((b) => (
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
          checked={campaign.throwOnBadInput}
          onChange={(v) => setCampaign({ ...campaign, throwOnBadInput: v })}
        />
      </div>
    </CreatePageLayout>
  );
};
