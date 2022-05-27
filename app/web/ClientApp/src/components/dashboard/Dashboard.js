import React from "react";
import { useAnalytics } from "../../analytics/analyticsHooks";
import { useGeneralSummary } from "../../api-hooks/dataSummaryApi";
import { usePromotions } from "../../api-hooks/promotionsApi";
import { Title, Spinner, Navigation } from "../molecules";
import { CardSection, Label, MoreLink } from "../molecules/layout/CardSection";

import { Campaigns } from "./Campaigns";
import { Items } from "./Promotions";
import Metrics from "./Metrics";
import { MembershipRow } from "../tenant-settings/TenantMembers";
import {
  useCurrentTenant,
  useCurrentTenantMemberships,
} from "../../api-hooks/tenantsApi";

import "./dashboard.css";

const Fact = ({ label, to, children }) => {
  return (
    <div className="d-flex justify-content-between my-3 fact">
      <div>
        <div>{label}</div>
        <div className="fact-value">{children}</div>
      </div>
      <div>
        {to ? (
          <Navigation to={to}>
            <span>View All</span>
          </Navigation>
        ) : null}
      </div>
    </div>
  );
};
export const Dashboard = () => {
  const { analytics } = useAnalytics();
  analytics.track("site:dashboard_mounted");
  const tenant = useCurrentTenant();
  const generalSummary = useGeneralSummary();
  const memberships = useCurrentTenantMemberships();
  const items = usePromotions();
  return (
    <React.Fragment>
      <Title data-qa="title">Dashboard</Title>
      <hr />
      <div>
        <CardSection className="p-4 dashboard-this-is">
          This is <span className="tenant-name">{tenant.name}</span>
        </CardSection>
      </div>
      <div className="row">
        <div className="col">
          <CardSection className="p-4">
            {generalSummary.loading ? (
              <Spinner />
            ) : (
              <React.Fragment>
                <Fact label="Number of Customers" to="/customers/customers">
                  {generalSummary.totalCustomers}
                </Fact>
                <Fact label="Events in 24 Hours">
                  {generalSummary.eventCount24Hour}
                </Fact>
                <Fact label="Recommendations in 24 Hours">
                  {generalSummary.recommendationCount24Hour}
                </Fact>
              </React.Fragment>
            )}
          </CardSection>
        </div>
        <div className="col">
          <CardSection className="p-4">
            <Navigation to="/"></Navigation>
            <Label>Team</Label>
            {memberships.loading ? (
              <Spinner />
            ) : (
              memberships.items.map((m) => (
                <MembershipRow membership={m} key={m.userId} />
              ))
            )}
            <MoreLink to="/tenant-settings">Invite More</MoreLink>
          </CardSection>
        </div>
      </div>

      <div className="row mb-3">
        <div className="col">
          <Items items={items} />
        </div>
        <div className="col">
          <Campaigns
            hasItems={!items.loading && items.items && items.items.length > 0}
          />
        </div>
      </div>
      <div>
        <Metrics />
      </div>
    </React.Fragment>
  );
};
