import React from "react";
import { useParams } from "react-router-dom";
import { useIntegratedSystem } from "../../../../api-hooks/integratedSystemsApi";
import { Spinner } from "../../../molecules";
import { CustomIntegrationDetail } from "./CustomIntegrationDetail";

export const CustomIntegrationComponent = () => {
  const { id } = useParams();
  const integratedSystem = useIntegratedSystem({ id });

  if (integratedSystem.loading) {
    return <Spinner />;
  }

  return (
    <React.Fragment>
      <CustomIntegrationDetail integratedSystem={integratedSystem} />
    </React.Fragment>
  );
};
