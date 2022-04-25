import React, { useState } from "react";
import { useParams } from "react-router-dom";
import { TextInput } from "../../../molecules/TextInput";
import { InputGroup } from "reactstrap";
import { ErrorCard, AsyncButton, ExpandableCard } from "../../../molecules";
import { useAccessToken } from "../../../../api-hooks/token";
import { setKlaviyoApiKeysAsync } from "../../../../api/klaviyoApi";
import { useNavigation } from "../../../../utility/useNavigation";
import { useTenantName } from "../../../tenants/PathTenantProvider";

export const KlaviyoConfiguration = ({ integratedSystem }) => {
  const token = useAccessToken();
  const { id } = useParams();
  const [error, setError] = useState();
  const [saving, setSaving] = useState(false);
  const { navigate } = useNavigation();
  const { tenantName } = useTenantName();

  const [apiKeys, setApiKeys] = useState({
    publicKey: "",
    privateKey: "",
  });

  const handleSave = () => {
    setError(null);
    setSaving(true);

    setKlaviyoApiKeysAsync({ token, tenant: tenantName, id, apiKeys })
      .then(() => {
        navigate(`/settings/integrations/detail/${id}`);
      })
      .catch((e) => {
        setError(e);
      })
      .finally(() => setSaving(false));
  };

  return (
    <React.Fragment>
      <ExpandableCard label="Configuration">
        {error && <ErrorCard error={error} />}
        <div className="pt-3">
          <div>These API keys will not be shown again.</div>
          <InputGroup className="m-1">
            <TextInput
              label="Public API Key"
              placeholder="Enter your Klaviyo public API Key here"
              onChange={(e) =>
                setApiKeys({
                  ...apiKeys,
                  publicKey: e.target.value,
                })
              }
            />
          </InputGroup>
          <InputGroup className="m-1">
            <TextInput
              label="Private API Key"
              placeholder="Enter your Klaviyo private API Key here"
              onChange={(e) =>
                setApiKeys({
                  ...apiKeys,
                  privateKey: e.target.value,
                })
              }
            />
          </InputGroup>
          <AsyncButton
            className="float-right mt-3 btn btn-primary"
            loading={saving}
            disabled={!apiKeys?.publicKey || !apiKeys?.privateKey}
            onClick={handleSave}
          >
            Save
          </AsyncButton>
        </div>
      </ExpandableCard>
    </React.Fragment>
  );
};
