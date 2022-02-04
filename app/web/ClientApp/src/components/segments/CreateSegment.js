import React from "react";
import { useAnalytics } from "../../analytics/analyticsHooks";
import { Link } from "react-router-dom";
import { Title } from "../molecules/layout";
import { useAccessToken } from "../../api-hooks/token";
import { createSegmentAsync } from "../../api/segmentsApi";

export const CreateSegment = () => {
  const token = useAccessToken();
  const { analytics } = useAnalytics();
  const [name, setName] = React.useState("");
  const [segment, setSegment] = React.useState();
  const submit = () => {
    createSegmentAsync({
      payload: {
        name,
      },
      token,
    })
      .then((r) => {
        analytics.track("site:segment_create_success");
        setSegment(r);
      })
      .catch((e) => {
        analytics.track("site:segment_create_failure");
        alert("error");
      });
  };

  return (
    <React.Fragment>
      <Title>Create a new segment</Title>
      <div className="input-group">
        <input
          type="text"
          className="form-control"
          placeholder="Segment Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
        <button onClick={submit} className="btn btn-primary">
          Create
        </button>
      </div>
      {segment && (
        <div className="text-center mt-5">
          <p>Created segment {segment.name}</p>
          <Link to={`/segments`}>
            <button className="btn btn-success">Go to Segments</button>
          </Link>
        </div>
      )}
    </React.Fragment>
  );
};
