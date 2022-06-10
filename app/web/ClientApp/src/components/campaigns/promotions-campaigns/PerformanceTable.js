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
            <div className="row">
              <div className="col text-center">
                <DisplayReportImage
                  id={id}
                  useReportImageBlobUrl={useReportImageBlobUrl}
                />
              </div>
            </div>
            <div className="row mt-2">
              <div className="col text-center">
                <CampaignARPOChart reportData={arpoData} />
              </div>
            </div>
            <div className="row mt-2">
              <div className="col text-center">
                <CampaignAPVChart reportData={apvData} />
              </div>
            </div>
            <div className="row mt-2">
              <div className="col text-center">
                <CampaignConversionRateChart reportData={conversionRateData} />
              </div>
            </div>
            <div className="row mt-2">
              <div className="col text-center">
                <CampaignPerformanceChart reportData={performanceReportData} />
              </div>
            </div>
          </React.Fragment>
        )}
      </PromotionCampaignLayout>
    </React.Fragment>
  );
};

export default Reports;
