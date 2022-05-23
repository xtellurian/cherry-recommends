import React from "react";

import { useAnalytics } from "../../analytics/analyticsHooks";
import { useAccessToken } from "../../api-hooks/token";
import { createSegmentAsync } from "../../api/segmentsApi";
import { TextInput } from "../molecules/TextInput";
import {
  ErrorCard,
  MoveUpHierarchyPrimaryButton,
  PageHeading,
} from "../molecules";
import CreatePageLayout, {
  CreateButton,
} from "../molecules/layout/CreatePageLayout";
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
        navigate("/customers/segments");
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
          <CreateButton
            label="Create Segment"
            loading={loading}
            onClick={handleCreate}
          />
        }
        backButton={
          <MoveUpHierarchyPrimaryButton to="/customers/segments">
            Back to Segments
          </MoveUpHierarchyPrimaryButton>
        }
        header={<PageHeading title="Create a Segment" />}
      >
        {error ? <ErrorCard error={error} /> : null}
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
      </CreatePageLayout>
    </React.Fragment>
  );
};
