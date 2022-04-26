import React from "react";
import { useAccessToken } from "../../../../api-hooks/token";
import { useQuery } from "../../../../utility/utility";
import { Title } from "../../../molecules/layout";
import { ErrorCard } from "../../../molecules/ErrorCard";
import {
  fetchIntegratedSystemAsync,
  shopifyConnectAsync,
} from "../../../../api/shopifyApi";
import { useAnalytics } from "../../../../analytics/analyticsHooks";
import { useNavigation } from "../../../../utility/useNavigation";
import { Selector } from "../../../molecules";
import { useMemberships } from "../../../../api-hooks/tenantsApi";
import { useEnvironments } from "../../../../api-hooks/environmentsApi";
import { setTenant } from "cherry.ai";
import { InputLabel } from "../../../molecules/fields/InputLabel";
import { useHosting } from "../../../tenants/HostingProvider";
import { Spinner } from "reactstrap";

const baseUrl = `${window.location.protocol}//${window.location.host}`;

const stages = ["READY", "INSTALLING", "SAVING", "COMPLETE"];

const ProgressView = ({ stage }) => {
  const finished = stage === stages[3];

  return (
    <div className={`card w-50 m-auto ${finished ? "bg-success" : ""}`}>
      <div className="card-body text-center">Installation: {stage}</div>
    </div>
  );
};
const SystemStateView = ({ integratedSystem, tenant }) => {
  const { navigate } = useNavigation();
  if (!integratedSystem) {
    return null;
  }
  if (integratedSystem?.integrationStatus === "ok") {
    return (
      <div className="card w-50 m-auto">
        <div className="card-body text-center bg-success">
          Integration Status: {integratedSystem?.integrationStatus}
          <div>
            <button
              className="btn btn-primary btn-block"
              onClick={() => {
                const tenantPath = tenant ? `/${tenant}` : "";
                const environmentQuery = integratedSystem?.environmentId
                  ? `?environmentId=${integratedSystem?.environmentId}`
                  : "";
                navigate({
                  pathname: `${tenantPath}/settings/integrations/detail/${integratedSystem?.id}`,
                  search: environmentQuery,
                  state: { fromDashboard: false },
                });
              }}
            >
              View Integration
            </button>
          </div>
        </div>
      </div>
    );
  } else {
    return (
      <div className="card w-50 m-auto">
        <div className="card-body text-center">
          Integration Status: {integratedSystem?.integrationStatus}
        </div>
      </div>
    );
  }
};

