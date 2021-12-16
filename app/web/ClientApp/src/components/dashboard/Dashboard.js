import React from "react";
import { useDashboard } from "../../api-hooks/dataSummaryApi";
import { useItems } from "../../api-hooks/recommendableItemsApi";
import { Title, Spinner } from "../molecules";

import { NoteBox } from "../molecules/NoteBox";
import { Recommenders } from "./Recommenders";
import { Items } from "./Items";
import { RecentActivity } from "./RecentActivity";

export const Dashboard = () => {
  const dashboard = useDashboard({ scope: null }); // choose null, kind, or type
  const items = useItems();
  return (
    <React.Fragment>
      <Title>Dashboard</Title>
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
          <Recommenders hasItems={!items.loading && items.items.length > 0} />
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
