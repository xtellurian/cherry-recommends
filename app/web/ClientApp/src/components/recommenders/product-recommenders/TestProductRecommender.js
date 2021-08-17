import React from "react";
import { useParams } from "react-router-dom";
import { useProductRecommender } from "../../../api-hooks/productRecommendersApi";
import { invokeProductRecommender } from "../../../api/productRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import { Title, Subtitle, AsyncButton, BackButton } from "../../molecules";
import { JsonView } from "../../molecules/JsonView";
import { AsyncSelectTrackedUser } from "../../molecules/selectors/AsyncSelectTrackedUser";

export const TestProductRecommender = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const recommender = useProductRecommender({ id });

  const [selectedTrackedUser, setSelectedTrackedUser] = React.useState();
  const [loading, setInvoking] = React.useState(false);
  const [modelResponse, setModelResponse] = React.useState();
  const handleInvoke = () => {
    setInvoking(true);
    invokeProductRecommender({
      success: setModelResponse,
      error: (error) => setModelResponse({ error }),
      onFinally: () => setInvoking(false),
      token,
      id: recommender.id,
      input: {
        commonUserId: selectedTrackedUser.commonId,
        productRecommenderId: recommender.id,
      },
    });
  };

  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to={`/recommenders/product-recommenders/detail/${recommender.id}`}
      >
        Recommender
      </BackButton>
      <Title>Test Product Recommender</Title>
      <Subtitle>{recommender.name || "..."}</Subtitle>
      <hr />

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
      <div>{modelResponse && <JsonView data={modelResponse} />}</div>
    </React.Fragment>
  );
};
