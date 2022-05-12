import React from "react";

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
import CreatePageLayout from "../molecules/layout/CreatePageLayout";
import { useNavigation } from "../../utility/useNavigation";

export const CreateSegment = () => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
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
        navigate("/segments");
      })
      .catch((e) => {
        analytics.track("site:segment_create_failure");
        setError(e);
      })
      .finally(() => setLoading(false));
  };

  return (
    <React.Fragment>
      <CreatePageLayout
        createButton={
          <AsyncButton
            loading={loading}
            className="btn btn-primary"
            onClick={handleCreate}
          >
            Create
          </AsyncButton>
        }
        backButton={
          <MoveUpHierarchyPrimaryButton to="/segments">
            Back to Segments
          </MoveUpHierarchyPrimaryButton>
        }
        header={<PageHeading title="Create a new Segment" />}
      >
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
      </CreatePageLayout>
    </React.Fragment>
  );
};
