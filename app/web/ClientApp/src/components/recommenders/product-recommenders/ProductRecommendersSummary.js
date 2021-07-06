import React from "react";
import { Link } from "react-router-dom";
import {
  Title,
  ErrorCard,
  Spinner,
  Paginator,
  EmptyList,
  ExpandableCard,
} from "../../molecules";
import { CreateButton } from "../../molecules/CreateButton";
import { useProductRecommenders } from "../../../api-hooks/productRecommendersApi";
import { JsonView } from "../../molecules/JsonView";

const ProductRecommenderRow = ({ recommender }) => {
  return (
    <ExpandableCard label={recommender.name}>
      <Link to={`/recommenders/product-recommenders/detail/${recommender.id}`}>
        <button className="float-right btn btn-primary">View Details</button>
      </Link>
      <JsonView data={recommender} />
    </ExpandableCard>
  );
};
export const ProductRecommendersSummary = () => {
  const productRecommenders = useProductRecommenders();
  return (
    <React.Fragment>
      <CreateButton
        className="float-right"
        to="/recommenders/product-recommenders/create"
      >
        Create
      </CreateButton>
      <Title>Product Recommenders</Title>

      <hr />
      {productRecommenders.loading && <Spinner>Loading Recommenders</Spinner>}
      {productRecommenders.error && (
        <ErrorCard error={productRecommenders.error} />
      )}
      {productRecommenders.items && productRecommenders.items.length === 0 && (
        <EmptyList>
          <div className="text-muted m-3">
            There are no product recommenders.
          </div>
          <CreateButton to="/recommenders/product-recommenders/create">
            Create
          </CreateButton>
        </EmptyList>
      )}

      {productRecommenders.items &&
        productRecommenders.items.map((pr) => (
          <ProductRecommenderRow key={pr.id} recommender={pr} />
        ))}

      {productRecommenders.pagination && (
        <Paginator {...productRecommenders.pagination} />
      )}
    </React.Fragment>
  );
};
