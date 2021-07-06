import React from "react";
import { Link } from "react-router-dom";
import { ExpandableCard } from "../molecules";
import { CopyableField } from "../molecules/CopyableField";

export const ProductRow = ({ product }) => {
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