export const ShopifyConnector = () => {
  const query = useQuery();
  const code = query.get("code");
  const shop = query.get("shop");
  const xId = query.get("x-id");
  const xTenant = query.get("x-tenant");
  const xEnvironment = query.get("x-environment");

  const defaultStage = code && shop ? stages[2] : stages[0];

  const [stage, setStage] = React.useState(defaultStage);
  const [error, setError] = React.useState();
  const [integratedSystem, setIntegratedSystem] = React.useState();
  const [membershipOptions, setMembershipOptions] = React.useState([]);
  const [environmentOptions, setEnvironmentOptions] = React.useState([]);
  const [data, setData] = React.useState({
    tenant: undefined,
    environmentId: undefined,
    code: code,
    shop: shop,
  });

  const hosting = useHosting();
  const token = useAccessToken();
  const { analytics } = useAnalytics();
  const memberships = useMemberships();
  const environments = useEnvironments({ trigger: data.tenant });

  const loading = !hosting || memberships.loading || environments.loading;
  const singleOption =
    hosting &&
    !hosting.multitenant &&
    memberships.length === 1 &&
    environments.items &&
    environments.items.length <= 1;
  const showConnect = !singleOption && stage === stages[2];
  const showSelectTenant = memberships.length >= 1 && data.tenant;
  console.debug("showSelectTenant", showSelectTenant);
  React.useEffect(() => {
    if (integratedSystem?.integrationStatus === "ok" && stage !== stages[3]) {
      setStage(stages[3]);
    }
  }, [integratedSystem, stage]);

  React.useEffect(() => {
    if (!memberships.loading) {
      setMembershipOptions(
        memberships.map((_) => ({
          label: _.name,
          value: _.name,
        }))
      );
    }
  }, [memberships]);

  React.useEffect(() => {
    if (!environments.loading && environments.items?.length) {
      setEnvironmentOptions(
        environments.items.map((_) => ({
          label: _.name,
          value: _.id,
        }))
      );
    }
  }, [environments]);

  React.useEffect(() => {
    if (!xId && hosting && !memberships.loading && !environments.loading) {
      // Single tenant and single environment scenario
      if (hosting.multitenant && memberships.length === 1 && !data.tenant) {
        setData({
          ...data,
          tenant: memberships[0].name,
        });

        // Inform SDK what tenant to use in order to get the correct environments
        setTenant(memberships[0].name);
      } else if (
        !hosting.multitenant &&
        environments.items &&
        environments.items.length <= 1
      ) {
        setData({
          ...data,
          force: true,
        });
      }
    }
  }, [memberships, environments, hosting, data.tenant, xId]);

  React.useEffect(() => {
    if (data.force && token) {
      handleConnect();
    }
  }, [data.force, token]);

  React.useEffect(() => {
    if (token && xId) {
      fetchIntegratedSystemAsync({
        token: token,
        id: xId,
        tenant: xTenant,
        environment: xEnvironment,
      })
        .then((v) => {
          setStage(stages[3]);
          setIntegratedSystem({
            ...v,
            integrationStatus: "ok",
          });
          setError();
        })
        .catch((e) => setError(e));
    }
  }, [token, xId, xTenant, xEnvironment]);

  const handleConnect = () => {
    const qsParams = new URLSearchParams(window.location.search);
    shopifyConnectAsync({
      data,
      token,
      tenant: data.tenant,
      qs: qsParams.toString(),
    })
      .then((v) => {
        analytics.track("site:settings_integration_shopify_connect_success");
        if (v.chargeConfirmationUrl) {
          window.location.href = v.chargeConfirmationUrl;
        } else {
          setStage(stages[3]);
          setIntegratedSystem(v.integratedSystem);
          setError();
        }
      })
      .catch((e) => {
        analytics.track("site:settings_integration_shopify_connect_failure");
        setError(e);
      });
  };

  const setSelectedTenant = (o) => {
    setData({
      ...data,
      tenant: o.value,
    });

    // Inform SDK what tenant to use in order to get the correct environments
    setTenant(o.value);
  };

  const setSelectedEnvironment = (o) => {
    setData({
      ...data,
      environmentId: o.value,
    });
  };

  const isConnectDisabled =
    memberships.loading ||
    environments.loading ||
    (hosting && hosting.multitenant && memberships.length >= 1 && !data.tenant);

  return (
    <React.Fragment>
      <Title>Shopify Installation</Title>
      <hr />
      {<ErrorCard error={error} />}
      <SystemStateView
        integratedSystem={integratedSystem}
        tenant={data.tenant ?? xTenant}
      />
      <ProgressView stage={stage} />
      {loading && (
        <div className="text-center mt-3">
          <Spinner>Loading...</Spinner>
        </div>
      )}
      {showConnect && (
        <React.Fragment>
          {showSelectTenant && (
            <div className="mt-2 mb-2 col-6 offset-3">
              <InputLabel required>Tenant</InputLabel>
              <Selector
                placeholder="Select tenant"
                defaultValue={{
                  label: data.tenant,
                  value: data.tenant,
                }}
                onChange={setSelectedTenant}
                options={membershipOptions}
              />
            </div>
          )}
          <div className="mt-2 mb-2 col-6 offset-3">
            <InputLabel>Environment</InputLabel>
            <Selector
              placeholder="Select environment"
              onChange={setSelectedEnvironment}
              options={environmentOptions}
            />
          </div>

          <div className="text-center m-3">
            <button
              disabled={isConnectDisabled}
              onClick={handleConnect}
              className="btn btn-primary"
            >
              Connect
            </button>
          </div>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
