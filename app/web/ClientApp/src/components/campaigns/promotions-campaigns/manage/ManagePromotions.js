import React from "react";
import { useParams } from "react-router";
import {
  usePromotionsCampaign,
  usePromotions,
} from "../../../../api-hooks/promotionsCampaignsApi";
import {
  EmptyList,
  Paginator,
  Spinner,
  PageHeading,
  MoveUpHierarchyPrimaryButton,
} from "../../../molecules";
import EntityDetailPageLayout from "../../../molecules/layout/EntityDetailPageLayout";
import { PromotionRow } from "../../../promotions/PromotionRow";
import { AddItemPopup } from "../AddItemPopup";
import { RemoveItemPopup } from "../RemoveItemPopup";
import { ManageNav } from "./Tabs";

export const ManageItems = () => {
  const { id } = useParams();
  const recommender = usePromotionsCampaign({ id });

  const [trigger, setTrigger] = React.useState();
  const items = usePromotions({ id, trigger });

  const [isAddItemPopupOpen, setIsAddItemPopupOpen] = React.useState(false);
  const [isRemoveItemPopupOpen, setIsRemoveItemPopupOpen] =
    React.useState(false);
  const [itemToRemove, setItemToRemove] = React.useState();

  return (
    <EntityDetailPageLayout
      backButton={
        <MoveUpHierarchyPrimaryButton
          to={{
            pathname: `/campaigns/promotions-campaigns/detail/${id}`,
            search: null,
          }}
        >
          Back to Campaign
        </MoveUpHierarchyPrimaryButton>
      }
      header={
        <PageHeading
          title="Manage Promotions"
          subtitle={recommender.name || "..."}
        />
      }
      options={
        <button
          onClick={() => setIsAddItemPopupOpen(true)}
          className="float-right btn btn-primary"
        >
          Add a Promotion
        </button>
      }
    >
      <React.Fragment>
        <ManageNav id={id} />

        {items.loading && <Spinner>Loading Promotions</Spinner>}
        {items.items &&
          items.items.map((i) => (
            <PromotionRow key={i.id} promotion={i}>
              <button
                onClick={() => {
                  setItemToRemove(i);
                  setIsRemoveItemPopupOpen(true);
                }}
                className="btn btn-outline-danger"
              >
                Remove Promotion
              </button>
            </PromotionRow>
          ))}

        {items.pagination && items.pagination.totalItemCount === 0 && (
          <EmptyList>
            There are no promotions associated with this campaign. All
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
    </EntityDetailPageLayout>
  );
};
