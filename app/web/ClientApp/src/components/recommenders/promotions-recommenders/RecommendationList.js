import React from "react";
import { useParams, Link } from "react-router-dom";
import { Link45deg } from "react-bootstrap-icons";
import {
  Spinner,
  ErrorCard,
  EmptyList,
  ExpandableCard,
  Paginator,
} from "../../molecules";
import { JsonView } from "../../molecules/JsonView";
import { DateTimeField } from "../../molecules/DateTimeField";
import {
  usePromotionsRecommendations,
  usePromotionsRecommender,
} from "../../../api-hooks/promotionsRecommendersApi";
import { BigPopup } from "../../molecules/popups/BigPopup";
import { EntityRow } from "../../molecules/layout/EntityRow";
import { Col } from "../../molecules/layout/Grid";
import { RecommendationRow } from "../../recommendations/RecommendationRow";

const RecommendationRowTemp = ({ recommendation, size }) => {
  return <RecommendationRow recommendation={recommendation} />;
  const dataSubset = {
    recommendationCorrelatorId: recommendation.recommendationCorrelatorId,
    scoredItems: recommendation.scoredItems,
    customer: recommendation.customer,
    recommender: recommendation.recommender,
  };
  const nItems = recommendation.scoredItems?.length || 0;
  const label = `Recommended ${nItems} item(s) for ${
    recommendation.customer.name || recommendation.customer.customerId
  }`;

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
          <JsonView data={dataSubset} />
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
export const ItemsRecommendationList = ({ size }) => {
  const { id } = useParams();
  const recommender = usePromotionsRecommender({ id });
  const recommendations = usePromotionsRecommendations({ id, pageSize: 5 });

  return (
    <React.Fragment>
      {recommendations.loading && <Spinner />}
      {recommendations.error && <ErrorCard error={recommendations.error} />}
      {recommendations.items && recommendations.items.length === 0 && (
        <EmptyList>This recommender is yet to make a recommendation.</EmptyList>
      )}
      {recommendations.items &&
        recommendations.items.map((r) => (
          <RecommendationRowTemp size={size} recommendation={r} key={r.id} />
        ))}
      {recommendations.pagination && size !== "sm" && (
        <Paginator {...recommendations.pagination} />
      )}
    </React.Fragment>
  );
};
