import React from "react";
import { useParams } from "react-router-dom";
import { usePromotionsRecommender } from "../../../api-hooks/promotionsRecommendersApi";
import { invokePromotionsRecommenderAsync } from "../../../api/promotionsRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import { Title, Subtitle, AsyncButton, BackButton } from "../../molecules";
import { JsonView } from "../../molecules/JsonView";
import { ConsumeRecommendationPopup } from "../utils/consumeRecommendationPopup";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";
import { AsyncSelectCustomer } from "../../molecules/selectors/AsyncSelectCustomer";
import { useAnalytics } from "../../../analytics/analyticsHooks";
import { AsyncSelectBusiness } from "../../molecules/selectors/AsyncSelectBusiness";
import { Row } from "../../molecules/layout";

export const TestRecommender = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const recommender = usePromotionsRecommender({ id });
  const { analytics } = useAnalytics();
  const [consumePopupOpen, setConsumePopupOpen] = React.useState(false);

  const [selectedCustomer, setSelectedCustomer] = React.useState();
  const [selectedBusiness, setSelectedBusiness] = React.useState();
  const [loading, setInvoking] = React.useState(false);
  const [modelResponse, setModelResponse] = React.useState();
  const handleInvoke = () => {
    setInvoking(true);
    invokePromotionsRecommenderAsync({
      token,
      id: recommender?.id || id,
      input: {
        customerId: selectedCustomer?.commonId,
        businessId: selectedBusiness?.commonId,
        arguments: {},
      },
    })
      .then((r) => {
        analytics.track("site:testItemsRecommender_invoke_success");
        setModelResponse(r);
      })
      .catch((error) => {
        analytics.track("site:testItemsRecommender_invoke_failure");
        setModelResponse({ error });
      })
      .finally(() => setInvoking(false));
  };

  return (
    <React.Fragment>
      <ItemRecommenderLayout>
        <Row className="mb-2">
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
          <Row>
            <div className="col">
              <JsonView
                data={modelResponse}
                shouldExpandNode={(n) => n.includes("scoredItems")}
              />
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
      </ItemRecommenderLayout>
    </React.Fragment>
  );
};
