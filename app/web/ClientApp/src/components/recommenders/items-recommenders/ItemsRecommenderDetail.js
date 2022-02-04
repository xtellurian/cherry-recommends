import React from "react";
import { Link, useHistory, useParams } from "react-router-dom";
import {
  useItemsRecommender,
  useReportImageBlobUrl,
} from "../../../api-hooks/itemsRecommendersApi";
import {
  deleteItemsRecommenderAsync,
  createItemsRecommenderAsync,
} from "../../../api/itemsRecommendersApi";
import { ItemRow } from "../../items/ItemRow";
import { useAccessToken } from "../../../api-hooks/token";
import { Subtitle, Spinner, ErrorCard, EmptyList } from "../../molecules";
import { ConfirmationPopup } from "../../molecules/popups/ConfirmationPopup";
import { CopyableField } from "../../molecules/fields/CopyableField";
import { EntityField } from "../../molecules/EntityField";
import { CloneRecommender } from "../utils/CloneRecommender";
import { GettingStartedSection } from "./GettingStartedSection";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";
import { ViewReportImagePopup } from "../utils/ViewImagePopup";

const tabs = [
  { id: "detail", label: "Detail" },
  { id: "arguments", label: "Arguments" },
  { id: "metrics", label: "Learning Metrics" },
];

export const RecommenderDetail = () => {
  return (
    <ItemRecommenderLayout>
      <RecommenderDetailSection />
    </ItemRecommenderLayout>
  );
};
const RecommenderDetailSection = () => {
  const { id } = useParams();
  const token = useAccessToken();
  const history = useHistory();
  const [reportOpen, setReportOpen] = React.useState(false);
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

      {recommender.loading && <Spinner>Loading Recommender</Spinner>}
      {recommender.error && <ErrorCard error={recommender.error} />}

      {!recommender.loading && (
        <div className="row">
          <div className="col">
            <GettingStartedSection recommender={recommender} />
          </div>
        </div>
      )}

      <div className="row">
        <div className="col">
          {recommender.commonId && (
            <CopyableField label="Common Id" value={recommender.commonId} />
          )}

          {recommender.id && (
            <CopyableField
              label="Invokation URL"
              value={`${window.location.protocol}//${window.location.host}/api/Recommenders/ItemsRecommenders/${recommender.id}/invoke`}
            />
          )}

          {recommender.baselineItem && (
            <EntityField
              label="Baseline Item"
              entity={recommender.baselineItem}
              to={`/recommendable-items/detail/${recommender.baselineItemId}`}
            />
          )}

          {recommender.targetMetric && (
            <EntityField
              label="Target Metric"
              entity={recommender.targetMetric}
              to={`/metrics/detail/${recommender.targetMetric.id}`}
            />
          )}
        </div>
      </div>

      <div className="d-flex flex-row-reverse">
        {!recommender.loading && !recommender.error && (
          <React.Fragment>
            <button
              className="btn btn-primary mr-1"
              onClick={() => setReportOpen(true)}
            >
              Show Latest Report
            </button>
            <button
              className="btn btn-outline-primary mr-1"
              onClick={() => setCloneOpen(true)}
            >
              Clone
            </button>
            <button
              className="btn btn-link mr-1"
              onClick={() => setDeleteOpen(true)}
            >
              Delete
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

            <ViewReportImagePopup
              isOpen={reportOpen}
              setIsOpen={setReportOpen}
              id={id}
              useReportImageBlobUrl={useReportImageBlobUrl}
            />
          </React.Fragment>
        )}
      </div>

      <hr />

      <div className="mt-5">
        <div className="mb-4">
          <Link to={`/recommenders/items-recommenders/manage-items/${id}`}>
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
    </React.Fragment>
  );
};
