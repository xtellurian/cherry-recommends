import React from "react";
import { useParams } from "react-router-dom";
import { useParameterSetCampaign } from "../../../api-hooks/parameterSetCampaignsApi";
import { invokeParameterSetCampaignAsync } from "../../../api/parameterSetCampaignsApi";
import { useAccessToken } from "../../../api-hooks/token";
import {
  AsyncButton,
  ExpandableCard,
  ErrorCard,
  Spinner,
  Subtitle,
  Title,
} from "../../molecules";
import { JsonView } from "../../molecules/JsonView";
import { ConsumeRecommendationPopup } from "../utils/consumeRecommendationPopup";
import { ParameterSetCampaignLayout } from "./ParameterSetCampaignLayout";
import { AsyncSelectCustomer } from "../../molecules/selectors/AsyncSelectCustomer";
import { useAnalytics } from "../../../analytics/analyticsHooks";

const CategoricalArgumentInput = ({ arg, value, onChange }) => {
  return (
    <div className="row">
      <div className="col-8">
        <div className="input-group">
          <div className="input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              {arg.commonId}:
            </span>
          </div>
          <input
            type="text"
            className="form-control"
            placeholder={arg.commonId}
            value={value || ""}
            onChange={(e) => onChange(e.target.value)}
          />
        </div>
      </div>
    </div>
  );
};
const NumericalArgumentInput = ({ arg, value, onChange }) => {
  return (
    <div className="row">
      <div className="col-8">
        <div className="input-group">
          <div className="input-group-prepend ml-1">
            <span className="input-group-text" id="basic-addon3">
              {arg.commonId}:
            </span>
          </div>
          <input
            type="text"
            className="form-control"
            placeholder={arg.commonId}
            value={value || ""}
            onChange={(e) => onChange(e.target.value)}
          />
        </div>
      </div>
    </div>
  );
};
const ArgumentInput = ({ arg, value, onChange }) => {
  if (arg.argumentType === "numerical") {
    return (
      <NumericalArgumentInput arg={arg} value={value} onChange={onChange} />
    );
  } else if (arg.argumentType === "categorical") {
    return (
      <CategoricalArgumentInput arg={arg} value={value} onChange={onChange} />
    );
  } else {
    return <div>Unknown Argument Type</div>;
  }
};
export const TestParameterSetCampaign = () => {
  const { id } = useParams();
  const parameterSetRecommender = useParameterSetCampaign({ id });
  const token = useAccessToken();
  const { analytics } = useAnalytics();
  const [argValues, setArgValues] = React.useState({});
  const [selectedTrackedUser, setSelectedTrackedUser] = React.useState();

  const [consumePopupOpen, setConsumePopupOpen] = React.useState(false);

  React.useEffect(() => {
    if (parameterSetRecommender.arguments) {
      const newArgValues = {};
      for (const a of parameterSetRecommender.arguments) {
        if (a.argumentType === "numerical") {
          newArgValues[a.commonId] = a.defaultValue?.value || 0;
        } else if (a.argumentType === "categorical") {
          newArgValues[a.commonId] = a.defaultValue?.value || "";
        }
      }
      setArgValues(newArgValues);
    }
  }, [parameterSetRecommender]);

  const [loading, setLoading] = React.useState(false);
  const [error, setError] = React.useState();
  const [response, setResponse] = React.useState();
  const handleInvoke = () => {
    setLoading(true);
    setError(null);
    invokeParameterSetCampaignAsync({
      token,
      id: parameterSetRecommender.id,
      // version,
      input: {
        commonUserId: selectedTrackedUser.commonId,
        arguments: argValues,
      },
    })
      .then((r) => {
        analytics.track("site:testParameterSetRecommender_invoke_success");
        setResponse(r);
      })
      .catch((e) => {
        analytics.track("site:testParameterSetRecommender_invoke_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };

  return (
    <React.Fragment>
      <ParameterSetCampaignLayout>
        {error && <ErrorCard error={error} />}
        <AsyncSelectCustomer
          placeholder="Search for a Customer to make a recommendation for."
          onChange={(v) => setSelectedTrackedUser(v.value)}
        />
        <div className="row mt-3">
          <div className="col">
            <Subtitle>Arguments</Subtitle>
            {parameterSetRecommender.loading && <Spinner />}
            {parameterSetRecommender.arguments &&
              parameterSetRecommender.arguments.map((a) => (
                <ArgumentInput
                  key={a.commonId}
                  arg={a}
                  value={argValues[a.commonId]}
                  onChange={(v) =>
                    setArgValues({ ...argValues, [a.commonId]: v })
                  }
                />
              ))}

            <AsyncButton
              disabled={!selectedTrackedUser}
              onClick={handleInvoke}
              className="btn btn-primary m-2 w-50"
              loading={loading}
            >
              Invoke
            </AsyncButton>
          </div>
          <div className="col">
            <Subtitle>Model Response</Subtitle>
            {response && (
              <JsonView
                data={response}
                shouldExpandNode={(n) => n.includes("recommendedParameters")}
              />
            )}
            {loading && <Spinner />}
            {!response && !loading && (
              <div className="text-muted text-center">No Response.</div>
            )}
            {response && (
              <React.Fragment>
                <button
                  className="btn btn-primary"
                  onClick={() => setConsumePopupOpen(true)}
                >
                  Consume Recommendation
                </button>
                <ConsumeRecommendationPopup
                  recommendation={response}
                  isOpen={consumePopupOpen}
                  setIsOpen={setConsumePopupOpen}
                  onConsumed={() => setResponse(null)}
                />
              </React.Fragment>
            )}
          </div>
        </div>
      </ParameterSetCampaignLayout>
    </React.Fragment>
  );
};
