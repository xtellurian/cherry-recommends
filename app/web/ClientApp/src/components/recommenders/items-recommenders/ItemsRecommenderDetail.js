import React from "react";
import { Link, useHistory, useParams } from "react-router-dom";
import { useItemsRecommender } from "../../../api-hooks/itemsRecommendersApi";
import {
  deleteItemsRecommenderAsync,
  createItemsRecommenderAsync,
} from "../../../api/itemsRecommendersApi";
import { ItemRow } from "../../items/ItemRow";
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
import { Tabs, TabActivator } from "../../molecules/Tabs";
import { EntityField } from "../../molecules/EntityField";
import { CloneRecommender } from "../utils/CloneRecommender";
import { ArgumentsSection } from "./Arguments";
import { LearningFeatures } from "./LearningFeatures";

const tabs = [
  { id: "detail", label: "Detail" },
  { id: "arguments", label: "Arguments" },
  { id: "features", label: "Learning Features" },
];

export const RecommenderDetail = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const history = useHistory();
  const [trigger, setTrigger] = React.useState();
  const recommender = useItemsRecommender({ id, trigger });
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
      <Tabs defaultTabId={tabs[0].id} tabs={tabs} />
      {recommender.loading && <Spinner>Loading Recommender</Spinner>}
      {recommender.error && <ErrorCard error={recommender.error} />}

      <TabActivator defaultTabId="detail" tabId="detail">
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
            <div className="mt-5">
              <div className="mb-4">
                <Link
                  to={`/recommenders/items-recommenders/manage-items/${id}`}
                >
                  <button className="float-right btn btn-outline-primary">
                    Manage Items
                  </button>
                </Link>
                <Subtitle>Associated Items</Subtitle>
              </div>
              {recommender.items &&
                recommender.items.map((i) => <ItemRow item={i} key={i.id} />)}
              {recommender.items && recommender.items.length === 0 && (
                <EmptyList>
                  This recommender works with all recommendable items.
                </EmptyList>
              )}
            </div>
          </div>
        </div>
      </TabActivator>
      <TabActivator defaultTabId="detail" tabId="arguments">
        <ArgumentsSection recommender={recommender} setTrigger={setTrigger} />
      </TabActivator>
      <TabActivator defaultTabId="detail" tabId="features">
        <LearningFeatures />
      </TabActivator>
    </React.Fragment>
  );
};
