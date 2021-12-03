import React from "react";
import { EmptyList, Paginator, Title, ErrorCard } from "../../molecules";
import { ClickableRow } from "../../molecules/layout/ClickableRow";

import { useFeatures } from "../../../api-hooks/featuresApi";
import { CreateButtonClassic } from "../../molecules/CreateButton";

const FeatureRow = ({ feature }) => {
  return (
    <ClickableRow
      buttonText="Detail"
      label={feature.name}
      to={`/admin/features/detail/${feature.id}`}
    />
  );
};
export const FeaturesSummary = () => {
  const features = useFeatures();
  return (
    <React.Fragment>
      <CreateButtonClassic className="float-right" to="/admin/features/create">
        Create Feature
      </CreateButtonClassic>
      <Title>Features</Title>
      <hr />
      {features.items && features.items.length === 0 && (
        <EmptyList>There are no features.</EmptyList>
      )}
      {features.error && <ErrorCard error={features.error} />}
      {features.items &&
        features.items.map((f) => <FeatureRow key={f.id} feature={f} />)}

      {features.pagination && <Paginator {...features.pagination} />}
    </React.Fragment>
  );
};
