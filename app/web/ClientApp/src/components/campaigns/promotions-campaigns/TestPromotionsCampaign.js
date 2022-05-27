import React from "react";
import { useParams } from "react-router-dom";
import {
  useArguments,
  usePromotionsCampaign,
} from "../../../api-hooks/promotionsCampaignsApi";
import { invokePromotionsCampaignAsync } from "../../../api/promotionsCampaignsApi";
import { useAccessToken } from "../../../api-hooks/token";
import {
  AsyncButton,
  EmptyList,
  ExpandableCard,
  Spinner,
  Typography,
} from "../../molecules";
import { JsonView } from "../../molecules/JsonView";
import { ConsumeRecommendationPopup } from "../utils/consumeRecommendationPopup";
import { PromotionCampaignLayout } from "./PromotionCampaignLayout";
import { AsyncSelectCustomer } from "../../molecules/selectors/AsyncSelectCustomer";
import { useAnalytics } from "../../../analytics/analyticsHooks";
import { AsyncSelectBusiness } from "../../molecules/selectors/AsyncSelectBusiness";
import { Row } from "../../molecules/layout";
import { MonitorCampaign } from "./MonitorPromotionsCampaign";
import { CampaignCard } from "../CampaignCard";
import { TextInput } from "../../molecules/TextInput";

const SetArgument = ({ arg, setArgument }) => {
  const [value, setValue] = React.useState("");
  React.useEffect(() => {
    if (arg && arg.commonId) {
      setArgument(arg.commonId, value);
    }
  }, [value, arg]);
  if (!arg) {
    return null;
  }
  return (
    <React.Fragment>
      <TextInput
        label={arg.commonId}
        value={value}
        onChange={(e) => setValue(e.target.value)}
      />
    </React.Fragment>
  );
};
export const TestCampaign = () => {
  const [trigger, setTrigger] = React.useState({});
  const { id } = useParams();
  const token = useAccessToken();
  const recommender = usePromotionsCampaign({ id });
  const args = useArguments({ id });
  const { analytics } = useAnalytics();
  const [consumePopupOpen, setConsumePopupOpen] = React.useState(false);

  const [selectedCustomer, setSelectedCustomer] = React.useState();
  const [selectedBusiness, setSelectedBusiness] = React.useState();
  const [loading, setInvoking] = React.useState(false);
  const [modelResponse, setModelResponse] = React.useState();
  const [argValues, setArgValues] = React.useState({});
  const handleInvoke = () => {
    setInvoking(true);
    invokePromotionsCampaignAsync({
      token,
      id: recommender?.id || id,
      input: {
        customerId: selectedCustomer?.commonId,
        businessId: selectedBusiness?.commonId,
        arguments: argValues,
      },
    })
      .then((r) => {
        analytics.track("site:testPromotionsCampaign_invoke_success");
        setModelResponse(r);
      })
      .catch((error) => {
        analytics.track("site:testPromotionsCampaign_invoke_failure");
        setModelResponse({ error });
      })
      .finally(() => {
        setInvoking(false);
        setTrigger({});
      });
  };
  const setArgument = (argCommonId, value) => {
    setArgValues({ ...argValues, [argCommonId]: value });
  };

  return (
    <React.Fragment>
      <PromotionCampaignLayout>
        <CampaignCard title="Test">
          <Row>
            <div className="col">
              {recommender.targetType === "customer" ? (
                <AsyncSelectCustomer
                  placeholder="Search for a Customer to make a recommendation for."
                  onChange={(v) => setSelectedCustomer(v.value)}
                />
              ) : null}
              {recommender.targetType === "business" ? (
                <AsyncSelectBusiness
                  placeholder="Search for a business to make a recommendation for."
                  onChange={(v) => setSelectedBusiness(v.value)}
                />
              ) : null}
              <div>
                <Typography variant="h7">Arguments</Typography>
                {args.loading ? (
                  <Spinner />
                ) : Array.isArray(args) ? (
                  args.map((a) => (
                    <SetArgument key={a.id} arg={a} setArgument={setArgument} />
                  ))
                ) : null}
                <SetArgument />
                {args.length === 0 ? <EmptyList>No Arguments</EmptyList> : null}
              </div>
            </div>

            <div className="col">
              <AsyncButton
                disabled={!selectedCustomer && !selectedBusiness}
                onClick={handleInvoke}
                className="btn btn-primary w-100"
                loading={loading}
              >
                Invoke
              </AsyncButton>
            </div>
          </Row>
          {modelResponse ? (
            <Row className="mt-4">
              <div className="col">
                <ExpandableCard label="Response">
                  <JsonView
                    data={modelResponse}
                    shouldExpandNode={(n) => n.includes("scoredItems")}
                  />
                </ExpandableCard>
              </div>
              <div className="col">
                <button
                  disabled={recommender.targetType === "business"}
                  className="btn btn-outline-primary w-100"
                  onClick={() => setConsumePopupOpen(true)}
                >
                  Consume Recommendation
                </button>
                {recommender.targetType === "business" ? (
                  <div className="text-muted">
                    Businesses may not consume recommendations
                  </div>
                ) : null}
                <ConsumeRecommendationPopup
                  recommendation={modelResponse}
                  isOpen={consumePopupOpen}
                  setIsOpen={setConsumePopupOpen}
                  onConsumed={() => setModelResponse(null)}
                />
              </div>
            </Row>
          ) : null}
        </CampaignCard>
        <CampaignCard title="Monitor">
          <MonitorCampaign trigger={trigger} />
        </CampaignCard>
      </PromotionCampaignLayout>
    </React.Fragment>
  );
};
