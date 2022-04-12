import React from "react";
import { Spinner } from "reactstrap";
import { fetchShopifyAuthorizeUrlAsync } from "../../../../api/shopifyApi";
import { ErrorCard } from "../../../molecules";

const baseUrl = `${window.location.protocol}//${window.location.host}`;

export const ShopifyInstall = () => {
  const [loading, setLoading] = React.useState(true);
  const [error, setError] = React.useState();

  React.useEffect(() => {
    const redirectUrl = encodeURI(`${baseUrl}/_connect/shopify/callback`);
    const qsParams = new URLSearchParams(window.location.search);
    qsParams.set("redirectUrl", redirectUrl);

    fetchShopifyAuthorizeUrlAsync({ qs: qsParams.toString() })
      .then((url) => {
        window.location.href = url;
      })
      .catch((e) => setError(e))
      .finally(() => setLoading(false));
  }, []);

  return (
    <React.Fragment>
      {loading && <Spinner>Loading...</Spinner>}
      <ErrorCard error={error} />
    </React.Fragment>
  );
};
