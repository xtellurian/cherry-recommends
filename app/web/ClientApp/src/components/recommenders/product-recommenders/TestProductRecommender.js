import React from "react";
import { useParams } from "react-router-dom";
import { useProductRecommender } from "../../../api-hooks/productRecommendersApi";
import { invokeProductRecommender } from "../../../api/productRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import { useTrackedUsers } from "../../../api-hooks/trackedUserApi";
import { fetchTrackedUsers } from "../../../api/trackedUsersApi";
import { Title, Subtitle, AsyncButton, BackButton } from "../../molecules";
import { AsyncSelector } from "../../molecules/AsyncSelect";
import { JsonView } from "../../molecules/JsonView";

export const TestProductRecommender = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const recommender = useProductRecommender({ id });
  const trackedUsers = useTrackedUsers({});

  const trackedUsersSelectable = trackedUsers.items
    ? trackedUsers.items.map((u) => ({
        label: u.name || u.commonId,
        value: u,
      }))
    : [];
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

  const loadUsers = (inputValue, callback) => {
    fetchTrackedUsers({
      success: (r) =>
        callback(
          r.items.map((x) => ({ value: x, label: x.name || x.commonId }))
        ),
      error: (e) => console.log(e),
      token,
      searchTerm: inputValue,
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

      {/* <Selector
        placeholder="Choose a user to make a recommendation for."
        onChange={(v) => setSelectedTrackedUser(v.value)}
        options={trackedUsersSelectable}
      /> */}

      <AsyncSelector
        defaultOptions={trackedUsersSelectable}
        placeholder="Search for a user to make a recommendation for."
        cacheOptions
        loadOptions={loadUsers}
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
