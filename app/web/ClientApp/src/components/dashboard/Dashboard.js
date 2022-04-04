import React from "react";
import { useDashboard } from "../../api-hooks/dataSummaryApi";
import { usePromotions } from "../../api-hooks/promotionsApi";
import { Title, Spinner } from "../molecules";

import { NoteBox } from "../molecules/NoteBox";
import { Recommenders } from "./Recommenders";
import { Items } from "./Promotions";
import { RecentActivity } from "./RecentActivity";

export const Dashboard = () => {
  const dashboard = useDashboard({ scope: null }); // choose null, kind, or type
  const items = usePromotions();
  return (
    <React.Fragment>
      <Title data-qa="title">Dashboard</Title>
      <hr />
      <NoteBox className="mb-3" label="Number of Customers">
        {dashboard.loading && <Spinner />}
        <div className="display-4">{dashboard.totalTrackedUsers}</div>
      </NoteBox>

      <div className="row mb-3">
        <div className="col">
          <Items items={items} />
        </div>
        <div className="col">
          <Recommenders
            hasItems={!items.loading && items.items && items.items.length > 0}
          />
        </div>
      </div>
      <div className="row mb-3">
        <div className="col">
          <RecentActivity />
        </div>
      </div>
    </React.Fragment>
  );
};
