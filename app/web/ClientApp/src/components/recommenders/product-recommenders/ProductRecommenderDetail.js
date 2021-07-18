import React from "react";
import { useHistory, useParams } from "react-router-dom";
import { useProductRecommender } from "../../../api-hooks/productRecommendersApi";
import { deleteProductRecommender } from "../../../api/productRecommendersApi";
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
import {
  ActionLink,
  ActionItemsGroup,
  ActionsButton,
  ActionItem,
} from "../../molecules/ActionsButton";
import { ConfirmationPopup } from "../../molecules/ConfirmationPopup";
import { CopyableField } from "../../molecules/CopyableField";

export const ProductRecommenderDetail = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const history = useHistory();
  const recommender = useProductRecommender({ id });
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [deleteError, setDeleteError] = React.useState();
  const onDeleted = () => {
    history.push("/recommenders/product-recommenders");
  };
  return (
    <React.Fragment>
      <ActionsButton
        to={`/recommenders/product-recommenders/recommendations/${id}`}
        label="Latest Recommendations"
      >
        <ActionItemsGroup label="Actions">
          <ActionLink to={`/recommenders/product-recommenders/test/${id}`}>
            Test Page
          </ActionLink>
          <ActionLink
            to={`/recommenders/product-recommenders/target-variable/${id}`}
          >
            Target Variable
          </ActionLink>
          <ActionLink to={`/recommenders/product-recommenders/integrate/${id}`}>
            Technical Integration
          </ActionLink>
          <ActionLink
            to={`/recommenders/product-recommenders/link-to-model/${id}`}
          >
            Link to Model
          </ActionLink>
          <ActionItem onClick={() => setDeleteOpen(true)}>Delete</ActionItem>

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
        </ActionItemsGroup>
      </ActionsButton>
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
            <RecommenderStatusBox recommender={recommender} />
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
