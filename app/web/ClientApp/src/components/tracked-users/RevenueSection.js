import React, { Suspense } from "react";
import { Link } from "react-router-dom";
import { useTrackedUserRevenueActions } from "../../api-hooks/trackedUserApi";
import { Spinner, ErrorCard, EmptyList } from "../molecules";

const RevenueChart = React.lazy(() =>
  import("../molecules/charts/RevenueChart")
);

export const RevenueSection = ({ trackedUser }) => {
  const revenueActions = useTrackedUserRevenueActions({
    id: trackedUser.id,
    page: 1,
  });
  return (
    <React.Fragment>
      {revenueActions.loading && <Spinner>Loading Revenue</Spinner>}
      {revenueActions.error && <ErrorCard error={revenueActions.error} />}

      {revenueActions.items && revenueActions.items.length > 0 && (
        <Suspense fallback={<Spinner>Loading Chart</Spinner>}>
          <RevenueChart actions={revenueActions.items} />
        </Suspense>
      )}
      {revenueActions.items && revenueActions.items.length === 0 && (
        <EmptyList>
          <div>
            There are no assoicated revenue events. You may need to configure
            your reward settings.
          </div>
          <Link to="/settings/rewards">
            <button className="btn btn-primary mt-3">
              Configure Reward Settings
            </button>
          </Link>
        </EmptyList>
      )}
    </React.Fragment>
  );
};
