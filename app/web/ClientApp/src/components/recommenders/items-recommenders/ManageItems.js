import React from "react";
import { useParams } from "react-router";
import {
  useItemsRecommender,
  useItems,
} from "../../../api-hooks/itemsRecommendersApi";
import {
  EmptyList,
  Paginator,
  Spinner,
  Subtitle,
  Title,
  BackButton,
} from "../../molecules";
import { ItemRow } from "../../items/ItemRow";
import { AddItemPopup } from "./AddItemPopup";
import { RemoveItemPopup } from "./RemoveItemPopup";

export const ManageItems = () => {
  const { id } = useParams();
  const recommender = useItemsRecommender({ id });

  const [trigger, setTrigger] = React.useState();
  const items = useItems({ id, trigger });

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
        Add an Item
      </button>
      <BackButton
        className="float-right mr-1"
        to={`/recommenders/items-recommenders/detail/${id}`}
      >
        Recommender
      </BackButton>
      <Title>Manage Items</Title>
      <Subtitle>
        {recommender.loading && <Spinner />}
        {recommender.name}
      </Subtitle>
      <hr />
      {items.loading && <Spinner>Loading Items</Spinner>}
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
              Remove Item
            </button>
          </ItemRow>
        ))}

      {items.pagination && items.pagination.totalItemCount === 0 && (
        <EmptyList>
          There are no items associated with this recommender. All items will be
          used.
          <div className="mt-2 text-center">
            <button
              onClick={() => setIsAddItemPopupOpen(true)}
              className="btn btn-primary"
            >
              Add an Item
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
