import React from "react";
import { useParams } from "react-router-dom";
import { useModelRegistration } from "../../api-hooks/modelRegistrationsApi";

import SwaggerUI from "swagger-ui-react";
import "swagger-ui-react/swagger-ui.css";
import { Spinner } from "../molecules/Spinner";
import { useAccessToken } from "../../api-hooks/token";

export const InvokeModel = () => {
  const { id } = useParams();
  const accessToken = useAccessToken();
  const { model } = useModelRegistration({ id });

  const interceptor = (res) => {
    res.url = `api/models/${id}/invoke`;
    res.headers = {
      ...res.headers,
      Authorization: `Bearer ${accessToken}`,
    };
    return res;
  };
  return (
    <React.Fragment>
      {model && (
        <SwaggerUI spec={model.swagger} requestInterceptor={interceptor} showMutatedRequest={true} />
      )}
      {!model && <Spinner />}
    </React.Fragment>
  );
};
