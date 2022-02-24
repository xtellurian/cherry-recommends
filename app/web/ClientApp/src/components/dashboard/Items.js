import React from "react";
import { Link } from "react-router-dom";
import { EmptyState, Spinner } from "../molecules";
import { ItemRow } from "../items/ItemRow";
import { NoteBox } from "../molecules/NoteBox";
import { EmptyStateText } from "../molecules/empty/EmptyStateText";

const MAX_LIST_LENGTH = 5;
export const Items = ({ className, items }) => {
  let itemList = items.items;

  if (itemList && itemList.length > MAX_LIST_LENGTH) {
    itemList = itemList.slice(1, MAX_LIST_LENGTH + 1);
  }

  return (
    <>
      <div className={className}>
        <NoteBox label="Promotions">
          {items.loading && <Spinner />}
          {itemList && itemList.map((i) => <ItemRow key={i.id} item={i} />)}

          {!items.loading && itemList.length === 0 && (
            <EmptyState>
              <EmptyStateText>
                You haven't created any promotions.
              </EmptyStateText>
              <Link to="/promotions/create">
                <button className="btn btn-primary">Create a Promotion</button>
              </Link>
            </EmptyState>
          )}
          <div className="text-center text-muted">
            <Link to="/promotions">
              <button className="btn btn-link btn-sm">View More</button>
            </Link>
          </div>
        </NoteBox>
      </div>
    </>
  );
};
