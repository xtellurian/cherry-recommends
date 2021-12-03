import React from "react";
import { Link, useRouteMatch } from "react-router-dom";
import { useSegments } from "../../api-hooks/segmentsApi";
import { DropdownComponent, DropdownItem } from "../molecules/Dropdown";
import { CreateButtonClassic } from "../molecules/CreateButton";
import { Spinner } from "../molecules/Spinner";
import { Paginator } from "../molecules/Paginator";

const SegmentRow = ({ segment }) => {
  let { path } = useRouteMatch();
  return (
    <div className="card">
      <div className="card-body">
        <div className="row">
          <div className="col">{segment.name}</div>
          <div className="col-3 text-right">
            <DropdownComponent title="Options">
              <DropdownItem>
                <Link
                  className="btn btn-secondary btn-block"
                  to={`${path}/${segment.id}/members`}
                >
                  Members
                </Link>
              </DropdownItem>
              <DropdownItem>
                <Link
                  className="btn btn-secondary btn-block"
                  to={`experiments/create?segmentId=${segment.id}`}
                >
                  Create an Experiment
                </Link>
              </DropdownItem>
            </DropdownComponent>
          </div>
        </div>
      </div>
    </div>
  );
};
export const SegmentList = () => {
  let { path } = useRouteMatch();

  const { result } = useSegments();
  return (
    <div>
      <h3>All Segments</h3>
      <hr />
      {result && result.items.map((s) => <SegmentRow key={s.id} segment={s} />)}
      {!result && <Spinner />}
      {result && result.items.length === 0 && (
        <div className="text-center p-5">
          <div>There are no segments.</div>
          <CreateButtonClassic to={`${path}/create`} className="mt-4">
            Create New Segment
          </CreateButtonClassic>
        </div>
      )}
      {result && <Paginator {...result.pagination} />}
    </div>
  );
};
