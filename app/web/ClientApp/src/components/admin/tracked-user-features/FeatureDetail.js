import React from "react";
import { useParams } from "react-router-dom";
import { useFeature } from "../../../api-hooks/featuresApi";
import { Title, Subtitle, Spinner, ErrorCard, BackButton } from "../../molecules";
import { CopyableField } from "../../molecules/CopyableField";
export const FeatureDetail = () => {
  const { id } = useParams();
  const feature = useFeature({ id });
  return (
    <React.Fragment>
        <BackButton className="float-right" to="/admin/features">All Features</BackButton>
      <Title>Feature</Title>
      <Subtitle>{feature.name ? feature.name : "..."}</Subtitle>
      <hr />
      {feature.loading && <Spinner />}
      {feature.error && <ErrorCard error={feature.error} />}
      {feature.commonId && (
        <CopyableField label="Common Id" value={feature.commonId} />
      )}
    </React.Fragment>
  );
};
