import React from "react";
import { useParams } from "react-router";
import {
  usePromotionsRecommender,
  usePromotions,
} from "../../../../api-hooks/promotionsRecommendersApi";
import {
  EmptyList,
  Paginator,
  Spinner,
  Subtitle,
  Title,
  BackButton,
  PageHeading,
  PrimaryBackButton,
} from "../../../molecules";
import { ItemRow } from "../../../items/ItemRow";
import { AddItemPopup } from "../AddItemPopup";
import { RemoveItemPopup } from "../RemoveItemPopup";
import { ManageNav } from "./Tabs";

export const ManageItems = () => {
  const { id } = useParams();
  const recommender = usePromotionsRecommender({ id });

  const [trigger, setTrigger] = React.useState();
  const items = usePromotions({ id, trigger });

  const [isAddItemPopupOpen, setIsAddItemPopupOpen] = React.useState(false);
  const [isRemoveItemPopupOpen, setIsRemoveItemPopupOpen] =
    React.useState(false);
  const [itemToRemove, setItemToRemove] = React.useState();
  return (
    <React.Fragment>
      <button
        onClick={() => setIsAddItemPopupOpen(true)}
        className="float-right btn btn-primary"
      >
        Add a Promotion
      </button>
      <PrimaryBackButton
        to={{
          pathname: `/recommenders/promotions-recommenders/detail/${id}`,
          search: null,
        }}
      >
        Recommender
      </PrimaryBackButton>
      <PageHeading
        title="Manage Promotions"
        subtitle={recommender.name || "..."}
      />
      <ManageNav id={id} />
      {items.loading && <Spinner>Loading Promotions</Spinner>}
      {items.items &&
        items.items.map((i) => (
          <ItemRow key={i.id} item={i}>
            <button
              onClick={() => {
                setItemToRemove(i);
                setIsRemoveItemPopupOpen(true);
              }}
              className="btn btn-outline-danger"
            >
              Remove Promotion
            </button>
          </ItemRow>
        ))}

      {items.pagination && items.pagination.totalItemCount === 0 && (
        <EmptyList>
          There are no promotions associated with this recommender. All
          promotions will be used.
          <div className="mt-2 text-center">
            <button
              onClick={() => setIsAddItemPopupOpen(true)}
              className="btn btn-primary"
            >
              Add a Promotion
            </button>
          </div>
        </EmptyList>
      )}

      <AddItemPopup
        isOpen={isAddItemPopupOpen}
        setIsOpen={setIsAddItemPopupOpen}
        recommender={recommender}
        onAdded={setTrigger}
      />

      <RemoveItemPopup
        open={isRemoveItemPopupOpen}
        setOpen={setIsRemoveItemPopupOpen}
        recommender={recommender}
        onRemoved={setTrigger}
        item={itemToRemove}
      />

      {items.pagination && <Paginator {...items.pagination} />}
    </React.Fragment>
  );
};
