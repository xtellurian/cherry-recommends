import React from "react";
import { useParams, Link } from "react-router-dom";
import { useSegment } from "../../api-hooks/segmentsApi";
import { useRules } from "../../api-hooks/rulesApi";
import { Spinner } from "../../components/molecules/Spinner";

const NewRuleButton = ({ segmentId }) => {
  return (
    <React.Fragment>
      <Link to={`/segments/create-rule?segmentId=${segmentId}`}>
        <button className="btn btn-primary">New Rule</button>
      </Link>
    </React.Fragment>
  );
};

const RuleRow = ({ rule }) => {
  return (
    <div className="card">
      <div className="card-header">{rule.name}</div>
      <div className="card-body">{JSON.stringify(rule)}</div>
    </div>
  );
};
export const SegmentRules = () => {
  const { id } = useParams();
  const { segment } = useSegment({ id });
  const { rules } = useRules({ segmentId: id });

  if (!rules && !segment) {
    return <Spinner />;
  }

  return (
    <React.Fragment>
      <div className="row">
        <div className="col text-capitalize">
          {segment && <h3>{segment.name} | Rules</h3>}
        </div>
        <div className="col-3 text-right">
          {segment && <NewRuleButton segmentId={segment.id} />}
        </div>
      </div>
      <hr />
      <div>
        {rules && rules.map((rule) => <RuleRow key={rule.id} rule={rule} />)}
      </div>
      {segment && (!rules || rules.length === 0) && (
        <div className="text-center p-5">
          <div>There are no rules.</div>
          <div className="mt-4">
            <NewRuleButton segmentId={segment.id} />
          </div>
        </div>
      )}
    </React.Fragment>
  );
};
