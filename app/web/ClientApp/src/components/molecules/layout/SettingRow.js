import React from "react";

import { Typography } from "../Typography";
import { EntityRow } from "./EntityRow";

export const SettingRow = ({ label, description, children }) => {
  return (
    <React.Fragment>
      <EntityRow>
        <div className="col">
          <Typography className="semi-bold">{label}</Typography>
          <Typography className="text-secondary mt-1">{description}</Typography>
        </div>
        <div className="col-lg-6 col-md-8 text-center">{children}</div>
      </EntityRow>
    </React.Fragment>
  );
};
