import React from "react";
import { useParams } from "react-router";
import {
  useAPVReport,
  useARPOReport,
  useOfferConversionRateReport,
  usePerformance,
  usePerformanceReport,
  usePromotionsCampaign,
  useReportImageBlobUrl,
} from "../../../api-hooks/promotionsCampaignsApi";
import {
  ErrorCard,
  ExpandableCard,
  Navigation,
  Spinner,
} from "../../molecules";
import { DisplayReportImage } from "../utils/ViewImagePopup";
import { PromotionCampaignLayout } from "./PromotionCampaignLayout";
import {
  Table,
  TableBody,
  TableHead,
  TableRow,
  Cell,
} from "../../molecules/Table";
import CampaignARPOChart from "../../molecules/charts/CampaignARPOChart";
import CampaignConversionRateChart from "../../molecules/charts/CampaignConversionRateChart";
import CampaignAPVChart from "../../molecules/charts/CampaignAPVChart";
import CampaignPerformanceChart from "../../molecules/charts/CampaignPerformanceChart";

const PerformanceTableRow = ({ data, itemsById }) => {
  const item = itemsById[data.itemId];
  if (!item) {
    console.warn("ItemID not found in itemsById");
    return null;
  }

  return (
    <TableRow>
      <Cell>
        <Navigation to={`/promotions/promotions/detail/${data.itemId}`}>
          {item.name}
        </Navigation>
      </Cell>
      <Cell>{data.customerCount}</Cell>
      <Cell>{data.businessCount}</Cell>
      <Cell>{data.recommendationCount}</Cell>
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
        headings={["Promotion", "Customers", "Businesses", "Recommendations"]}
      />
      <TableBody>
        {reports.map((r, i) => (
          <PerformanceTableRow key={i} data={r} itemsById={itemsById} />
        ))}
      </TableBody>
    </Table>
  );
};

const Reports = () => {
  const { id } = useParams();
  const recommender = usePromotionsCampaign({ id });
  const performance = usePerformance({ id });
  const arpoData = useARPOReport({ id });
  const apvData = useAPVReport({ id });
  const conversionRateData = useOfferConversionRateReport({ id });
  const performanceReportData = usePerformanceReport({ id });
  return (
    <React.Fragment>
      <PromotionCampaignLayout>
        {performance.error && <ErrorCard error={performance.error} />}
        {performance.loading && <Spinner />}
        {performance.performanceByItem && (
          <PerformanceTable
            targetMetric={performance.targetMetric}
            reports={performance.performanceByItem}
            itemsById={performance.itemsById}
          />
        )}
        {!recommender.loading && !recommender.error && (
          <React.Fragment>
            <div className="mt-2">
              <DisplayReportImage
                id={id}
                useReportImageBlobUrl={useReportImageBlobUrl}
              />
            </div>
            <div className="mt-2">
              <ExpandableCard label="Average Revenue per Offer">
                <CampaignARPOChart reportData={arpoData} />
              </ExpandableCard>
            </div>
            <div className="mt-2">
              <ExpandableCard label="Average Basket Size">
                <CampaignAPVChart reportData={apvData} />
              </ExpandableCard>
            </div>
            <div className="mt-2">
              <ExpandableCard label="Conversion Rate">
                <CampaignConversionRateChart reportData={conversionRateData} />
              </ExpandableCard>
            </div>
            <div className="mt-2">
              <ExpandableCard label="Additional Revenue">
                <CampaignPerformanceChart reportData={performanceReportData} />
              </ExpandableCard>
            </div>
          </React.Fragment>
        )}
      </PromotionCampaignLayout>
    </React.Fragment>
  );
};

export default Reports;
