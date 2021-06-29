import React, { Suspense } from "react";
import { useAccessToken } from "../../../api-hooks/token";
import { Spinner } from "../../molecules";
import "swagger-ui-react/swagger-ui.css";

const SwaggerUI = React.lazy(() => import("swagger-ui-react"));

export const TestAzureMLClassifier = ({ model }) => {
  const accessToken = useAccessToken();
  const interceptor = (res) => {
    res.url = `api/models/AzureSingleClassClassifier/${model.id}/invoke`;
    res.headers = {
      ...res.headers,
      Authorization: `Bearer ${accessToken}`,
    };
    return res;
  };
  return (
    <React.Fragment>
      <Suspense fallback={<Spinner>Loading Swagger UI</Spinner>}>
        <SwaggerUI
          spec={model.swagger}
          requestInterceptor={interceptor}
          showMutatedRequest={true}
        />
      </Suspense>
    </React.Fragment>
  );
};
