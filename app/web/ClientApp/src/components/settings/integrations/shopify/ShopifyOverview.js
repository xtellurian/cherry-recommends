import React from "react";
import { useAnalytics } from "../../../../analytics/analyticsHooks";
import { useShopInformation } from "../../../../api-hooks/shopifyApi";
import { useAccessToken } from "../../../../api-hooks/token";
import { shopifyDisconnectAsync } from "../../../../api/shopifyApi";
import { useNavigation } from "../../../../utility/useNavigation";
import {
  AsyncButton,
  ErrorCard,
  Navigation,
  Spinner,
} from "../../../molecules";
import { ConfirmationPopup } from "../../../molecules/popups/ConfirmationPopup";

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

export const ShopifyOverview = ({ integratedSystem }) => {
  const [error, setError] = React.useState();
  const [isOpen, setIsOpen] = React.useState(false);
  const [loading, setLoading] = React.useState(false);

  const { navigate } = useNavigation();
  const { analytics } = useAnalytics();
  const token = useAccessToken();
  const shop = useShopInformation({ id: integratedSystem.id });

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
    setLoading(true);
    shopifyDisconnectAsync({ token, id: integratedSystem.id })
      .then(() => {
        analytics.track("site:settings_integration_shopify_disconnect_success");
        navigate("/");
      })
      .catch((e) => {
        analytics.track("site:settings_integration_shopify_disconnect_failure");
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
          {!isIntegrated && (
            <Navigation
              to={`/settings/integrations/shopifyconnector?state=${integratedSystem.id}`}
            >
              <button className="btn btn-primary">Connect to Shopify</button>
            </Navigation>
          )}
          {isIntegrated && (
            <button className="btn btn-primary" onClick={() => setIsOpen(true)}>
              Disconnect from Shopify
            </button>
          )}
        </div>
      </div>
    </React.Fragment>
  );
};
