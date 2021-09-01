import React from "react";
import { useParams } from "react-router-dom";
import { useProducts } from "../../../api-hooks/productsApi";
import {
  useProductRecommender,
  useDefaultProduct,
} from "../../../api-hooks/productRecommendersApi";
import { useAccessToken } from "../../../api-hooks/token";
import {
  updateErrorHandlingAsync,
  setDefaultProductAsync,
} from "../../../api/productRecommendersApi";
import { Selector } from "../../molecules/Select";
import { SettingsUtil } from "../utils/settingsUtil";
import { Spinner } from "../../molecules";

export const Settings = () => {
  const { id } = useParams();
  const [updatedErrorHandling, setUpdatedErrorHandling] = React.useState({});
  const recommender = useProductRecommender({
    id,
    trigger: updatedErrorHandling,
  });
  const token = useAccessToken();
  const products = useProducts();
  const productOptions = products.items
    ? products.items.map((p) => ({ label: p.name, value: p.commonId }))
    : [];

  const handleUpdateError = (e) => {
    alert(e.title);
  };

  const [updatedDefaultProduct, setUpdatedDefaultProduct] = React.useState({});
  const defaultProduct = useDefaultProduct({
    id,
    trigger: updatedDefaultProduct,
  });
  const handleUpdate = (errorHandling) => {
    updateErrorHandlingAsync({
      id,
      token,
      errorHandling,
    })
      .then(setUpdatedErrorHandling)
      .catch(handleUpdateError);
  };

  const handleSetDefaultProduct = (productId) => {
    setDefaultProductAsync({ token, id, productId })
      .then(setUpdatedDefaultProduct)
      .catch(handleUpdateError);
  };
  return (
    <React.Fragment>
      {recommender.loading && <Spinner>Loading Recommender</Spinner>}
      {!recommender.loading && (
        <React.Fragment>
          <SettingsUtil
            recommender={recommender}
            basePath="/recommenders/product-recommenders"
            updateErrorHandling={handleUpdate}
          />
          <div className="row">
            <div className="col">
              <h5>Default Product</h5>
              Choosing a default product helps the recommender know what to do
              in error situations or when there's no information about a tracked
              user.
            </div>
            <div className="col-3">
              {defaultProduct.loading ? (
                <Spinner />
              ) : (
                <Selector
                  isSearchable
                  placeholder={
                    defaultProduct.name || "Choose a default product."
                  }
                  noOptionsMessage={(inputValue) => "No Products Available"}
                  onChange={(so) => handleSetDefaultProduct(so.value)}
                  options={productOptions}
                />
              )}
            </div>
          </div>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
