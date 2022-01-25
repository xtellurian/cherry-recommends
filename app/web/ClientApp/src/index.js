import "bootstrap/dist/css/bootstrap.css";
import React from "react";
import ReactDOM from "react-dom";
import Modal from "react-modal";
import { BrowserRouter } from "react-router-dom";
import { Auth0Provider } from "@auth0/auth0-react";
import { fetchAuth0ConfigurationAsync } from "./api/reactConfigApi";
import { FunloaderContainer } from "./components/molecules/fullscreen/FunLoader";
import { CherrySecret } from "./components/molecules/fullscreen/CherrySecret";
import { FunError } from "./components/molecules/fullscreen/FunError";
import EnvironmentStore from "./contexts/EnvironmentStore";
import { Auth0ProviderWrapper } from "./components/auth0/Auth0ProviderWrapper";
import App from "./App";

//import registerServiceWorker from './registerServiceWorker';
Modal.setAppElement("#root");

const baseUrl = document.getElementsByTagName("base")[0].getAttribute("href");
const rootElement = document.getElementById("root");

ReactDOM.render(<FunloaderContainer />, rootElement);

if (window.location.host.startsWith("hidden-cherry-secret.local.zone")) {
  // if we're at cherry, then don't render the app yet
  ReactDOM.render(<CherrySecret />, rootElement);
} else {
  fetchAuth0ConfigurationAsync()
    .then((x) => new Promise((resolve) => setTimeout(() => resolve(x), 500))) // ensure you wait half a second for the loading screen
    .then((auth0Config) => {
      ReactDOM.render(
        <>
          <BrowserRouter basename={baseUrl}>
            <Auth0ProviderWrapper auth0Config={auth0Config}>
              <EnvironmentStore>
                <App />
              </EnvironmentStore>
            </Auth0ProviderWrapper>
          </BrowserRouter>
        </>,
        rootElement
      );
    })
    .catch((e) => {
      console.log(e);
      ReactDOM.render(<FunError />, rootElement);
    });
}

// Uncomment the line above that imports the registerServiceWorker function
// and the line below to register the generated service worker.
// By default create-react-app includes a service worker to improve the
// performance of the application by caching static assets. This service
// worker can interfere with the Identity UI, so it is
// disabled by default when Identity is being used.
//
//registerServiceWorker();
