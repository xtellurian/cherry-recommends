import React, { forwardRef } from "react";
import { Card, CardBody } from "reactstrap";

export const RecommenderCard = forwardRef(({ title, children }, ref) => {
  return (
    <div ref={ref}>
      <Card className="shadow-sm mt-4">
        <CardBody>
          <h3 className="mb-4">{title}</h3>
          {children}
        </CardBody>
      </Card>
    </div>
  );
});
