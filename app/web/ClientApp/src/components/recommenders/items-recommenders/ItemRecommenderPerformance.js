import React from "react";
import { useParams } from "react-router";
import { usePerformance } from "../../../api-hooks/promotionsRecommendersApi";
import { ErrorCard, Spinner } from "../../molecules";
import { EntityRow } from "../../molecules/layout/EntityRow";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";
import {
  Table,
  TableBody,
  TableHead,
  TableRow,
  Cell,
} from "../../molecules/Table";
import { Link } from "react-router-dom";

const PerformanceTableRow = ({ data, itemsById }) => {
  const item = itemsById[data.itemId];
  if (!item) {
    console.warn("ItemID not found in itemsById");
    return null;
  }

  return (
    <TableRow>
      <Cell>
        <Link to={`/promotions/detail/${data.itemId}`}>{item.name}</Link>
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

const PerformanceTable = ({ reports, itemsById, targetMetric }) => {
  const targetMetricName = targetMetric.name;
  console.log(reports);
  console.log(itemsById);
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
  const performance = usePerformance({ id });
  return (
    <React.Fragment>
      <ItemRecommenderLayout>
        {performance.error && <ErrorCard error={performance.error} />}
        {performance.loading && <Spinner />}
        {performance.performanceByItem && (
          <PerformanceTable
            targetMetric={performance.targetMetric}
            reports={performance.performanceByItem}
            itemsById={performance.itemsById}
          />
        )}
      </ItemRecommenderLayout>
    </React.Fragment>
  );
};

export default Performance;
