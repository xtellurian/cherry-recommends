import React from "react";
import { useProducts } from "../../api-hooks/productsApi";
import { Title, EmptyList, Spinner, ErrorCard, Paginator } from "../molecules";
import { NoteBox } from "../molecules/NoteBox";
import { CreateButtonClassic } from "../molecules/CreateButton";

import { ProductRow } from "./ProductRow";
import { Link } from "react-router-dom";

export const ProductSummary = () => {
  const products = useProducts();
  return (
    <React.Fragment>
      <CreateButtonClassic className="float-right" to="/products/create">
        Create Product
      </CreateButtonClassic>
      <Title>Product Catalogue</Title>
      <hr />
      <div className="m-3">
        <NoteBox label="Deprecation Warning">
          <div className="m-2">
            Products are obsolete. Please use Recommendable Items.
          </div>
          <Link to="/recommendable-items">
            <button className="btn btn-primary">
              Go to recommendable items
            </button>
          </Link>
        </NoteBox>
      </div>
      {products.loading && <Spinner>Loading Products</Spinner>}
      {products.items && products.items.length === 0 && (
        <EmptyList>
          <div>No Existing Products</div>
          <CreateButtonClassic to="/products/create">Create Product</CreateButtonClassic>
        </EmptyList>
      )}
      {products.error && <ErrorCard error={products.error} />}
      {products.items &&
        products.items.map((p) => <ProductRow key={p.id} product={p} />)}

      {products.pagination && <Paginator {...products.pagination} />}
    </React.Fragment>
  );
};
