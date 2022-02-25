import React from "react";
import { useParams } from "react-router-dom";
import { useSegment } from "../../api-hooks/segmentsApi";
import { Title, Subtitle } from "../molecules/layout";
import { Spinner } from "../molecules/Spinner";

export const SegmentMembers = () => {
  let { id } = useParams();

  const { segment } = useSegment({ id });

  if (!segment) {
    return <Spinner />;
  }
  return (
    <div>
      <div className="float-right">
        <small>{id}</small>
      </div>
      <Title>Segment | Members</Title>
      <Subtitle>{segment.name}</Subtitle>
      <hr />
      <div>TODO: show customers here</div>
    </div>
  );
};
