import React from "react";
import { Link } from "react-router-dom";
import { useProducts } from "../../api-hooks/productsApi";
import {
  Title,
  EmptyList,
  Spinner,
  ErrorCard,
  ExpandableCard,
  Paginator,
} from "../molecules";
import { CreateButton } from "../molecules/CreateButton";
import { CopyableField } from "../molecules/CopyableField";

const ProductRow = ({ product }) => {
  return (
    <ExpandableCard label={product.name}>
      <CopyableField label="Product Id" value={product.commonId} />
      <p>{product.description}</p>
      <Link to={`/products/detail/${product.id}`}>
        <button className="float-right btn btn-primary">Details</button>
      </Link>
    </ExpandableCard>
  );
};
export const ProductSummary = () => {
  const products = useProducts();
  return (
    <React.Fragment>
      <CreateButton className="float-right" to="/products/create">
        Create Product
      </CreateButton>
      <Title>Product Catalogue</Title>
      <hr />
      {products.loading && <Spinner>Loading Products</Spinner>}
      {products.items && products.items.length === 0 && (
        <EmptyList>
          <div>No Existing Products</div>
          <CreateButton to="/products/create">
            Create Product
          </CreateButton>
        </EmptyList>
      )}
      {products.error && <ErrorCard error={products.error} />}
      {products.items &&
        products.items.map((p) => <ProductRow key={p.id} product={p} />)}

      {products.pagination && <Paginator {...products.pagination} />}
    </React.Fragment>
  );
};
