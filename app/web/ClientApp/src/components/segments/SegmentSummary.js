import React from "react";
import { useSegments } from "../../api-hooks/segmentsApi";
import { Spinner } from "../molecules/Spinner";
import { Paginator } from "../molecules/Paginator";
import { EmptyList } from "../molecules";
import { SegmentRow } from "./SegmentRow";

import Layout, {
  CreateEntityButton,
} from "../molecules/layout/EntitySummaryLayout";

export const SegmentSummary = () => {
  const segments = useSegments();
  const isEmpty = segments.items && segments.items.length === 0;

  return (
    <Layout
      header="Segments"
      error={segments.error}
      createButton={
        <CreateEntityButton to="/customers/segments/create">
          Create a Segment
        </CreateEntityButton>
      }
    >
      {segments.loading && <Spinner />}

      {isEmpty && <EmptyList>There are no segments.</EmptyList>}
      <div className="mt-3">
        {segments.items &&
          segments.items.map((s) => <SegmentRow key={s.id} segment={s} />)}
      </div>
      <Paginator {...segments.pagination} />
    </Layout>
  );
};
