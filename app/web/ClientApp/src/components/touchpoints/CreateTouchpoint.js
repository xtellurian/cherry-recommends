import React from "react";
import { Title } from "../molecules/PageHeadings";
import { createTouchpointMetadata } from "../../api/touchpointsApi";
import { useAccessToken } from "../../api-hooks/token";

export const CreateTouchpoint = () => {
  const token = useAccessToken();
  const [touchpoint, setTouchpoint] = React.useState({
    commonId: "",
    name: "",
  });
  return (
    <React.Fragment>
      <Title>Create a Touchpoint</Title>
      <hr />

      <label className="form-label">Enter a name for the touchpoint.</label>
      <div className="input-group">
        <input
          type="text"
          className="form-control"
          placeholder="Unique ID"
          value={touchpoint.commonId}
          onChange={(e) =>
            setTouchpoint({
              ...touchpoint,
              commonId: e.target.value,
            })
          }
        />
        <input
          type="text"
          className="form-control"
          placeholder="Name"
          value={touchpoint.name}
          onChange={(e) =>
            setTouchpoint({
              ...touchpoint,
              name: e.target.value,
            })
          }
        />
        <button
          className="btn btn-primary"
          onClick={() => {
            createTouchpointMetadata({
              success: () => alert("created a touchpoint!"),
              error: (e) => alert(e),
              payload: touchpoint,
              token,
            });
          }}
        >
          Create
        </button>
      </div>
    </React.Fragment>
  );
};
