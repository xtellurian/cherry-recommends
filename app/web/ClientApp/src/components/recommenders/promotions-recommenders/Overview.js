import React from "react";
import { PromotionRecommenderLayout } from "./PromotionRecommenderLayout";
import { CardSection, Label } from "../../molecules/layout/CardSection";
import { Row, Col } from "../../molecules/layout/Grid";
import { ItemsRecommendationList } from "./RecommendationList";
import { useStatistics } from "../../../api-hooks/promotionsRecommendersApi";
import { useParams } from "react-router-dom";
import { TargetMetricChartLoader } from "../../molecules/charts/TargetMetricChartLoader";

const LatestRecommendations = ({ id }) => {
  return (
    <CardSection>
      <Label>Latest Recommendations</Label>

      <ItemsRecommendationList size="sm" />
    </CardSection>
  );
};
const ResultsChart = ({ id }) => {
  return (
    <CardSection>
      <Label>Average Metric per Recommendation</Label>
      <TargetMetricChartLoader />
    </CardSection>
  );
};
const StatCard = ({ label, value }) => {
  return (
    <CardSection>
      <Label>{label}</Label>
      <div className="text-center">{value}</div>
    </CardSection>
  );
};
export const Overview = () => {
  const { id } = useParams();
  const statistics = useStatistics({ id });
  return (
    <React.Fragment>
      <PromotionRecommenderLayout>
        <Row>
          <Col columnClass="col-lg-6">
            <div className="container">
              <Row>
                <Col columnClass="col-lg-6">
                  <LatestRecommendations />
                </Col>
                <Col>
                  <StatCard
                    label="Customers Recommended"
                    value={statistics.numberCustomersRecommended}
                  />
                  <StatCard
                    label="Invokations"
                    value={statistics.numberInvokations}
                  />
                </Col>
              </Row>
            </div>
          </Col>
          <Col>
            <ResultsChart />
          </Col>
        </Row>
      </PromotionRecommenderLayout>
    </React.Fragment>
  );
};
