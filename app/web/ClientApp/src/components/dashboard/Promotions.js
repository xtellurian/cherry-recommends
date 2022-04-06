import React from "react";
import { EmptyState, Spinner } from "../molecules";
import { PromotionRow } from "../promotions/PromotionRow";
import { NoteBox } from "../molecules/NoteBox";
import { EmptyStateText } from "../molecules/empty/EmptyStateText";
import { Navigation } from "../molecules";

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
          {itemList &&
            itemList.map((i) => <PromotionRow key={i.id} promotion={i} />)}

          {!items.loading && itemList.length === 0 && (
            <EmptyState>
              <EmptyStateText>
                You haven't created any promotions.
              </EmptyStateText>
              <Navigation to="/promotions/create">
                <button className="btn btn-primary">Create a Promotion</button>
              </Navigation>
            </EmptyState>
          )}
          <div className="text-center text-muted">
            <Navigation to="/promotions">
              <button className="btn btn-link btn-sm">View More</button>
            </Navigation>
          </div>
        </NoteBox>
      </div>
    </>
  );
};
