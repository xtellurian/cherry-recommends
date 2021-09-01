import React from "react";
import { Link } from "react-router-dom";
import { ExpandableCard } from "../molecules";
import { CopyableField } from "../molecules/CopyableField";

export const ItemRow = ({ item }) => {
  return (
    <ExpandableCard label={item.name}>
      <CopyableField label="Item Id" value={item.commonId} />
      <p>{item.description}</p>
      <Link to={`/recommendable-items/detail/${item.id}`}>
        <button className="float-right btn btn-primary">Details</button>
      </Link>
    </ExpandableCard>
  );
};
