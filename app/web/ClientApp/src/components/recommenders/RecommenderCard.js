import React, { forwardRef } from "react";
import { Card, CardTitle, CardBody } from "reactstrap";
import { Typography } from "../molecules";

export const RecommenderCard = forwardRef(({ title, children }, ref) => {
  return (
    <div ref={ref}>
      <Card className="shadow-sm mt-4">
        <CardBody>
          <Typography variant="h6" className="semi-bold">
            {title}
          </Typography>
          {children}
        </CardBody>
      </Card>
    </div>
  );
});
