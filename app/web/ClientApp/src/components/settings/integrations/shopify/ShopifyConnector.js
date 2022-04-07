import React from "react";
import { useAccessToken } from "../../../../api-hooks/token";
import { useQuery } from "../../../../utility/utility";
import { useIntegratedSystem } from "../../../../api-hooks/integratedSystemsApi";
import { Title } from "../../../molecules/layout";
import { Spinner } from "../../../molecules/Spinner";
import { ErrorCard } from "../../../molecules/ErrorCard";
import { BackButton } from "../../../molecules/BackButton";
import {
  fetchShopifyInstallUrlAsync,
  shopifyConnectAsync,
} from "../../../../api/shopifyApi";
import { useShopifyAppInformation } from "../../../../api-hooks/shopifyApi";
import { InputGroup, TextInput } from "../../../molecules/TextInput";
import { useAnalytics } from "../../../../analytics/analyticsHooks";
import { useParams } from "react-router-dom";
import { useTenantName } from "../../../tenants/PathTenantProvider";
import { useNavigation } from "../../../../utility/useNavigation";

const basePath = `${window.location.protocol}//${window.location.host}`;

const stages = ["READY", "INSTALLING", "SAVING", "COMPLETE"];
const Top = () => {
  return (
    <React.Fragment>
      <BackButton to="/settings/integrations" className="float-right">
        Integrations
      </BackButton>
      <Title>Shopify Installation</Title>
      <hr />
    </React.Fragment>
  );
};

const ProgressView = ({ stage }) => {
  const finished = stage === stages[3];

  return (
    <div className={`card w-50 m-auto ${finished ? "bg-success" : ""}`}>
      <div className="card-body text-center">Installation: {stage}</div>
    </div>
  );
};
const SystemStateView = ({ integratedSystem, force, stateObject }) => {
  const { navigate } = useNavigation();
  if (integratedSystem.loading && !force) {
    return <Spinner>Loading System Details</Spinner>;
  }
  if (integratedSystem.integrationStatus === "ok" || force) {
    return (
      <div className="card w-50 m-auto">
        <div className="card-body text-center bg-success">
          Integration Status: {integratedSystem.integrationStatus ?? "ok"}
          <div>
            <button
              className="btn btn-primary btn-block"
              onClick={() => {
                if (force) {
                  const prefix = stateObject?.tenant
                    ? `/${stateObject?.tenant}`
                    : "";
                  window.location.href = `${basePath}${prefix}/settings/integrations/detail/${stateObject?.id}`;
                } else {
                  navigate(
                    `/settings/integrations/detail/${integratedSystem.id}`
                  );
                }
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
          Integration Status: {integratedSystem.integrationStatus}
        </div>
      </div>
    );
  }
};

export const ShopifyConnector = () => {
  const { id } = useParams();
  const query = useQuery();
  const state = query.get("state");
  const code = query.get("code");
  const shopifyUrl = query.get("shop");

  const defaultTrigger = 0;
  const defaultStage = code && shopifyUrl ? stages[2] : stages[0];

  const [useId, setUseId] = React.useState();
  const [trigger, setTrigger] = React.useState(defaultTrigger);
  const [stage, setStage] = React.useState(defaultStage);
  const [error, setError] = React.useState();
  const [inputShopifyUrl, setInputShopifyUrl] = React.useState("");
  const [connectAnalytics, setConnectAnalytics] = React.useState();
  const [stateObject, setStateObject] = React.useState();
  const [force, setForce] = React.useState(false);

  const token = useAccessToken();
  const { tenantName } = useTenantName();
  const integratedSystem = useIntegratedSystem({
    id: useId,
    trigger: `${tenantName}${trigger}`,
  });
  const { analytics } = useAnalytics();
  const { loading } = useShopifyAppInformation();

  const redirectUri = encodeURI(`${basePath}/_connect/shopify/callback`);
  const initialLoad = trigger === defaultTrigger && stage === defaultStage; // important to prevent multiple api calls
  const showConnect = stage === stages[0] || stage === stages[1];
  React.useEffect(() => {
    let mounted = true;
    if (stateObject && code && shopifyUrl && token && initialLoad) {
      shopifyConnectAsync({
        code: {
          code,
          shopifyUrl,
        },
        id: stateObject?.id,
        token,
        tenant: stateObject?.tenant,
      })
        .then(() => {
          if (mounted) {
            setTrigger(trigger + 1);
            setStage(stages[3]);
            setConnectAnalytics(
              "site:settings_integration_shopify_connect_success"
            );
            if (stateObject?.tenant) {
              setForce(true);
            }
          }
        })
        .catch((e) => {
          if (mounted) {
            setConnectAnalytics(
              "site:settings_integration_shopify_connect_failure"
            );
            setError(e);
          }
        });
    }
    return () => (mounted = false);
  }, [stateObject, code, shopifyUrl, token, trigger, initialLoad]);

  // Move analytics.track away from shopifyConnectAsync to prevent multiple API calls
  React.useEffect(() => {
    if (connectAnalytics) {
      analytics.track(connectAnalytics);
    }
  }, [connectAnalytics, analytics.track]);

  React.useEffect(() => {
    if (integratedSystem.integrationStatus === "ok") {
      setStage(stages[3]);
    }
  }, [integratedSystem.integrationStatus]);

  // // Parse state to object
  React.useEffect(() => {
    if (state) {
      const so = JSON.parse(state);
      setStateObject(so);
      setUseId(so.id);
    } else if (id) {
      setUseId(id);
    }
  }, [state, id]);

  const handleInstall = () => {
    fetchShopifyInstallUrlAsync({
      token,
      id,
      shopifyUrl: inputShopifyUrl,
      redirectUrl: redirectUri,
      tenant: tenantName,
    })
      .then((v) => {
        window.open(v, "_blank");
        setError(undefined);
      })
      .catch(setError);
    setStage(stages[1]);
  };

  return (
    <React.Fragment>
      <Top />
      {loading && <Spinner>Loading Shopify App Information</Spinner>}
      {error && <ErrorCard error={error} />}
      <SystemStateView
        integratedSystem={integratedSystem}
        force={force}
        stateObject={stateObject}
      />
      <ProgressView stage={stage} />
      {showConnect && (
        <React.Fragment>
          <InputGroup className="mt-3 m-1">
            <TextInput
              label="Shopify Shop URL"
              placeholder="https://shopname.myshopify.com"
              type="text"
              value={inputShopifyUrl}
              onChange={(e) => setInputShopifyUrl(e.target.value)}
            />
          </InputGroup>
          <div className="text-center m-5">
            <button
              disabled={loading || !inputShopifyUrl}
              onClick={handleInstall}
              className="btn btn-primary"
            >
              {loading ? "Loading App Info" : "Install Shopify App"}
            </button>
          </div>
        </React.Fragment>
      )}
    </React.Fragment>
  );
};
