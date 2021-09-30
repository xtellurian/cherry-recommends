import React from "react";
import { useAccessToken } from "./token";
import {
  fetchHubspotAccount,
  fetchHubspotAppInformationAsync,
  fetchHubspotWebhookBehaviourAsync,
  fetchHubspotPushBehaviourAsync,
  fetchHubspotCrmCardBehaviourAsync,
  fetchHubspotClientAllContactProperties,
  fetchHubspotClientContactEventsAsync,
  fetchHubspotConnectedContactPropertiesAsync,
  fetchHubspotContacts,
} from "../api/hubspotApi";

export const useHubspotAppInformation = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });

  React.useEffect(() => {
    if (token) {
      setState({ loading: true });
      fetchHubspotAppInformationAsync({
        token,
      })
        .then((s) => {
          setState(s);
        })
        .catch((error) => setState({ error }));
    }
  }, [token]);
  return result;
};

export const useHubspotWebhookBehaviour = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      setState({ loading: true });
      fetchHubspotWebhookBehaviourAsync({
        id,
        token,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);
  return result;
};

export const useHubspotPushBehaviourAsync = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      setState({ loading: true });
      fetchHubspotPushBehaviourAsync({
        id,
        token,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);
  return result;
};

export const useHubspotConnectedContactProperties = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      setState({ loading: true });
      fetchHubspotConnectedContactPropertiesAsync({
        id,
        token,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);
  return result;
};

export const useHubspotCrmCardBehaviour = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      setState({ loading: true });
      fetchHubspotCrmCardBehaviourAsync({
        id,
        token,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);
  return result;
};

export const useHubspotAccount = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    if (token && id) {
      setState({ loading: true });
      fetchHubspotAccount({
        success: setState,
        error: (e) => setState({ error: e }),
        token,
        id,
      });
    }
  }, [token, id]);
  return result;
};

export const useHubspotClientAllContactProperties = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    if (token && id) {
      setState({ loading: true });
      fetchHubspotClientAllContactProperties({
        success: setState,
        error: (e) => setState({ error: e }),
        token,
        id,
      });
    }
  }, [token, id]);
  return result;
};

export const useHubspotContactEvents = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    if (token && id) {
      setState({ loading: true });
      fetchHubspotClientContactEventsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);
  return result;
};

export const useHubspotContacts = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    if (token && id) {
      setState({ loading: true });
      fetchHubspotContacts({
        success: setState,
        error: (e) => setState({ error: e }),
        token,
        id,
      });
    }
  }, [token, id]);
  return result;
};
