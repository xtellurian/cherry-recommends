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

export const TestRecommender = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const recommender = usePromotionsRecommender({ id });
  const { analytics } = useAnalytics();
  const [consumePopupOpen, setConsumePopupOpen] = React.useState(false);

  const [selectedTrackedUser, setSelectedTrackedUser] = React.useState();
  const [loading, setInvoking] = React.useState(false);
  const [modelResponse, setModelResponse] = React.useState();
  const handleInvoke = () => {
    setInvoking(true);
    invokePromotionsRecommenderAsync({
      token,
      id: recommender?.id || id,
      input: {
        commonUserId: selectedTrackedUser.commonId,
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
        <AsyncSelectCustomer
          placeholder="Search for a user to make a recommendation for."
          onChange={(v) => setSelectedTrackedUser(v.value)}
        />

        <AsyncButton
          disabled={!selectedTrackedUser}
          onClick={handleInvoke}
          className="btn btn-primary m-2 w-25"
          loading={loading}
        >
          Invoke
        </AsyncButton>
        {modelResponse && (
          <div className="row">
            <div className="col">
              <JsonView
                data={modelResponse}
                shouldExpandNode={(n) => n.includes("scoredItems")}
              />
            </div>
            <div className="col-4">
              <button
                className="btn btn-primary"
                onClick={() => setConsumePopupOpen(true)}
              >
                Consume Recommendation
              </button>
              <ConsumeRecommendationPopup
                recommendation={modelResponse}
                isOpen={consumePopupOpen}
                setIsOpen={setConsumePopupOpen}
                onConsumed={() => setModelResponse(null)}
              />
            </div>
          </div>
        )}
      </ItemRecommenderLayout>
    </React.Fragment>
  );
};
