import React from "react";
import { useAnalytics } from "../../../../analytics/analyticsHooks";
import { useEnvironmentReducer } from "../../../../api-hooks/environmentsApi";
import { useShopInformation } from "../../../../api-hooks/shopifyApi";
import { useAccessToken } from "../../../../api-hooks/token";
import { setIsDCGeneratorAsync } from "../../../../api/integratedSystemsApi";
import { shopifyDisconnectAsync } from "../../../../api/shopifyApi";
import { AsyncButton, ErrorCard, Spinner } from "../../../molecules";
import { AdvancedOptionsPanel } from "../../../molecules/layout/AdvancedOptionsPanel";
import { SettingRow } from "../../../molecules/layout/SettingRow";
import { ConfirmationPopup } from "../../../molecules/popups/ConfirmationPopup";
import { ToggleSwitch } from "../../../molecules/ToggleSwitch";
import { useTenantName } from "../../../tenants/PathTenantProvider";

const ShopDetails = ({ shop }) => {
  return (
    <table className="table table-borderless">
      <tbody>
        <tr>
          <th className="text-right">Shop</th>
          <td className="text-left">{shop.name}</td>
        </tr>
        <tr>
          <th className="text-right">Domain</th>
          <td className="text-left">{shop.domain}</td>
        </tr>
      </tbody>
    </table>
  );
};

export const ShopifyOverview = ({ integratedSystem, onReload }) => {
  const [error, setError] = React.useState();
  const [isOpen, setIsOpen] = React.useState(false);
  const [loading, setLoading] = React.useState(false);

  const { analytics } = useAnalytics();
  const token = useAccessToken();
  const shop = useShopInformation({ id: integratedSystem.id });
  const tenant = useTenantName();
  const [environment] = useEnvironmentReducer();

  const isIntegrated = integratedSystem.integrationStatus === "ok";

  const renderShopDetails = () => {
    if (isIntegrated && (!shop || shop.loading)) {
      return <Spinner>Loading Shop Details</Spinner>;
    } else if (isIntegrated && shop) {
      return <ShopDetails shop={shop} />;
    } else {
      return null;
    }
  };

  const handleDisconnect = () => {
    if (token && tenant) {
      setLoading(true);
      shopifyDisconnectAsync({
        token,
        tenant: tenant?.tenantName,
        id: integratedSystem.id,
        environment: environment?.id,
      })
        .then(() => {
          analytics.track(
            "site:settings_integration_shopify_disconnect_success"
          );
          if (onReload) {
            onReload();
          }
        })
        .catch((e) => {
          analytics.track(
            "site:settings_integration_shopify_disconnect_failure"
          );
          setError(e);
        })
        .finally(() => {
          setLoading(false);
          setIsOpen(false);
        });
    }
  };

  const handleToggleDCGenerator = (checked) => {
    setIsDCGeneratorAsync({
      id: integratedSystem.id,
      token,
      value: checked,
    })
      .then(() => {
        if (onReload) {
          onReload();
        }
      })
      .catch((e) => {
        setError(e);
      });
  };

  return (
    <React.Fragment>
      <ConfirmationPopup
        isOpen={isOpen}
        setIsOpen={setIsOpen}
        label="Are you sure you want to disconnect from Shopify?"
      >
        <div
          className="btn-group"
          role="group"
          aria-label="Ok or cancel buttons"
        >
          <button
            className="btn btn-secondary"
            onClick={() => setIsOpen(false)}
          >
            Cancel
          </button>
          <AsyncButton
            loading={loading}
            className="btn btn-danger"
            onClick={handleDisconnect}
          >
            OK
          </AsyncButton>
        </div>
      </ConfirmationPopup>
      <div className="row text-center">
        <div className="col">{renderShopDetails()}</div>
        <div className="col">
          <ErrorCard error={error} />
          {isIntegrated && (
            <button className="btn btn-primary" onClick={() => setIsOpen(true)}>
              Disconnect from Shopify
            </button>
          )}
        </div>
      </div>
      <div className="row">
        <div className="col">
          <AdvancedOptionsPanel>
            {integratedSystem.loading && <Spinner />}
            {!integratedSystem.loading && !integratedSystem.error && (
              <SettingRow
                label="Discount Code Generator"
                description="When enabled, promotions campaign invokations will generate discount codes in Shopify. 
                  Only one discount code per promotion can be generated within a single day."
              >
                <ToggleSwitch
                  id="isDiscountCodeGenerator"
                  checked={integratedSystem.isDiscountCodeGenerator}
                  onChange={handleToggleDCGenerator}
                />
              </SettingRow>
            )}
          </AdvancedOptionsPanel>
        </div>
      </div>
    </React.Fragment>
  );
};
