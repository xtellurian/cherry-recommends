import React from "react";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";
import { CardSection, Label } from "../../molecules/layout/CardSection";
import { Row, Col } from "../../molecules/layout/Grid";
import { ItemsRecommendationList } from "./RecommendationList";
import { useStatistics } from "../../../api-hooks/itemsRecommendersApi";
import { useParams } from "react-router-dom";
import { TargetFeatureChartLoader } from "../../molecules/charts/TargetFeatureChartLoader";

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
      <TargetFeatureChartLoader />
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
      <ItemRecommenderLayout>
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
      </ItemRecommenderLayout>
    </React.Fragment>
  );
};