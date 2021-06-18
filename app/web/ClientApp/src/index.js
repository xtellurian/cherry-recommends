import "bootstrap/dist/css/bootstrap.css";
import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter } from "react-router-dom";
import { Auth0Provider } from "@auth0/auth0-react";
import { fetchAuth0Configuration } from "./api/reactConfigApi";
import App from "./App";

//import registerServiceWorker from './registerServiceWorker';

const baseUrl = document.getElementsByTagName("base")[0].getAttribute("href");
const rootElement = document.getElementById("root");

fetchAuth0Configuration().then((auth0Config) => {
  ReactDOM.render(
    <Auth0Provider
      domain={auth0Config.domain}
      clientId={auth0Config.clientId}
      redirectUri={auth0Config.redirectUri || window.location.origin}
      audience={auth0Config.audience}
      scope={auth0Config.scope}
      skipRedirectCallback={
        // this is a boolean, not just the one path
        window.location.pathname === "/settings/integrations/hubspotconnector"
      }
    >
      <BrowserRouter basename={baseUrl}>
        <App />
      </BrowserRouter>
    </Auth0Provider>,
    rootElement
  );
});

// Uncomment the line above that imports the registerServiceWorker function
// and the line below to register the generated service worker.
// By default create-react-app includes a service worker to improve the
// performance of the application by caching static assets. This service
// worker can interfere with the Identity UI, so it is
// disabled by default when Identity is being used.
//
//registerServiceWorker();
