import React from "react";
import { Link } from "react-router-dom";
import { useCustomers } from "../../api-hooks/customersApi";
import { Spinner } from "./Spinner";
import { EntityRow } from "./layout/EntityRow";

export const CustomerListItem = ({ customer }) => {
  return (
    <EntityRow>
      <div className="col">
        <h5>{customer.name || customer.commonId || customer.id}</h5>
      </div>
      <div className="col-3">
        <Link to={`/customers/detail/${customer.id}`}>
          <button className="btn btn-outline-primary btn-block">Details</button>
        </Link>
      </div>
    </EntityRow>
  );
};

export const CustomerList = ({ ids }) => {
  console.log("WARNING: THIS ISNt IMPLEMNTED");
  const trackedUsers = useCustomers({});

  if (!trackedUsers) {
    return <Spinner />;
  }
  if (trackedUsers && trackedUsers.length === 0) {
    return <div className="text-center">The Segment is Empty</div>;
  }
  return trackedUsers.map((u) => (
    <div key={u.id} className="row">
      <div className="col">
        <CustomerListItem customer={u} />
      </div>
    </div>
  ));
};
