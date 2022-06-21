import React from "react";
import { useAccessToken } from "../../../../api-hooks/token";
import { useQuery } from "../../../../utility/utility";
import { saveHubspotCodeAsync } from "../../../../api/hubspotApi";
import { Spinner } from "../../../molecules/Spinner";
import { useNavigation } from "../../../../utility/useNavigation";

const basePath = `${window.location.protocol}//${window.location.host}`;
const redirectUri = `${basePath}/_connect/hubspot/callback`;

export const HubspotCallback = () => {
  const query = useQuery();
  const code = query.get("code");
  const token = useAccessToken();
  const [loading, setLoading] = React.useState(false);

  const state = JSON.parse(query.get("state"));
  const integratedSystemId = state?.id;
  const tenant = state?.tenant;
  const { navigate } = useNavigation();

  React.useEffect(() => {
    if (code && token && integratedSystemId) {
      setLoading(true);
      saveHubspotCodeAsync({
        code,
        redirectUri,
        integratedSystemId,
        token,
        tenant,
      }).finally(() => {
        const tenantPath = tenant ? `/${tenant}` : "";
        setLoading(false);
        navigate({
          pathname: `${tenantPath}/settings/integrations/hubspotconnector`,
          search: `?state=${integratedSystemId}`,
        });
      });
    }
  }, [code, token, integratedSystemId, tenant]);

  return (
    <React.Fragment>
      {loading && <Spinner>Loading Hubspot App Information</Spinner>}
    </React.Fragment>
  );
};
