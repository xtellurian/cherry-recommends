import React from "react";
import { Link, useParams } from "react-router-dom";
import { Link45deg } from "react-bootstrap-icons";
import {
  useParameterSetRecommender,
  useParameterSetRecommendations,
} from "../../../api-hooks/parameterSetRecommendersApi";
import {
  Spinner,
  ErrorCard,
  EmptyList,
  ExpandableCard,
  Paginator,
} from "../../molecules";
import { JsonView } from "../../molecules/JsonView";
import { DateTimeField } from "../../molecules/DateTimeField";
import { BigPopup } from "../../molecules/popups/BigPopup";
import { Col } from "../../molecules/layout/Grid";
import { EntityRow } from "../../molecules/layout/EntityRow";

const RecommendationRow = ({ recommendation, size }) => {
  const dataToShow = {
    customer: recommendation.customer,
    recommendedParameters: JSON.parse(recommendation.modelOutput)
      .recommendedParameters,
    correlatorId: recommendation.recommendationCorrelatorId,
  };
  // dataToShow.modelOutput.CorrelatorId =
  //   recommendation.recommendationCorrelatorId; // patch this value

  let label = `Correlator: ${recommendation.recommendationCorrelatorId}`;
  if (recommendation.trackedUser) {
    label = `Recommendation for ${
      recommendation.trackedUser.name ||
      recommendation.trackedUser.commonId ||
      "user"
    }`;
  }
  const [isOpen, setIsOpen] = React.useState(false);
  return (
    <React.Fragment>
      <BigPopup isOpen={isOpen} setIsOpen={setIsOpen}>
        {recommendation.customer && (
          <Link to={`/customers/detail/${recommendation.customer.id}`}>
            <button className="btn btn-primary float-right">
              View Customer <Link45deg />
            </button>
          </Link>
        )}
        <DateTimeField label="Created On" date={recommendation.created} />
        {label}
        <ExpandableCard label="Data">
          <JsonView data={dataToShow} />
        </ExpandableCard>
      </BigPopup>
      {(!size || size === "lg") && (
        <EntityRow>
          <Col columnClass="col-lg">{label}</Col>
          <Col columnClass="col-lg-4">
            <button
              className="btn btn-outline-primary btn-block"
              onClick={() => setIsOpen(true)}
            >
              Detail
            </button>
          </Col>
        </EntityRow>
      )}
      {size === "sm" && (
        <EntityRow size="sm" tooltip={label}>
          {label}
        </EntityRow>
      )}
    </React.Fragment>
  );
};
export const ParameterRecommendationList = ({ size }) => {
  const { id } = useParams();
  const recommender = useParameterSetRecommender({ id });
  const recommendations = useParameterSetRecommendations({ id, pageSize: 5 });
  return (
    <React.Fragment>
      {recommendations.loading && <Spinner />}
      {recommendations.error && <ErrorCard error={recommendations.error} />}
      {recommendations.items && recommendations.items.length === 0 && (
        <EmptyList>This recommender is yet to make a recommendation.</EmptyList>
      )}
      {recommendations.items &&
        recommendations.items.map((r) => (
          <RecommendationRow size={size} recommendation={r} key={r.id} />
        ))}
      {recommendations.pagination && size !== "sm" && (
        <Paginator {...recommendations.pagination} />
      )}
    </React.Fragment>
  );
};
