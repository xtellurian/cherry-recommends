import React from "react";
import { useSegments } from "../../api-hooks/segmentsApi";
import { Spinner } from "../molecules/Spinner";
import { Paginator } from "../molecules/Paginator";
import { EmptyList, ErrorCard, Title } from "../molecules";
import { SegmentRow } from "./SegmentRow";

export const SegmentSummary = () => {
  const segments = useSegments();
  const isEmpty = segments.items && segments.items.length === 0;

  return (
    <div>
      <Title>Segments</Title>
      <hr />
      {segments.loading && <Spinner />}
      {segments.error && <ErrorCard error={segments.error} />}
      {isEmpty && <EmptyList>There are no segments.</EmptyList>}
      <div className="mt-3">
        {segments.items &&
          segments.items.map((s) => <SegmentRow key={s.id} segment={s} />)}
      </div>
      <Paginator {...segments.pagination} />
    </div>
  );
};
