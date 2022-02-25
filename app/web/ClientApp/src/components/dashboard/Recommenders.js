import React from "react";
import Tippy from "@tippyjs/react";
import { useItemsRecommenders } from "../../api-hooks/itemsRecommendersApi";
import { useParameterSetRecommenders } from "../../api-hooks/parameterSetRecommendersApi";
import { EmptyState, Spinner } from "../molecules";
import { NoteBox } from "../molecules/NoteBox";
import { Link } from "react-router-dom";
import { EmptyStateText } from "../molecules/empty/EmptyStateText";
import { RecommenderRow } from "../recommenders/RecommenderRow";

const Tooltip = ({ children }) => {
  return <div className="bg-light border rounded p-3">{children}</div>;
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
        itemRec.recommenderSubPath = "promotions-recommenders";
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
                  !hasItems && <Tooltip>First create some promotions</Tooltip>
                }
              >
                <Link to="/recommenders/promotions-recommenders/create">
                  <button disabled={!hasItems} className="btn btn-primary">
                    Create a Recommender
                  </button>
                </Link>
              </Tippy>
            </EmptyState>
          )}
          <div className="text-center text-muted">
            <Link to="/recommenders/promotions-recommenders">
              <button className="btn btn-link btn-sm">View More</button>
            </Link>
          </div>
        </NoteBox>
      </div>
    </>
  );
};
