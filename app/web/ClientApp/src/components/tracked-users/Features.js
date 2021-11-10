import React from "react";
import { useParams } from "react-router-dom";
import { useTrackedUser } from "../../api-hooks/trackedUserApi";
import {
  useTrackedUserFeatures,
  useTrackedUserFeatureValues,
} from "../../api-hooks/featuresApi";
import {
  Title,
  Subtitle,
  Spinner,
  ExpandableCard,
  EmptyList,
  BackButton,
  ErrorCard,
} from "../molecules";
import { CopyableField } from "../molecules/fields/CopyableField";

const FeatureValues = ({ userId, feature }) => {
  const values = useTrackedUserFeatureValues({
    id: userId,
    feature: feature.commonId,
  });
  if (values.loading) {
    return <Spinner>Loading Feature Value</Spinner>;
  } else if (values.error) {
    return <ErrorCard error={values.error} />;
  } else {
    const valueType = values.numericValue ? "Numeric" : "String";
    return (
      <div>
        <CopyableField label="Feature Common Id" value={feature.commonId} />
        <CopyableField label="Current Version" value={values.version} />
        <CopyableField label="Current Value" value={`${values.value}`} />
        <CopyableField label="Value Type" value={valueType} />
      </div>
    );
  }
};
const FeatureValuesRow = ({ userId, feature }) => {
  return (
    <ExpandableCard label={feature.name}>
      <FeatureValues userId={userId} feature={feature} />
    </ExpandableCard>
  );
};

export const Features = () => {
  const { id } = useParams();
  const trackedUser = useTrackedUser({ id });
  const trackedUserFeatures = useTrackedUserFeatures({ id });
  return (
    <React.Fragment>
      <BackButton className="float-right" to={`/tracked-users/detail/${id}`}>
        User Details
      </BackButton>
      <Title>Features</Title>
      <Subtitle>
        {trackedUser.name || trackedUser.commonId || trackedUser.id || "..."}
      </Subtitle>
      <hr />
      {(trackedUser.loading || trackedUserFeatures.loading) && <Spinner />}
      {trackedUserFeatures.length > 0 &&
        trackedUserFeatures.map((f) => (
          <FeatureValuesRow feature={f} userId={id} key={f.id} />
        ))}
      {trackedUserFeatures.length === 0 && (
        <EmptyList>This user has no features.</EmptyList>
      )}
    </React.Fragment>
  );
};
