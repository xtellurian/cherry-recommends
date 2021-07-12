import React from "react";
import { useHistory } from "react-router-dom";
import { useAccessToken } from "../../../api-hooks/token";
import { useProducts } from "../../../api-hooks/productsApi";
import { useTouchpoints } from "../../../api-hooks/touchpointsApi";
import {
  createTouchpointMetadata,
  fetchTouchpoint,
} from "../../../api/touchpointsApi";
import { createProductRecommender } from "../../../api/productRecommendersApi";
import {
  Title,
  BackButton,
  Selector,
  AsyncButton,
  ErrorCard,
} from "../../molecules";

export const CreateProductRecommender = () => {
  const token = useAccessToken();
  const history = useHistory();
  const products = useProducts();
  const [error, setError] = React.useState();
  const productOptions = products.items
    ? products.items.map((p) => ({ label: p.name, value: p.commonId }))
    : [];

  const [selectedProducts, setSelectedProducts] = React.useState();
  const [recommender, setRecommender] = React.useState({
    commonId: "",
    name: "",
    touchpoint: "",
    productIds: null,
  });

  const allTouchpoints = useTouchpoints();

  const [loading, setLoading] = React.useState(false);
  const createRecommenderAfterTouchpoint = (tp) => {
    setLoading(true);
    recommender.touchpoint = tp.commonId;
    createProductRecommender({
      success: (pr) =>
        history.push(`/recommenders/product-recommenders/detail/${pr.id}`),
      error: setError,
      token,
      payload: recommender,
      onFinally: () => setLoading(false),
    });
  };

  const handleCreate = () => {
    setLoading(true);
    if (
      allTouchpoints.items &&
      allTouchpoints.items.some((_) => _.commonId === recommender.commonId)
    ) {
      fetchTouchpoint({
        success: createRecommenderAfterTouchpoint,
        error: setError,
        token,
        id: recommender.commonId,
      });
    } else {
      createTouchpointMetadata({
        success: createRecommenderAfterTouchpoint,
        error: setError,
        token,
        payload: {
          commonId: recommender.commonId,
          name: `Autogenerated for Product Recommender (${recommender.commonId})`,
        },
        onFinally: () => setLoading(false),
      });
    }
  };

  return (
    <React.Fragment>
      <BackButton
        className="float-right"
        to="/recommenders/product-recommenders"
      >
        Product Recommenders
      </BackButton>
      <Title>Create Product Recommender</Title>
      <hr />
      {error && <ErrorCard error={error} />}
      <div className="input-group m-1">
        <div className="input-group-prepend ml-1">
          <span className="input-group-text" id="basic-addon3">
            Display Name
          </span>
        </div>
        <input
          type="text"
          className="form-control"
          placeholder="A memorable name that you recognise later."
          value={recommender.name}
          onChange={(e) =>
            setRecommender({
              ...recommender,
              name: e.target.value,
            })
          }
        />
      </div>
      <div className="input-group m-1">
        <div className="input-group-prepend ml-1">
          <span className="input-group-text" id="basic-addon3">
            Identifier
          </span>
        </div>
        <input
          type="text"
          className="form-control"
          placeholder="A unique Id for this recommender resource."
          value={recommender.commonId}
          onChange={(e) =>
            setRecommender({
              ...recommender,
              commonId: e.target.value,
            })
          }
        />
      </div>
      <div className="m-1">
        <Selector
          isMulti
          isSearchable
          placeholder="Select products. Leave empty to include all products."
          noOptionsMessage={(inputValue) => "No Products Available"}
          defaultValue={selectedProducts}
          onChange={(so) => {
            setSelectedProducts(so);
            setRecommender({
              ...recommender,
              productIds: so.map((o) => o.value),
            });
          }}
          options={productOptions}
        />
      </div>
      <div>
        <AsyncButton
          onClick={handleCreate}
          loading={loading || allTouchpoints.loading}
        >
          Create
        </AsyncButton>
      </div>
    </React.Fragment>
  );
};