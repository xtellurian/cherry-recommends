import React from "react";
import { useParams } from "react-router";
import {
  usePerformance,
  usePromotionsRecommender,
  useReportImageBlobUrl,
} from "../../../api-hooks/promotionsRecommendersApi";
import { ErrorCard, Navigation, Spinner } from "../../molecules";
import { DisplayReportImage } from "../utils/ViewImagePopup";
import { PromotionRecommenderLayout } from "./PromotionRecommenderLayout";
import {
  Table,
  TableBody,
  TableHead,
  TableRow,
  Cell,
} from "../../molecules/Table";

const PerformanceTableRow = ({ data, itemsById }) => {
  const item = itemsById[data.itemId];
  if (!item) {
    console.warn("ItemID not found in itemsById");
    return null;
  }

  return (
    <TableRow>
      <Cell>
        <Navigation to={`/promotions/detail/${data.itemId}`}>
          {item.name}
        </Navigation>
      </Cell>
      <Cell>{data.targetMetricSum}</Cell>
      <Cell>{data.customerCount}</Cell>
      <Cell>{data.businessCount}</Cell>
      <Cell>{data.recommendationCount}</Cell>
      <Cell>
        {Math.round((100 * data.targetMetricSum) / data.recommendationCount) /
          100}
      </Cell>
    </TableRow>
  );
};

export const PerformanceTable = ({ reports, itemsById, targetMetric }) => {
  const targetMetricName = targetMetric.name;
  console.debug(reports);
  console.debug(itemsById);
  return (
    <Table>
      <TableHead
        headings={[
          "Promotion",
          `Sum ${targetMetricName}`,
          "Customers",
          "Businesses",
          "Recommendations",
          `Sum ${targetMetricName} / # Recommendations`,
        ]}
      />
      <TableBody>
        {reports.map((r, i) => (
          <PerformanceTableRow key={i} data={r} itemsById={itemsById} />
        ))}
      </TableBody>
    </Table>
  );
};

const Performance = () => {
  const { id } = useParams();
  const recommender = usePromotionsRecommender({ id });
  const performance = usePerformance({ id });
  const [reportOpen, setReportOpen] = React.useState(false);
  return (
    <React.Fragment>
      <PromotionRecommenderLayout>
        {performance.error && <ErrorCard error={performance.error} />}
        {performance.loading && <Spinner />}
        {performance.performanceByItem && (
          <PerformanceTable
            targetMetric={performance.targetMetric}
            reports={performance.performanceByItem}
            itemsById={performance.itemsById}
          />
        )}
        <div className="row">
          {!recommender.loading && !recommender.error && (
            <React.Fragment>
              <div className="col text-center">
                <DisplayReportImage
                  id={id}
                  useReportImageBlobUrl={useReportImageBlobUrl}
                />
              </div>
            </React.Fragment>
          )}
        </div>
      </PromotionRecommenderLayout>
    </React.Fragment>
  );
};

export default Performance;
