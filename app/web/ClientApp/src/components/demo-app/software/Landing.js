import React from "react";
import { Link, useLocation } from "react-router-dom";
import { Heading } from "./Heading";
import { DemoLayout } from "../DemoLayout";

function useQuery() {
  return new URLSearchParams(useLocation().search);
}

export const LandingPage = () => {
  const query = useQuery();
  const persona = query.get("persona") || "new";

  return (
    <React.Fragment>
      <DemoLayout
        fakeName="Nile"
        info="This is what a Nile customer sees with Four2."
      >
        <Heading />
        <div className="mt-5 text-center">
          <Link to={`/demo/software/cancel?persona=${persona}`}>
            <button className="btn btn-danger">Cancel Subscription</button>
          </Link>
        </div>
      </DemoLayout>
    </React.Fragment>
  );
};
