import React from "react";
import { useParams } from "react-router-dom";
import { useItemsRecommender } from "../../../api-hooks/itemsRecommendersApi";
import { invokeItemsRecommenderAsync } from "../../../api/itemsRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import { Title, Subtitle, AsyncButton, BackButton } from "../../molecules";
import { JsonView } from "../../molecules/JsonView";
import { AsyncSelectTrackedUser } from "../../molecules/selectors/AsyncSelectTrackedUser";
import { ConsumeRecommendationPopup } from "../utils/consumeRecommendationPopup";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";

export const TestRecommender = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const recommender = useItemsRecommender({ id });

  const [consumePopupOpen, setConsumePopupOpen] = React.useState(false);

  const [selectedTrackedUser, setSelectedTrackedUser] = React.useState();
  const [loading, setInvoking] = React.useState(false);
  const [modelResponse, setModelResponse] = React.useState();
  const handleInvoke = () => {
    setInvoking(true);
    invokeItemsRecommenderAsync({
      token,
      id: recommender?.id || id,
      input: {
        commonUserId: selectedTrackedUser.commonId,
        arguments: {},
      },
    })
      .then(setModelResponse)
      .catch((error) => setModelResponse({ error }))
      .finally(() => setInvoking(false));
  };

  return (
    <React.Fragment>
      <ItemRecommenderLayout>
        

        <AsyncSelectTrackedUser
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
