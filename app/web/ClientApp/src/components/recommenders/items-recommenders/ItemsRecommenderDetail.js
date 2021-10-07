import React from "react";
import { useHistory, useParams } from "react-router-dom";
import { useItemsRecommender } from "../../../api-hooks/itemsRecommendersApi";
import {
  deleteItemsRecommenderAsync,
  createItemsRecommenderAsync,
} from "../../../api/itemsRecommendersApi";
import { ProductRow } from "../../products/ProductRow";
import { useAccessToken } from "../../../api-hooks/token";
import {
  Title,
  Subtitle,
  BackButton,
  Spinner,
  ErrorCard,
  EmptyList,
} from "../../molecules";
import { RecommenderStatusBox } from "../../molecules/RecommenderStatusBox";
import { ActionsButtonUtil } from "../utils/actionsButtonUtil";
import { ConfirmationPopup } from "../../molecules/popups/ConfirmationPopup";
import { CopyableField } from "../../molecules/fields/CopyableField";
import { EntityField } from "../../molecules/EntityField";
import { CloneRecommender } from "../utils/CloneRecommender";

export const RecommenderDetail = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const history = useHistory();
  const recommender = useItemsRecommender({ id });
  const [cloneOpen, setCloneOpen] = React.useState(false);
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [deleteError, setDeleteError] = React.useState();
  const onDeleted = () => {
    history.push("/recommenders/items-recommenders");
  };

  const cloneAsync = (name, commonId) => {
    return createItemsRecommenderAsync({
      token,
      payload: {
        name,
        commonId,
        cloneFromId: recommender.id,
      },
    });
  };

  return (
    <React.Fragment>
      <ActionsButtonUtil
        id={id}
        basePath="/recommenders/items-recommenders"
        setDeleteOpen={setDeleteOpen}
      >
        <ConfirmationPopup
          isOpen={deleteOpen}
          setIsOpen={setDeleteOpen}
          label="Are you sure you want to delete this Recommender?"
        >
          <div className="m-2">{recommender.name}</div>
          {deleteError && <ErrorCard error={deleteError} />}
          <div
            className="btn-group"
            role="group"
            aria-label="Delete or cancel buttons"
          >
            <button
              className="btn btn-secondary"
              onClick={() => setDeleteOpen(false)}
            >
              Cancel
            </button>
            <button
              className="btn btn-danger"
              onClick={() => {
                deleteItemsRecommenderAsync({
                  token,
                  id: recommender.id,
                })
                  .then(() => {
                    setDeleteOpen(false);
                    if (onDeleted) {
                      onDeleted();
                    }
                  })
                  .catch(setDeleteError);
              }}
            >
              Delete
            </button>
          </div>
        </ConfirmationPopup>
      </ActionsButtonUtil>

      <BackButton
        className="float-right mr-1"
        to="/recommenders/items-recommenders"
      >
        Item Recommenders
      </BackButton>
      <Title>Item Recommender</Title>
      <Subtitle>{recommender.name || "..."}</Subtitle>
      <hr />
      {recommender.loading && <Spinner>Loading Recommender</Spinner>}
      {recommender.error && <ErrorCard error={recommender.error} />}

      <div className="row">
        <div className="col-md order-last">
          {!recommender.loading && !recommender.error && (
            <React.Fragment>
              <RecommenderStatusBox recommender={recommender} />
              <button
                className="btn btn-outline-primary btn-block"
                onClick={() => setCloneOpen(true)}
              >
                Clone this Recommender
              </button>
              <ConfirmationPopup
                isOpen={cloneOpen}
                setIsOpen={setCloneOpen}
                label="Clone this recommender?"
              >
                <CloneRecommender
                  recommender={recommender}
                  cloneAsync={cloneAsync}
                  onCloned={(r) =>
                    history.push(
                      `/recommenders/items-recommenders/detail/${r.id}`
                    )
                  }
                />
              </ConfirmationPopup>
            </React.Fragment>
          )}
        </div>
        <div className="col-8">
          {recommender.commonId && (
            <CopyableField label="Common Id" value={recommender.commonId} />
          )}

          {recommender.id && (
            <CopyableField
              label="Invokation URL"
              value={`${window.location.protocol}//${window.location.host}/api/Recommenders/ItemsRecommenders/${recommender.id}/invoke`}
            />
          )}

          {recommender.defaultItem && (
            <EntityField
              label="Default Item"
              entity={recommender.defaultItem}
              to={`/recommendable-items/detail/${recommender.defaultItemId}`}
            />
          )}
          <div className="mt-2">
            <Subtitle>Associated Items</Subtitle>
            {recommender.items &&
              recommender.items.map((p) => (
                <ProductRow product={p} key={p.id} />
              ))}
            {recommender.items && recommender.items.length === 0 && (
              <EmptyList>
                This recommender works with all recommendable items.
              </EmptyList>
            )}
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};
