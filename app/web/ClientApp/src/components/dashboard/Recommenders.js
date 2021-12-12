import React from "react";
import Tippy from "@tippyjs/react";
import { useItemsRecommenders } from "../../api-hooks/itemsRecommendersApi";
import { useParameterSetRecommenders } from "../../api-hooks/parameterSetRecommendersApi";
import { EmptyState, Spinner } from "../molecules";
import { NoteBox } from "../molecules/NoteBox";
import { EntityRow } from "../molecules/layout/EntityRow";
import { Link } from "react-router-dom";
import { EmptyStateText } from "../molecules/empty/EmptyStateText";

const Tooltip = ({ children }) => {
  return <div className="bg-light border rounded p-3">{children}</div>;
};

const RecommenderRow = ({ recommender }) => {
  return (
    <EntityRow>
      <div className="col text-left">{recommender.name}</div>
      <div className="col-4">
        <Link
          to={`/recommenders/${recommender.recommenderSubPath}/detail/${recommender.id}`}
        >
          <button className="btn btn-outline-primary btn-block">View</button>
        </Link>
      </div>
    </EntityRow>
  );
};
const MAX_LIST_LENGTH = 5;
export const Recommenders = ({ className, hasItems }) => {
  const itemsRecommenders = useItemsRecommenders();
  const parameterRecommenders = useParameterSetRecommenders();
  const [allRecommenders, setAllRecommenders] = React.useState([]);
  const loading = itemsRecommenders.loading || parameterRecommenders.loading;
  React.useEffect(() => {
    if (itemsRecommenders.items && parameterRecommenders.items) {
      for (const itemRec of itemsRecommenders.items) {
        itemRec.recommenderSubPath = "items-recommenders";
        itemRec.uniqueId = `items-${itemRec.id}`;
      }

      for (const parameterRec of parameterRecommenders.items) {
        parameterRec.recommenderSubPath = "parameter-set-recommenders";
        parameterRec.uniqueId = `parameter-${parameterRec.id}`;
      }

      let allRecs = [
        ...itemsRecommenders.items,
        ...parameterRecommenders.items,
      ];
      if (allRecs && allRecs.length > MAX_LIST_LENGTH) {
        allRecs = allRecs.slice(1, MAX_LIST_LENGTH + 1);
      }

      setAllRecommenders(allRecs);
    }
  }, [itemsRecommenders, parameterRecommenders]);
  return (
    <>
      <div className={className}>
        <NoteBox label="Recommenders">
          {loading && <Spinner />}
          {allRecommenders.map((r) => (
            <RecommenderRow key={r.uniqueId} recommender={r} />
          ))}

          {!loading && allRecommenders.length === 0 && (
            <EmptyState>
              <EmptyStateText>
                You haven't created any recommenders.
              </EmptyStateText>
              <Tippy
                content={
                  !hasItems && (
                    <Tooltip>First create some recommendable items</Tooltip>
                  )
                }
              >
                <Link to="/recommenders/items-recommenders/create">
                  <button disabled={!hasItems} className="btn btn-primary">
                    Create a Recommender
                  </button>
                </Link>
              </Tippy>
            </EmptyState>
          )}
          <div className="text-center text-muted">
            <Link to="/recommenders/items-recommenders">
              <button className="btn btn-link btn-sm">View More</button>
            </Link>
          </div>
        </NoteBox>
      </div>
    </>
  );
};
