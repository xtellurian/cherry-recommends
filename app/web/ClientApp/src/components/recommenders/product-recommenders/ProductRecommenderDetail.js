import React from "react";
import { useHistory, useParams } from "react-router-dom";
import { useProductRecommender } from "../../../api-hooks/productRecommendersApi";
import {
  deleteProductRecommender,
  createProductRecommenderAsync,
} from "../../../api/productRecommendersApi";
import { ProductRow } from "../../products/ProductRow";
import { useAccessToken } from "../../../api-hooks/token";
import {
  Title,
  Subtitle,
  BackButton,
  Spinner,
  ErrorCard,
  EmptyList,
} from "../../molecules";
import { RecommenderStatusBox } from "../../molecules/RecommenderStatusBox";
import { ActionsButtonUtil } from "../utils/actionsButtonUtil";
import { ConfirmationPopup } from "../../molecules/popups/ConfirmationPopup";
import { CopyableField } from "../../molecules/fields/CopyableField";
import { EntityField } from "../../molecules/EntityField";
import { CloneRecommender } from "../utils/CloneRecommender";

export const ProductRecommenderDetail = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const history = useHistory();
  const recommender = useProductRecommender({ id });
  const [cloneOpen, setCloneOpen] = React.useState(false);
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [deleteError, setDeleteError] = React.useState();
  const onDeleted = () => {
    history.push("/recommenders/product-recommenders");
  };

  const cloneAsync = (name, commonId) => {
    return createProductRecommenderAsync({
      token,
      payload: {
        name,
        commonId,
        cloneFromId: recommender.id,
      },
    });
  };

  return (
    <React.Fragment>
      <ActionsButtonUtil
        id={id}
        basePath="/recommenders/product-recommenders"
        setDeleteOpen={setDeleteOpen}
      >
        <ConfirmationPopup
          isOpen={deleteOpen}
          setIsOpen={setDeleteOpen}
          label="Are you sure you want to delete this model?"
        >
          <div className="m-2">{recommender.name}</div>
          {deleteError && <ErrorCard error={deleteError} />}
          <div
            className="btn-group"
            role="group"
            aria-label="Delete or cancel buttons"
          >
            <button
              className="btn btn-secondary"
              onClick={() => setDeleteOpen(false)}
            >
              Cancel
            </button>
            <button
              className="btn btn-danger"
              onClick={() => {
                deleteProductRecommender({
                  success: () => {
                    setDeleteOpen(false);
                    if (onDeleted) {
                      onDeleted();
                    }
                  },
                  error: setDeleteError,
                  token,
                  id: recommender.id,
                });
              }}
            >
              Delete
            </button>
          </div>
        </ConfirmationPopup>
      </ActionsButtonUtil>

      <BackButton
        className="float-right mr-1"
        to="/recommenders/product-recommenders"
      >
        Product Recommenders
      </BackButton>
      <Title>Product Recommender</Title>
      <Subtitle>{recommender.name || "..."}</Subtitle>
      <hr />
      {recommender.loading && <Spinner>Loading Recommender</Spinner>}
      {recommender.error && <ErrorCard error={recommender.error} />}

      <div className="row">
        <div className="col-md order-last">
          {!recommender.loading && !recommender.error && (
            <React.Fragment>
              <RecommenderStatusBox recommender={recommender} />
              <button
                className="btn btn-outline-primary btn-block"
                onClick={() => setCloneOpen(true)}
              >
                Clone this Recommender
              </button>
              <ConfirmationPopup
                isOpen={cloneOpen}
                setIsOpen={setCloneOpen}
                label="Clone this recommender?"
              >
                <CloneRecommender
                  recommender={recommender}
                  cloneAsync={cloneAsync}
                  onCloned={(r) =>
                    history.push(
                      `/recommenders/product-recommenders/detail/${r.id}`
                    )
                  }
                />
              </ConfirmationPopup>
            </React.Fragment>
          )}
        </div>
        <div className="col-8">
          {recommender.commonId && (
            <CopyableField label="Common Id" value={recommender.commonId} />
          )}
          {recommender.touchpoint && (
            <CopyableField
              label="Touchpoint Id"
              value={recommender.touchpoint.commonId}
            />
          )}
          {recommender.defaultProduct && (
            <EntityField
              label="Default Product"
              entity={recommender.defaultProduct}
              to={`/products/detail/${recommender.defaultProductId}`}
            />
          )}
          <div className="mt-2">
            <Subtitle>Associated Products</Subtitle>
            {recommender.products &&
              recommender.products.map((p) => (
                <ProductRow product={p} key={p.id} />
              ))}
            {recommender.products && recommender.products.length === 0 && (
              <EmptyList>This recommender works with all products.</EmptyList>
            )}
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};
