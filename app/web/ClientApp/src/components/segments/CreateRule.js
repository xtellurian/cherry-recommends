import React from "react";
import { useLocation } from "react-router-dom";
import { createRule } from "../../api/rulesApi";

function useQuery() {
  return new URLSearchParams(useLocation().search);
}

export const CreateRule = () => {
  const query = useQuery();
  let initialSegmentId = query.get("segmentId");
  const [rule, setRule] = React.useState({
    name: "",
    segmentId: initialSegmentId,
    eventKey: "",
    eventLogicalValue: "",
  });

  return (
    <React.Fragment>
      <div className="row">
        <div className="col">
          <h3>Create a new segmenting rule</h3>
        </div>
      </div>
      <hr />
      <div className="mb-3">
        <input
          type="text"
          className="form-control"
          placeholder="Rule Name"
          value={rule.name}
          onChange={(e) =>
            setRule({
              ...rule,
              name: e.target.value,
            })
          }
        />
      </div>

      <div>
        <h5>WHEN</h5>
        <div className="input-group">
          <input
            type="text"
            className="form-control"
            placeholder="Event Key"
            value={rule.eventKey}
            onChange={(e) =>
              setRule({
                ...rule,
                eventKey: e.target.value,
              })
            }
          />
          <span className="input-group-addon font-weight-bold ml-3 mr-3">
            =
          </span>
          <input
            type="text"
            className="form-control"
            placeholder="Logical Value"
            value={rule.eventLogicalValue}
            onChange={(e) =>
              setRule({
                ...rule,
                eventLogicalValue: e.target.value,
              })
            }
          />
        </div>
        <div className="row mt-2">
          <div className="col">
            <h5>THEN</h5>
            <p>Add to this segment</p>
            <label>Segment ID</label>
            <input
              className="form-control"
              placeholder="Segment ID"
              value={rule.segmentId}
              onChange={(e) =>
                setRule({
                  ...rule,
                  segmentId: e.target.value,
                })
              }
            />
          </div>
          <div className="col"></div>
        </div>

        <div className="text-right">
          <button
            className="btn btn-primary"
            disabled={
              !rule.eventKey ||
              !rule.eventLogicalValue ||
              !rule.segmentId ||
              !rule.name
            }
            onClick={() => {
              createRule({
                success: () => alert("Created Rule"),
                error: () => alert("Something Broke."),
                payload: rule,
              });
            }}
          >
            Create Rule
          </button>
        </div>
      </div>
    </React.Fragment>
  );
};
