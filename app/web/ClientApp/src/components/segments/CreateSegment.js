import React from "react";
import { useHistory } from "react-router-dom";

import { useAnalytics } from "../../analytics/analyticsHooks";
import { useAccessToken } from "../../api-hooks/token";
import { createSegmentAsync } from "../../api/segmentsApi";
import { InputGroup, TextInput } from "../molecules/TextInput";
import {
  AsyncButton,
  ErrorCard,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
} from "../molecules";

export const CreateSegment = () => {
  const token = useAccessToken();
  const history = useHistory();
  const { analytics } = useAnalytics();
  const [loading, setLoading] = React.useState(false);
  const [segment, setSegment] = React.useState();
  const [error, setError] = React.useState();
  const handleCreate = () => {
    setLoading(true);
    createSegmentAsync({
      payload: {
        name: segment.name,
      },
      token,
    })
      .then((r) => {
        analytics.track("site:segment_create_success");
        history.push("/segments");
      })
      .catch((e) => {
        analytics.track("site:segment_create_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };

  return (
    <React.Fragment>
      <div>
        <MoveUpHierarchyPrimaryButton to="/segments">
          Back to Segments
        </MoveUpHierarchyPrimaryButton>
        <PageHeading title="Create a new Segment" showHr />
        {error && <ErrorCard error={error} />}
        <InputGroup>
          <TextInput
            label="Segment Name"
            placeholder="Enter segment name here"
            value={segment?.name}
            onChange={(e) =>
              setSegment({
                ...segment,
                name: e.target.value,
              })
            }
          />
        </InputGroup>
        <div className="mt-4">
          <AsyncButton
            loading={loading}
            className="btn btn-primary"
            onClick={handleCreate}
          >
            Create
          </AsyncButton>
        </div>
      </div>
    </React.Fragment>
  );
};
