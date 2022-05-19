import React from "react";
import { EntityField } from "../molecules/EntityField";
import { JsonView } from "../molecules/JsonView";

export const OfferSection = ({ recommendation, customer, business }) => {
  return (
    <React.Fragment>
      {recommendation.customerId ? (
        <EntityField
          entity={customer}
          label="Customer"
          to={{
            pathname: `/customers/detail/${customer.id}`,
            search: null,
          }}
        />
      ) : null}
      {recommendation.businessId ? (
        <EntityField
          entity={business}
          label="Business"
          to={{
            pathname: `/businesses/detail/${business.id}`,
            search: null,
          }}
        />
      ) : null}
      {recommendation.offer ? (
        <div className="mt-2">
          <JsonView data={recommendation.offer} />
        </div>
      ) : null}
    </React.Fragment>
  );
};
